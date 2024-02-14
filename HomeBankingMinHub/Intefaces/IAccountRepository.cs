using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
        Account FinByNumber(string number);
        IEnumerable<Account> FindByClientId(long clientId);
    }
}
