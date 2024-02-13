using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        //IEnumerable<Card> GetAllCardsByIdClient();
        void Save(Card card);
        Card FindById(long id);


    }
}
