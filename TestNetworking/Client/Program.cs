using System;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace Client
{

    class Client
    {
        static Thread ecrire;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ecrire = new Thread(() =>
            {
                lire();
            });
            ecrire.Start();
            int i = 0;
            while (true)
            {
                //Console.WriteLine(i);
                if (i % 50 == 0)
                {
                    Thread.Sleep(1000);
                }
                
                i++ ;
                if (i >= 100450)
                {
                    ecrire.Join();
                }
            }
            
            /*string serverIp = "127.0.0.1"; // Server IP
            int serverPort = 12345; // Port number

            // Connect to the TCP server
            TcpClient tcpClient = new TcpClient(serverIp, serverPort);
            NetworkStream stream = tcpClient.GetStream();

            Console.WriteLine("Connected to server. Waiting for messages...");

            // Read messages from the server
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // If no data is read, client is disconnected

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Message from server: " + message);
            }

            tcpClient.Close();*/
        }
        static int v = int.MaxValue;
        static void lire()
        {
            
            while (true)
            {
                v--;
                Console.WriteLine(v);
                //string message = Console.ReadLine();
                //Console.WriteLine(message);
            }
            
        }
    }

}
