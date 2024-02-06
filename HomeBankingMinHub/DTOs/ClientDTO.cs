using HomeBankingMinHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.DTOs
{
    public class ClientDTO
    {
        [JsonIgnore]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<AccountDTO> Accounts { get; set; }

        public ICollection<ClientLoanDTO> Loans { get; set; }

        public ClientDTO() {}

        public ClientDTO(Client client)
        {
            Id = client.Id;
            Email = client.Email;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Accounts = client.Accounts.Select(ac => new AccountDTO
            {
                Id = ac.Id,
                Balance = ac.Balance,
                CreationDate = ac.CreationDate,
                Number = ac.Number
            }).ToList();
            Loans = client.ClientLoans.Select(cl => new ClientLoanDTO
            {
                Id = cl.Id,
                LoanId = cl.LoanId,
                Name = cl.Loan.Name,
                Amount = cl.Amount,
                Payments = int.Parse(cl.Payments)
            }).ToList();


        }
    }
}
