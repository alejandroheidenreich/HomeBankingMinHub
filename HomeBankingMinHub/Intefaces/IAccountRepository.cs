using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account client);
        Account FindById(long id);
    }
}
