using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Classes
{
    class Server
    {
        public uint Port { get; init; }
        public List<Socket> Clients = new List<Socket>();
        private TcpListener Listener { get; init; }

        public Server(uint port)
        {
            Port = port;
            IPAddress ipAd = IPAddress.Parse("127.0.0.1"); //use local m/c IP address, and use the same in the client
            /* Initializes the Listener */
            Listener = new TcpListener(ipAd, (int)Port);
        }

        public void Start()
        {
            Listener.Start();
            Console.WriteLine($"The server is running at port {Port}...");
            Console.WriteLine("The local End point is :" + Listener.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");
            //Clients.Add(new Client(Port));
            while (true)
            {
                if (Listener.Pending())
                {
                    Socket s = Listener.AcceptSocket();
                    Clients.Add(s);
                    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                }
                byte[] b = new byte[100];

                foreach (var client in Clients)
                {
                    if (/*check if there's a message*/ true)
                    {
                        int k = client.ReceiveAsync(b, SocketFlags.None).ContinueWith(() =>
                        {

                        }); //client.Receive(b);
                        Console.WriteLine("Recieved...");
                        for (int i = 0; i < k; i++)
                            Console.Write(Convert.ToChar(b[i]));
                        //ASCIIEncoding asen = new ASCIIEncoding();
                        //s.Send(asen.GetBytes("The string was recieved by the server."));
                        //Console.WriteLine("\nSent Acknowledgement");
                    }
                }
            }
        }

        /*public void Listen()
        {
            
        }
        public void ServerMain()
        {
            try
            {
                
                /* clean up */
        /*s.Close();
        myList.Stop();
    }
    catch (Exception e)
    {
        Console.WriteLine("Error..... " + e.StackTrace);
    }
}*/
    }

    class Client
    {
        private TcpClient _Client { get; init; }
        private uint ServerPort { get; init; }
        public Client(uint serverPort)
        {
            _Client = new TcpClient();
            ServerPort = serverPort;
            try
            {
                Console.WriteLine("Connecting.....");
                _Client.Connect("127.0.0.1", (int)ServerPort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void SendMessage(string msg = "allo")
        {
            try
            {
                Console.Write("Enter the string to be transmitted : ");
                String? str = Console.ReadLine();
                Stream stm = _Client.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("Transmitting.....");
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                //int k = stm.Read(bb, 0, 100);
                //Console.WriteLine("apres le read");
                //for (int i = 0; i < k; i++)
                // Console.Write(Convert.ToChar(bb[i]));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        /*public static void ClientMain()
        {
            try
            {
                
               
                tcpclnt.Close();
            }
            
        }*/
    }
}
