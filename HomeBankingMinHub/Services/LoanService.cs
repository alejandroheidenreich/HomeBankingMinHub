using HomeBankingMindHub.Models;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Services
{
    public class LoanService : ILoanService
    {
        private ILoanRepository _loanRepository;
        private IClientRepository _clientRepository;
        private IClientLoanRepository _clientLoanRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
        public LoanService(ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {

            _loanRepository = loanRepository;
            _clientRepository = clientRepository;
            _clientLoanRepository = clientLoanRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public string CreateClientLoan(LoanApplicationDTO loanApplicationDTO, string email)
        {
            var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

            if (loan == null)
            {
                return "Non-existent loan.";
            }

            if (loanApplicationDTO.ToAccountNumber == string.Empty)
            {
                return "Destination account not provided.";
            }

            Account toAccount = _accountRepository.FinByNumber(loanApplicationDTO.ToAccountNumber);
            if (toAccount == null)
            {
                return "The destination account does not exist.";
            }

            Client client = _clientRepository.FindByEmail(email);
            if (toAccount.ClientId != client.Id)
            {
                return "The provided account does not belong to the current user.";
            }


            if (loanApplicationDTO.Payments.IsNullOrEmpty())
            {
                return "Payment not provided.";
            }

            if (!ValidateLoanPayment(loanApplicationDTO.Payments, loanApplicationDTO.LoanId))
            {
                return "Invalid provided payment.";
            }


            if (loanApplicationDTO.Amount <= 0)
            {
                return "Amount must be greater than zero.";
            }

            if (loanApplicationDTO.Amount > loan.MaxAmount)
            {
                return "The amount must exceed the maximum loan amount.";
            }

            ClientLoan clientLoan = new ClientLoan()
            {
                ClientId = client.Id,
                Amount = loanApplicationDTO.Amount * 1.20,
                LoanId = loanApplicationDTO.LoanId,
                Payments = loanApplicationDTO.Payments,

            };
            _clientLoanRepository.Save(clientLoan);

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT,
                Amount = loanApplicationDTO.Amount,
                Description = $"{loan.Name} loan approved",
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });
            toAccount.Balance = toAccount.Balance + loanApplicationDTO.Amount;
            _accountRepository.Save(toAccount);

            return "OK";
        }

        public List<LoanDTO> GetLoans()
        {
            var loans = _loanRepository.GetAll();
            var loansDTO = new List<LoanDTO>();

            foreach (Loan loan in loans)
            {
                loansDTO.Add(new LoanDTO(loan));
            }

            return loansDTO;
        }

        private bool ValidateLoanPayment(string payment, long id)
        {
            var loans = _loanRepository.GetAll();
            foreach (var loan in loans)
            {
                if (loan.Id == id)
                {
                    if (loan.Payments.Split(',').Contains(payment))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
