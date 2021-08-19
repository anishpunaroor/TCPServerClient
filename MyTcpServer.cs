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
            // Stop listening. 
            StopListening(); 
        }
    }
    
}
