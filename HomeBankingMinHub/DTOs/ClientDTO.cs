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

        public ICollection<ClientLoanDTO> Credits { get; set; }

        public ICollection<CardDTO> Cards { get; set; }

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
            Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
            {
                Id = cl.Id,
                LoanId = cl.LoanId,
                Name = cl.Loan.Name,
                Amount = cl.Amount,
                Payments = int.Parse(cl.Payments)
            }).ToList();
            Cards = client.Cards.Select(c => new CardDTO
            {
                Id = c.Id,
                CardHolder = c.CardHolder,
                Color = c.Color,
                Cvv = c.Cvv,
                FromDate = c.FromDate,
                Number = c.Number,
                ThruDate = c.ThruDate,
                Type = c.Type
            }).ToList();

        }
    }
}
