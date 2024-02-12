﻿using HomeBankingMinHub.DTOs;
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
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    clientsDTO.Add(new ClientDTO(client));
                }
         
                return Ok(clientsDTO);
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
                var client = _clientRepository.FindById(id);

                if (client == null) return Forbid();

                var clientDTO = new ClientDTO(client);

                return Ok(clientDTO);
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
                if (string.IsNullOrEmpty(client.Email) || string.IsNullOrEmpty(client.Password) || string.IsNullOrEmpty(client.FirstName) || string.IsNullOrEmpty(client.LastName))
                    return StatusCode(400, "Invalid data");
                
                Client user = _clientRepository.FindByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email is in use");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = ClientUtils.HashPassword(client.Password),
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };

                _clientRepository.Save(newClient);
                return Created("", newClient);

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
                if (ValidateClientUser(out Client client) && client.Accounts.Count < 3)
                {
                    var accounts = _accountRepository.GetAllAccounts();

                    Account account = new Account { ClientId = client.Id, CreationDate = DateTime.Now, Number = AccountUtils.GenerateVinNumber(accounts), Balance = 0 };

                    _accountRepository.Save(account);

                    return StatusCode(201, $"Account created by {client.FirstName} {client.LastName}: {account.Number}");
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
                string email;
                if (User.FindFirst("Client") != null)
                {
                    email = User.FindFirst("Client").Value;
                }
                else
                {
                    return Forbid();
                }
                
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO(client);
                
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("manager")]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "datos inválidos");

                Client user = _clientRepository.FindByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };

                _clientRepository.Save(newClient);
                return Created("", newClient);
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
                if (client != null) return true;
            }
            return false;
        }
    }
}
