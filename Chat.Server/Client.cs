using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;
using Microsoft.VisualBasic.CompilerServices;

namespace Chat.Server
{
    public class Client
    {
        public static string Name { get; set; } = "alex";
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        //static List<ServerUser> _allUsers = new List<ServerUser>();    

        //private static int _id = 1;
        private Client(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _stream = tcpClient.GetStream();
        }

        public static void AddClient(object tcpClient/*, string name*/)
        {
            var client = new Client((TcpClient)tcpClient);
            //ServerUser user = new ServerUser
            //{
            //    Id = _id,
            //    Name = name
            //};
            //_id++;
            Console.WriteLine(($"{Name} connected to the server!"));
            //_allUsers.Add(user);
            Clients.AddClient(client);
            client.StartReceivingMessage();
        }
    
        //public static void RemoveClient(int id)
        //{
        //    var user = _allUsers.FirstOrDefault(x => x.Id == id);
        //    if (user != null)
        //    {
        //        _allUsers.Remove(user);
        //        Console.WriteLine(($"{Name} disconnected from this server"));
        //    }
        //}

        public static void RemoveClient(string name)
        {
            Console.WriteLine($"{name} disconnected from this server");
        }

        private void StartReceivingMessage()    
        {
            int count = 0;
            var json = string.Empty;
            using (var fs = File.OpenRead("Words.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var config = JsonConvert.DeserializeObject<JsonConfig>(json);

            try
            {
                while (true)
                {                  
                    string message = GetMessage();
                    if(message.Contains(config.badWord))
                    {
                        Console.WriteLine($"{Name}: {message.Replace(config.badWord, "***")}");
                        count++;
                        if (count == 3) { RemoveClient(Name); }                                             
                    }
                    else
                    {                    
                        Console.WriteLine($"{Name}: {message}");
                        Clients.GetClientsExceptOne(this)
                            .ForEach(x => x.SendMessage($"{Name}: {message}"));
                    }                  
                }              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally 
            {
                _stream?.Close(); 
                _tcpClient?.Close(); 
            };
        }

        private string GetMessage()
        {
            var buffer = new byte[128];
            var stringBuilder = new StringBuilder();
            do
            {
                int count = _stream.Read(buffer, 0, 128);
                stringBuilder.Append(Encoding.UTF8
                    .GetString(buffer, 0, count));                                    
            }
            while (_stream.DataAvailable);
            return stringBuilder.ToString();
        }

        public void SendMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}
