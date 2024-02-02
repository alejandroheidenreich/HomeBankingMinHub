using HomeBankingMindHub.Models;

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
        }
    }
}
