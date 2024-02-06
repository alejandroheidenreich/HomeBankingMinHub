using HomeBankingMindHub.Models;
using System.Linq;

namespace HomeBankingMinHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "pepito@gmail.com", FirstName="Jose", LastName="Gomez", Password="abc123"},
                    new Client { Email = "ana@gmail.com", FirstName="Ana", LastName="Perez", Password="123456"},
                    new Client { Email = "miguelito@gmail.com", FirstName="Miguel", LastName="Lopez", Password="hola123"},
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="asd111"},
                };

                context.Clients.AddRange(clients);

                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var allClients = context.Clients.ToList();

                foreach (var c in allClients)
                {
                    if (c != null)
                    {
                        var accounts = new Account[]
                        {
                            new Account { ClientId = c.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 1000 }
                        };
                        foreach (Account account in accounts)
                        {
                            context.Accounts.Add(account);
                        }
                        context.SaveChanges();
                    }
                }
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }


            }


            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");

                if (account1 != null)
                {

                    var transactions = new Transaction[]
                    {

                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }

                    context.SaveChanges();

                }
            }
        }
    }
}
