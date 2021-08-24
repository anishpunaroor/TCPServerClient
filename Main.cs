static void Main(string[] args)
{
    MakeClientCallToServer("Just wanted to say hello");
    MakeClientCallToServer("Just wanted to say hello");
    MakeClientCallToServer("Just wanted to say hello");

    string msg; 
    for (int i = 0; i < 100; i++)
    {
        msg = string.Format("I'll not be ignored! (round {0})", i); 
        ThreadPool.QueueUserWorkItem(new WaitCallback(MakeClientCallToServer), msg); 
    } 

    Console.WriteLine("\n Press any key to continue...");
    Console.Read)(); 
}

static void MakeClientCallToServer(object objMsg)
{
    string msg = (string)objMsg; 
    MyTcpClient client = new MyTcpClient(IPAddress.Loopback, 55555); 
    client.ConnectToServer(msg); 
}