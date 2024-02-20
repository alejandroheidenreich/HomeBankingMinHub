using HomeBankingMindHub.Models;
using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client").Value;
                if (!email.IsNullOrEmpty())
                {
                    string response = _transactionService.CreateTransaction(transferDTO);
                    if (string.Equals(response, "OK"))
                    {
                        return StatusCode(200, "Transaction released");
                    }
                    else
                    {
                        return StatusCode(403, response);
                    }
                }
                else
                {
                    return StatusCode(401, "The client request has not been completed because it lacks valid authentication credentials for the requested resource.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

 
    }
}
