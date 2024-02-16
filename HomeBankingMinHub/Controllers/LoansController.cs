using HomeBankingMindHub.Models;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private ILoanRepository _loanRepository;
        private IClientRepository _clientRepository;
        private IClientLoanRepository _clientLoanRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public LoansController(ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _loanRepository = loanRepository;
            _clientRepository = clientRepository;
            _clientLoanRepository = clientLoanRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();

                foreach (Loan loan in loans)
                {
                    loansDTO.Add(new LoanDTO(loan));
                }

                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {

                if (!ValidateClientUser(out Client client))
                {
                    return Forbid("No existe el cliente");
                }

                var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

                if (loan == null)
                {
                    return Forbid();
                }

                if (loanApplicationDTO.ToAccountNumber == string.Empty)
                {
                    return Forbid("Cuenta de destino no proporcionada.");
                }

                Account toAccount = _accountRepository.FinByNumber(loanApplicationDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return Forbid("Cuenta de destino no existe");
                }

                if (toAccount.ClientId != client.Id)
                {
                    return Forbid("La cuenta proporcionada no es del usuario actual");
                }


                if (loanApplicationDTO.Payments.IsNullOrEmpty())
                {
                    return Forbid("Cuotas no proporcionadas.");
                }

                if (!ValidateLoanPayment(loanApplicationDTO.Payments, loanApplicationDTO.LoanId))
                {
                    return Forbid("Cuotas proporcionadas invalidas.");
                }
                

                if (loanApplicationDTO.Amount <= 0 )
                {
                    return Forbid("Monto debe ser mayor a cero.");
                }

                if (loanApplicationDTO.Amount > loan.MaxAmount )
                {
                    return Forbid("Monto debe exceder el monto maximo del prestamo.");
                }


                ClientLoan clientLoan = new ClientLoan()
                {
                    ClientId = client.Id,
                    Amount = loanApplicationDTO.Amount * 1.20,
                    LoanId = loanApplicationDTO.LoanId,
                    Payments = loanApplicationDTO.Payments,

                };

                _clientLoanRepository.Save(clientLoan);

                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = loanApplicationDTO.Amount,
                    Description = $"{loan.Name} loan approved",
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });


                toAccount.Balance = toAccount.Balance + loanApplicationDTO.Amount;

                _accountRepository.Save(toAccount);

                return Created("Creado con exito", clientLoan);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool ValidateClientUser(out Client client)
        {
            client = null;
            if (User.FindFirst("Client") != null)
            {
                client = _clientRepository.FindByEmail(User.FindFirst("Client").Value);
                if (client != null)
                    return true;
            }
            return false;
        }

        private bool ValidateLoanPayment(string payment, long id)
        {
            var loans = _loanRepository.GetAll();
            foreach (var loan in loans)
            {
                if (loan.Id == id)
                {
                    if (loan.Payments.Split(',').Contains(payment))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
