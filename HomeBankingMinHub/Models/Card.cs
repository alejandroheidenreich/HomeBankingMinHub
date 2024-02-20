using HomeBankingMinHub.Utils;
using System.Drawing;

namespace HomeBankingMinHub.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public CardType Type { get; set; }
        public CardColor Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public Card(long clientId, string cardHolder, CardType type, CardColor color, List<Card> cards)
        {
            ClientId = clientId;
            CardHolder = cardHolder;
            Type = type;
            Color = color;
            Number = CardUtils.GenerateCardNumber(cards);
            Cvv = CardUtils.GenerateCvvNumber();
            FromDate = DateTime.Now;
            ThruDate = DateTime.Now.AddYears(4);
        }
    }
}
