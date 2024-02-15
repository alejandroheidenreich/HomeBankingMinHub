using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
