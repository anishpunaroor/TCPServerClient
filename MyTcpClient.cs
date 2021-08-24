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


}