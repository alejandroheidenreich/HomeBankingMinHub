using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        void Save(Transaction transaction);
        Transaction FindById(long id);
    }
}
