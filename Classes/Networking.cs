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
                

                foreach (var client in Clients)
                {
                    if (client.Available > 0)
                    {
                        byte[] b = new byte[client.Available];
                        int k = client.Receive(b);
                        Console.WriteLine("Recieved...");
                        string message = "";
                        for (int i = 0; i < k; i++)
                            message += (Convert.ToChar(b[i]));
                        Console.WriteLine(message);
                        Clients.ForEach(localclient =>
                        {
                            if (localclient.RemoteEndPoint != client.RemoteEndPoint)
                            {
                                // Pour que le serveur renvoie le message aux autres clients, mais ca ne fonctionne pas
                                /*
                                ASCIIEncoding asen = new ASCIIEncoding();
                                localclient.Send(asen.GetBytes(message));*/
                            }
                        });
                    }
                }
            }
        }
    }

    class Client
    {
        private TcpClient _Client { get; init; }
        private uint ServerPort { get; init; }
        public string Nom { get; init; } = "invité";
        public Client(uint serverPort)
        {
            _Client = new TcpClient();
            Console.WriteLine("Quel est votre nom?: ");
            //string? nom = Console.ReadLine();
            //if (nom != null) Nom = nom;
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

        public void SendMessage()
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
    }
}
