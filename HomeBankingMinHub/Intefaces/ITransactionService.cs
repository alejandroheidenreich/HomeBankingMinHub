using HomeBankingMinHub.DTOs;

namespace HomeBankingMinHub.Intefaces
{
    public interface ITransactionService
    {
        string CreateTransaction(TransferDTO transferDTO);
    }
}
