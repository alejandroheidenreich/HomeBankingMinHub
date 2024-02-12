using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
        IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
