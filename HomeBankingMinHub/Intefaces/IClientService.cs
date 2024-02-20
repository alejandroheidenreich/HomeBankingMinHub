using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IClientService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        string CreateNewClient(NewClientDTO client);
        string CreateCurrentAccount(string email);
        List<AccountDTO> GetCurrentAllAccounts(string email);
        string CreateCurrentCard(NewCardDTO newCard, string email);
        ClientDTO GetCurrent(string email);
    }
}
