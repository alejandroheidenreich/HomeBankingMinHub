using HomeBankingMinHub.DTOs;

namespace HomeBankingMinHub.Intefaces
{
    public interface IAccountService
    {
        List<AccountDTO> GetAccounts();
        AccountDTO GetAccount(long id);

    }
}
