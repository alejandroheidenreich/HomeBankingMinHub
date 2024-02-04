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
                            new Account { ClientId = c.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
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
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
