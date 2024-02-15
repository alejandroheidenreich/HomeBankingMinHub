using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        void Save(Card card);
        Card FindById(long id);


    }
}
