using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        private static Socket lastSocket;
        private static List<Client> clients;

        public static void Main()
        {
            clients = new List<Client>();

            IPAddress ipAddress = IPAddress.Parse("192.168.1.5");
            TcpListener listener = new TcpListener(ipAddress, 8000);
            listener.Start();

            while (true)
            {
                try
                {
                    Socket socket = listener.AcceptSocket();
                    lastSocket = socket;
                    var clientService = new Thread(new ThreadStart(createClient));
                    clientService.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error... ");
                    Console.ReadKey();
                }
            }
        }

        private static void createClient()
        {
            Client client = new Client("", lastSocket);
            clients.Add(client);

            while (client.IsAlive)
            {
                byte[] bites = new byte[100];
                int numberOfBits = client.Socket.Receive(bites);

                StringBuilder textBuilder = new StringBuilder();

                for (int i = 0; i < numberOfBits; i++)
                    textBuilder.Append(Convert.ToChar(bites[i]));

                string text = textBuilder.ToString();

                if (client.Name == "")
                {
                    client.Name = text;
                    Console.WriteLine($"New client was added {client.Name}");
                    newMessageReceived(client, $"New client was added {client.Name}");
                }
                else
                {
                    Console.WriteLine($"{client.Name}:{text}");
                    newMessageReceived(client, $"{client.Name}:{text}");
                }
            }
        }

        private static void newMessageReceived(Client client, string message)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            var text = encoding.GetBytes(message);
            try
            {
                clients.ForEach(c =>
                {
                    if (c.Name != client.Name)
                    {
                        c.Socket.Send(text, text.Length, 0);
                    }
                });
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }
    }
}
