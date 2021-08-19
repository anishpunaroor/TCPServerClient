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

}
