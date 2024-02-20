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
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_loanService.GetLoans());
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
                string email = User.FindFirst("Client").Value;
                if (!email.IsNullOrEmpty())
                {
                    string response = _loanService.CreateClientLoan(loanApplicationDTO, email);
                    if (string.Equals(response, "OK"))
                    {
                        return StatusCode(200, "Loan released");
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
