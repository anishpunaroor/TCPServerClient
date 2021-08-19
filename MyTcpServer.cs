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

    

}
