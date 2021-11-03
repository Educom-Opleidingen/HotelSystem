using HotelSystem.DataLayer;
using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelSystem.Test
{
    internal class TestClientRepository : IClientRepository
    {
        public List<Client> Clients { get; } = new List<Client>();

        public void StoreClient(Client client)
        {
            Clients.Add(client);
        }

        public void ChangeClient(int? clientId, Client updatedInfo)
        {
            Client selectedClient = Clients.Find(c => c.Id == clientId);
            selectedClient.FirstName = updatedInfo.FirstName;
            selectedClient.LastName = updatedInfo.LastName;
            selectedClient.Birthdate = updatedInfo.Birthdate;
            selectedClient.Account = updatedInfo.Account;
            selectedClient.Room = updatedInfo.Room;
        }

        public void RemoveClient(int? clientId)
        {
            var foundClient = Clients.Find(c => c.Id == clientId);
            Clients.Remove(foundClient);
        }

        public IEnumerable<Client> GetAllClients()
        {
            return Clients;
        }

        public bool HasClients()
        {
            return Clients.Any();
        }
    }
}
