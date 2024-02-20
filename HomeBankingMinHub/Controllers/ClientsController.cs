using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace HomeBankingMinHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_clientService.GetAllClients());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                if (id > 0)
                {
                    var client = _clientService.GetClientById(id);
                    if (client == null)
                    {
                        return StatusCode(404, "Client not found");
                    }
                    else
                    {
                        return Ok(client);
                    }
                }
                else
                {
                    return StatusCode(400, "Invalid ID given");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewClientDTO client)
        {
            try
            {
                string response = _clientService.CreateNewClient(client);
                if (string.Equals(response, "OK"))
                {
                    return StatusCode(201, "New client created");
                }
                else
                {
                    return StatusCode(403, response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        public IActionResult CreateAccount()
        {
            try
            {
                string email = User.FindFirst("Client").Value;
                if (!email.IsNullOrEmpty())
                {
                    string response = _clientService.CreateCurrentAccount(email);
                    if (string.Equals(response, "OK"))
                    {
                        return StatusCode(201, "Account created");
                    }
                    else
                    {
                        return StatusCode(403, response);
                    }
                }
                return StatusCode(401, "The client request has not been completed because it lacks valid authentication credentials for the requested resource.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/accounts")]
        public IActionResult GetAccounts()
        {
            try
            {
                string email = User.FindFirst("Client").Value;
                if (!email.IsNullOrEmpty())
                {
                    return Ok(_clientService.GetCurrentAllAccounts(email));
                }
                return StatusCode(401, "The client request has not been completed because it lacks valid authentication credentials for the requested resource.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("current/cards")]
        public IActionResult CreateCard([FromBody] NewCardDTO newCard)
        {
            try
            {
                string email = User.FindFirst("Client").Value;

                if (!email.IsNullOrEmpty())
                {
                    string response = _clientService.CreateCurrentCard(newCard, email);
                    if (string.Equals(response, "OK"))
                    {
                        return StatusCode(201, "Card created");
                    }
                    else
                    {
                        return StatusCode(403, response);
                    }
                }
                return StatusCode(401, "The client request has not been completed because it lacks valid authentication credentials for the requested resource.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client").Value;

                if (!email.IsNullOrEmpty())
                {
                    return Ok(_clientService.GetCurrent(email));
                }
                return StatusCode(401, "Client doesnt exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
