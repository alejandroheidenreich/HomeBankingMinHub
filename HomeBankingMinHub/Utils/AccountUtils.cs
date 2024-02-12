using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using System.Collections.Generic;

namespace HomeBankingMinHub.Utils
{
    public class AccountUtils
    {
        public static string GenerateVinNumber(IEnumerable<Account> accounts)
        {
            Random rand = new Random();

            string randomNumbers = rand.Next(1, 99999999).ToString();
            randomNumbers = randomNumbers.ToString().PadLeft(8, '0');

            string vin = "VIN-" + randomNumbers;

            foreach (var acc in accounts)
            {
                if (vin == acc.Number)
                {
                    GenerateVinNumber(accounts);
                }
            }

            return vin;
        }
    }
}
