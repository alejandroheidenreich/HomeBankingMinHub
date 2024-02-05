using HomeBankingMindHub.Models;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMinHub.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext){}

   
        public IEnumerable<Transaction> GetAllTransactions()
        {
            throw new NotImplementedException();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }

        Transaction ITransactionRepository.FindById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
