using System;
using System.Net.Sockets;
using System.Threading;

namespace Chat.Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Run server...");
            var thread = new Thread(Server.RunServer);
            thread.Start(("127.0.0.1", 55555));
        }
    }
}
