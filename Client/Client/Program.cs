using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        static TcpClient tcpClient;

        static void Main(string[] args)
        {
            try
            {
                tcpClient = new TcpClient();
                Console.WriteLine("Connecting.....");
                Console.WriteLine("Connected");
                tcpClient.Connect("192.168.1.5", 8000);




                var readConsole = new Thread(new ThreadStart(ReadConsole));
                var readFromServer = new Thread(new ThreadStart(ReadFromServer));
                readConsole.Start();
                readFromServer.Start();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        static void ReadConsole()
        {
            String message;
            do
            {
                message = Console.ReadLine();

                Stream stream = tcpClient.GetStream();

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] bites = encoding.GetBytes(message);

                stream.Write(bites, 0, bites.Length);

            } while (message.Length != 0);
        }

        static void ReadFromServer()
        {
            do
            {
                Stream stream = tcpClient.GetStream();

                byte[] bites = new byte[100];
                int k = stream.Read(bites, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bites[i]));
                }
                Console.WriteLine();

            } while (true);
        }
    }
}
