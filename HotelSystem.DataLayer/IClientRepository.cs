using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSystem.DataLayer
{
    public interface IClientRepository
    {
        void StoreClient(Client client);

        void ChangeClient(int? clientId, Client updatedInfo);

        void RemoveClient(int? clientId);

        IEnumerable<Client> GetAllClients();

        bool HasClients();
    }
}
