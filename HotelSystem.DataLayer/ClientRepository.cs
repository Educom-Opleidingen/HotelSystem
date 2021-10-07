using HotelSystem.HotelDbContext;
using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelSystem.DataLayer
{
    public class ClientRepository
    {
        public HotelContext Context { get; }

        public ClientRepository(HotelContext context)
        {
            Context = context;
        }

        public bool HasClients()
        {
            return Context.Clients.Any(); 
        }

        public IEnumerable<Client> GetAllClients()
        {
            return Context.Clients.ToList();
        }

        public void StoreClient(Client client)
        {
            Context.Clients.Add(client);
            Context.SaveChanges();
        }

        public void ChangeClient(int? clientId, Client updatedInfo)
        {
            Client SelectedClient = GetClient(clientId);

            SelectedClient.FirstName = updatedInfo.FirstName;
            SelectedClient.LastName = updatedInfo.LastName;
            SelectedClient.Birthdate = updatedInfo.Birthdate;
            SelectedClient.Account = updatedInfo.Account;
            SelectedClient.Room = updatedInfo.Room;
            Context.SaveChanges();
        }

        public void RemoveClient(int? clientId)
        {
            Client SelectedClient = GetClient(clientId);
            Context.Clients.Remove(SelectedClient);
            Context.SaveChanges();
        }

        /// <summary>
        /// Get's the client from the context, thows exception if not found
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>The Client</returns>
        private Client GetClient(int? clientId)
        {
            if (!clientId.HasValue)
            {
                throw new DataLayerException("No client selected");
            }
            Client SelectedClient = Context.Clients.Find(clientId.Value);

            if (SelectedClient == null)
            {
                throw new DataLayerException(string.Format("Client with id {0} not found", clientId.Value));
            }

            return SelectedClient;
        }

    }
}
