using HomeBankingMindHub.Models;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public string CreateTransaction(TransferDTO transferDTO)
        {

            if (transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty())
            {
                return "Source account or destination account not provided.";
            }

            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
            {
                return "Transfer to the same account is not allowed.";
            }

            if (transferDTO.Amount == 0 || transferDTO.Description.IsNullOrEmpty())
            {
                return "Amount or description not provided.";
            }

            if (transferDTO.Amount < 0)
            {
                return "Invalid amount.";
            }

            Account fromAccount = _accountRepository.FinByNumber(transferDTO.FromAccountNumber);
            if (fromAccount == null)
            {
                return "The source account does not exist.";
            }

            if (fromAccount.Balance < transferDTO.Amount)
            {
                return "Insufficient funds.";
            }

            Account toAccount = _accountRepository.FinByNumber(transferDTO.ToAccountNumber);
            if (toAccount == null)
            {
                return "The destination account does not exist.";
            }

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT,
                Amount = transferDTO.Amount * -1,
                Description = transferDTO.Description + " " + toAccount.Number,
                AccountId = fromAccount.Id,
                Date = DateTime.Now,
            });

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT,
                Amount = transferDTO.Amount,
                Description = transferDTO.Description + " " + fromAccount.Number,
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });

            fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;

            _accountRepository.Save(fromAccount);

            toAccount.Balance = toAccount.Balance + transferDTO.Amount;

            _accountRepository.Save(toAccount);

            return "OK";
        }
    }
}
