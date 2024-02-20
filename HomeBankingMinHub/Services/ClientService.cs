using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;

        }
        public string CreateCurrentAccount(string email)
        {
            Client client = _clientRepository.FindByEmail(email);

            if (client.Accounts.Count < 3)
            {
                Account account = new Account { ClientId = client.Id, CreationDate = DateTime.Now, Number = AccountUtils.GenerateVinNumber(_accountRepository.GetAllAccounts()), Balance = 0 };

                _accountRepository.Save(account);

                return "OK";
            }
            return $"{client.FirstName} {client.LastName} reached the maximum amount of accounts";
        }

        public string CreateCurrentCard(NewCardDTO newCard, string email)
        {
            Client client = _clientRepository.FindByEmail(email);
            var cards = _cardRepository.GetAllCards();

            if (!CardUtils.ValidateNewCard(newCard))
                return "Invalid data";

            CardType cType = (CardType)Enum.Parse(typeof(CardType), newCard.Type);
            if (!CardUtils.CanCreateCardType(cards, cType, client.Id))
                return $"You reached the maximum number of {cType} cards";

            CardColor cColor = (CardColor)Enum.Parse(typeof(CardColor), newCard.Color);
            if (!CardUtils.CanCreateCardColor(cards, cColor, cType, client.Id))
                return $"You reached the maximum number of {cType} cards";

            Card card = new Card(client.Id, $"{client.FirstName} {client.LastName}", cType, cColor, cards.ToList());
            _cardRepository.Save(card);
            return "OK";
        }

        public string CreateNewClient(NewClientDTO client)
        {
            if (string.IsNullOrEmpty(client.Email) || string.IsNullOrEmpty(client.Password) || string.IsNullOrEmpty(client.FirstName) || string.IsNullOrEmpty(client.LastName))
                return "Invalid creation data";

            Client user = _clientRepository.FindByEmail(client.Email);
            if (user != null)
                return "Email is in use";
            
            Client newClient = new Client
            {
                Email = client.Email,
                Password = ClientUtils.HashPassword(client.Password),
                FirstName = client.FirstName,
                LastName = client.LastName,
            };

            _clientRepository.Save(newClient);
            return "OK";
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();

            foreach (Client client in clients)
            {
                clientsDTO.Add(new ClientDTO(client));
            }

            return clientsDTO;
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);

            if (client == null)
                return null;

            return new ClientDTO(client);
        }

        public ClientDTO GetCurrent(string email)
        {
            Client client = _clientRepository.FindByEmail(email);
    
            return new ClientDTO(client);
        }

        public List<AccountDTO> GetCurrentAllAccounts(string email)
        {
            Client client = _clientRepository.FindByEmail(email);
            var accounts = _accountRepository.FindByClientId(client.Id);
            var accountsDTO = new List<AccountDTO>();

            foreach (Account account in accounts)
            {
                accountsDTO.Add(new AccountDTO(account));
            }

            return accountsDTO;
        }
    }
}
