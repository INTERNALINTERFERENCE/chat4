using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Client
{
    public class Program
    {        
        private static void Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55555);
            var tcpClient = new TcpClient();

            Console.Write("Your name: ");
            var userName = Console.ReadLine();

            tcpClient.Connect(endPoint);
            var stream = tcpClient.GetStream();
            var thread = new Thread(ListenMessages);
            thread.Start(stream);

            Console.WriteLine($"Welcome, {userName}");
            try
            {
                while (true)
                {                 
                    var message = Console.ReadLine();
                    var bytes = Encoding.UTF8.GetBytes(message);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                stream?.Close();
                tcpClient?.Close();
            }
        }

        public static void ListenMessages(object data)
        {
            var stream = (NetworkStream)data;
            try
            {
                while (true)
                {
                    string message = GetMessage(stream);
                    Console.WriteLine(message);
                }
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.Message);
            }
        }

        public static string GetMessage(NetworkStream stream)
        {
            var buffer = new byte[128];
            var stringBuilder = new StringBuilder();
            do
            {
                int count = stream.Read(buffer, 0, 128);
                stringBuilder.Append(Encoding.UTF8
                    .GetString(buffer, 0, count));
            }
            while (stream.DataAvailable);
            return stringBuilder.ToString();
        }
    }
}
