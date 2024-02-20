using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;

namespace HomeBankingMinHub.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountDTO GetAccount(long id)
        {
            var account = _accountRepository.FindById(id);

            if (account == null)
                return null;

            return new AccountDTO(account);
        }

        public List<AccountDTO> GetAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();

            foreach (Account account in accounts)
            {
                accountsDTO.Add(new AccountDTO(account));
            }

            return accountsDTO;
        }
    }
}
