class MyTcpServer
{
    #region Private Members
    private TcpListener _listener = null; 
    private IPAddress _address; 
    private int _port; 
    private bool _listening = false; 
    #endregion

    #region Constructor
    public MyTcpServer(IPAddress address, int port)
    {
        _port = port; 
        _address = address; 
    }
    #endregion 

    #region Properties
    public IPAddress Address
    {
        get { return _address; }
    }

    public int Port 
    {
        get { return _port; }
    }

    public bool Listening 
    {
        get { return _listening; }
    }
    #endregion

    #region Public Methods
    public void Listen() 
    {
        try
        {
            lock (_syncRoot)
            {
                _listener = new TcpListener(_address, _port);

                // Start the server.
                _listener.Start(); 

                // Begin listening.
                _listening = true; 
            }

            do 
            {
                Trace.Write("Looking for the client..."); 

                // Wait for a connection.
                TcpClient newClient = _listener.AcceptTcpClient(); 
                Trace.WriteLine("Connected to new client.");

                // Queue a request to take care of the client. 
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessClient), newClient); 

            }
            while (_listening); 
        }
        catch (SocketException se)
        {
            Trace.WriteLine("SocketException: " + se.ToString());
        }
        finally 
        {
            StopListening(); 
        }
    }

    public void StopListening() 
    {
        if (_listening)
        {
            lock (_syncRoot)
            {
                // Stop listening. 
                _listening = false; 

                _listener.Stop(); 
            }
        }
    }
    #endregion 

    #region Private Methods
    private void ProcessClient(object client)
    {
        TcpClient newClient = (TcpClient)client; 
        try 
        {
            // Buffer for data. 
            byte[] bytes = new byte[1024]; 
            StringBuilder clientData = new StringBuilder(); 

            // Use stream to talk with client. 
            using (NetworkStream ns = newClient.GetStream())
            {
                ns.ReadTimeout = 60000; 

                // Receive data sent by client. 
                int bytesRead = 0; 
                do 
                {
                    try
                    {
                        bytesRead = ns.Read(bytes, 0, bytes.Length);
                        if (bytesRead > 0) 
                        {
                            // Translate data to ASCII string and append. 
                            clientData.Append(Encoding.ASCII.GetString(bytes, 0, bytesRead));
                            ns.ReadTimeout = 1000; 
                        }                     
                    }
                    catch (IOException ioe)
                    {
                        // Data has been retrieveed. 
                        Trace.WriteLine("Read timed out: " + ioe.ToString());
                        bytesRead = 0; 
                    }
                }
                while (bytesRead > 0); 

                Trace.WriteLine("Client says: " + clientData.ToString()); 
                bytes = Encoding.ASCII.GetBytes("Thanks call again!"); 

                // Send response back. 
                ns.Write(bytes, 0, bytes.length); 
            }
        }
        finally 
        {
            if (newClient != null)
                newClient.Close(); 
        }
    }
    #endregion 

    class Program 
    {
        static MyTcpServer server = null; 
        static void Main(string[] args)
        {
            // Run server on different thread.
            ThreadPool.QueueUserWorkItem(RunServer); 

            Console.WriteLine("Press Esc to stop the server ..."); 
            ConsoleKeyInfo cki; 
            while(true)
            {
                cki = Console.ReadKey(); 
                if (cki.Key == ConsoleKey.Escape)
                    break; 
            }
        }

        static void RunServer(object stateInfo) 
        {
            server = new MyTcpServer(IPAddress.Loopback,55555); 
            server.Listen(); 
        }
    }
}
