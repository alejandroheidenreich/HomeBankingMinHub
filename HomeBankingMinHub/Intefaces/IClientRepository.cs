﻿using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
    }
}