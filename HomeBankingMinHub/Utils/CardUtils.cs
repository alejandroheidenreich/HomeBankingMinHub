using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Utils
{
    public class CardUtils
    {
        
        public static string GenerateCardNumber(IEnumerable<Card> cards)
        {
            Random rand = new Random();

            string number = rand.Next(1000, 9999).ToString() + '-' + (rand.Next(0, 9999).ToString()).PadLeft(4,'0') + '-' + (rand.Next(0, 9999).ToString()).PadLeft(4, '0') + '-' + (rand.Next(0, 9999).ToString()).PadLeft(4, '0');

            foreach (var c in cards)
            {
                if (number == c.Number)
                {
                    GenerateCardNumber(cards);
                }
            }

            return number;
        }

        public static int GenerateCvvNumber()
        {
            Random rand = new Random();
            return rand.Next(100, 999);
        }

        public static bool ValidateNewCard(NewCardDTO newCard)
        {
            return (newCard.Type == "CREDIT" || newCard.Type == "DEBIT") && (newCard.Color == "GOLD" || newCard.Color == "SILVER" || newCard.Color == "TITANIUM");
        }

        public static bool CanCreateCardType(IEnumerable<Card> cards, CardType type, long clientId)
        {
            var cardsFilter = cards.Where(c => c.ClientId == clientId && c.Type == type);
            return cardsFilter is not null && cardsFilter.Count() < 3;
        }

        public static bool CanCreateCardColor(IEnumerable<Card> cards, CardColor color, CardType type, long clientId)
        {
            var cardsFilter = cards.Where(c => c.ClientId == clientId && c.Color == color && c.Type == type);
            return cardsFilter is not null && cardsFilter.Count() == 0;
        }
    }
}
