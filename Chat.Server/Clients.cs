using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server
{
    public static class Clients
    {
        private static List<Client> _allClients = new List<Client>();
        public static void AddClient(Client client) => _allClients.Add(client);
        public static void RemoveClient(Client client) => _allClients.Remove(client);
        public static List<Client> GetClientsExceptOne(Client client)
            => _allClients.Where(x => x != client).ToList();
         
    }
}
