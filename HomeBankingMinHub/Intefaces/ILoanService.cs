using HomeBankingMinHub.DTOs;

namespace HomeBankingMinHub.Intefaces
{

    public interface ILoanService
    {
        List<LoanDTO> GetLoans();
        string CreateClientLoan(LoanApplicationDTO loanApplicationDTO, string email);
    }
}
