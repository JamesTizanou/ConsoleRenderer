using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define server details
            string serverIp = "127.0.0.1"; // Local server IP
            int serverPort = 12345; // Port number for the server

            // Create a TCP listener to accept client connections
            TcpListener tcpListener = new TcpListener(IPAddress.Parse(serverIp), serverPort);
            tcpListener.Start();
            Console.WriteLine("Server is listening on port " + serverPort);

            List<TcpClient> clients = new List<TcpClient>(); // List of connected clients

            // Accept client connections in a separate thread
            Thread acceptThread = new Thread(() => AcceptClients(tcpListener, clients));
            acceptThread.Start();

            // Simulate broadcasting a message to all clients
            while (true)
            {
                Console.WriteLine("Enter message to broadcast: ");
                string message = Console.ReadLine();

                if (!string.IsNullOrEmpty(message))
                {
                    BroadcastMessage(message, clients);
                }
            }
        }

        // Method to accept client connections
        static void AcceptClients(TcpListener tcpListener, List<TcpClient> clients)
        {
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("New client connected!");

                // Start a new thread to handle communication with the client
                Thread clientThread = new Thread(() => HandleClient(client, clients));
                clientThread.Start();
            }
        }

        // Method to handle communication with the client
        static void HandleClient(TcpClient client, List<TcpClient> clients)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received message from client: {message}");
                }
                catch (Exception)
                {
                    break;
                }
            }

            // Remove client from the list if it disconnects
            clients.Remove(client);
            client.Close();
            Console.WriteLine("Client disconnected.");
        }

        // Method to broadcast a message to all connected clients
        static void BroadcastMessage(string message, List<TcpClient> clients)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Broadcasted message to a client.");
                }
                catch (Exception)
                {
                    // If the client is no longer connected, remove it
                    clients.Remove(client);
                    client.Close();
                    Console.WriteLine("A client has disconnected, and will be removed.");
                }
            }
        }
    }
}
