using System.Net.Sockets;

namespace Server
{
    class Client
    {
        public string Name { get; set; }
        public Socket Socket { get; set; }
        public bool IsAlive { get; set; }

        public Client(string name, Socket socket)
        {
            this.Name = name;
            this.Socket = socket;
            IsAlive = true;
        }
    }
}
