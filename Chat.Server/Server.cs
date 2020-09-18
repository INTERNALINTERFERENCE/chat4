using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Chat.Server
{
    public class Server
    {
        private IPEndPoint _endPoint;
        private TcpListener _tcpListener;
        private Server(string adress, int port)
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(adress), port);
            _tcpListener = new TcpListener(_endPoint);
            _tcpListener.Start();
        }

        public static void RunServer(object adress)
        {
            var data = ((string, int))adress;
            var server = new Server(data.Item1, data.Item2);
            server.WaitForClients();
        }

        private void WaitForClients()
        {
            while (true)
            {
                var tcpClient = _tcpListener.AcceptTcpClient();
                var thread = new Thread(Client.AddClient);
                thread.Start(tcpClient);               
            }
        }
    }
}
