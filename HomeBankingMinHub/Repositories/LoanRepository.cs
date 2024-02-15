using HomeBankingMindHub.Models;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public Loan FindById(long id)
        {
            return FindByCondition(card => card.Id == id).FirstOrDefault();
        }

        public IEnumerable<Loan> GetAll()
        {
            return FindAll().ToList();
        }
    }
}
