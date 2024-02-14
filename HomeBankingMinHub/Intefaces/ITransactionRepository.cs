using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}
