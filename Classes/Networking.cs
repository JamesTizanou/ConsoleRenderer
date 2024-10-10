using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Classes
{
    abstract class Server
    {
        public static uint Port { get; } = 8001;
        public static void ServerMain()
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse("127.0.0.1"); //use local m/c IP address, and use the same in the client
                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, (int)Port);
                /* Start Listeneting at the specified port */
                myList.Start();
                Console.WriteLine($"The server is running at port 8001...");
                Console.WriteLine("The local End point is :" + myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");
                Socket s = myList.AcceptSocket();
                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                byte[] b = new byte[100];
                int k = s.Receive(b);
                Console.WriteLine("Recieved...");
                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));
                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server."));
                Console.WriteLine("\nSent Acknowledgement");
                /* clean up */
                s.Close();
                myList.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }

    class Client
    {
        public static void ClientMain()
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
                tcpclnt.Connect("127.0.0.1", 8001); // use the ipaddress as in the server program
                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");
                String str = Console.ReadLine();
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("Transmitting.....");
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                Console.WriteLine("apres le read"); 
                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));
                tcpclnt.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
