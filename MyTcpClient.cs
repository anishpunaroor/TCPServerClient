class MyTcpClient
{
    private TcpClient _client = null; 
    private IPAddress _address;
    private int _port;
    private IPEndPoint _endpoint = null; 

    public MyTcpClient(IPAddress address, int port)
    {
        _address = address; 
        _port = port; 
        _endPoint = new IPEndPoint(_address, _port); 
    }

    public void ConnectToServer(string msg); 
    {
        try 
        {
            _client = new TcpClient(); 
            _client.Connect(_endPoint); 


            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            using (NetworkStream ns = _client.GetStream()) 
            {
                // Send the message. 
                Trace.WriteLine("Sending message to server: " + msg); 
                ns.Write(bytes, 0, bytes.Length); 

                bytes = new byte[1024]; 

                // Display response. 
                int bytesRead = ns.Read(bytes, 0, bytes.Length); 
                string ServerResponse = Encoding.ASCII.GetString(bytes, 0, bytesRead); 
                Trace.WriteLine("Server said: " + serverResponse); 
            }
        }
        catch (SocketException se)
        {
            Trace.WriteLine("There was an error talking to the server: " + se.ToString()); 
        }
        finally 
        {
            if (_client != null)
                _client.Close(); 
        }
    }
}