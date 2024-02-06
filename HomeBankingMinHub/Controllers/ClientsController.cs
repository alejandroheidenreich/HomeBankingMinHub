using HomeBankingMinHub.DTOs;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
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

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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
        public IActionResult Post([FromBody]NewClientDTO newClient)
        {
            try
            {
                if (newClient == null || newClient.Email.IsNullOrEmpty() || newClient.FirstName.IsNullOrEmpty() || newClient.LastName.IsNullOrEmpty())
                    return BadRequest();

                Client clientCreated = new Client(newClient);

                _clientRepository.Save(clientCreated);

                return Created("Usuario Creado", clientCreated);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
