using Mercatika.Business;
using Mercatika.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientBusiness _clientBusiness;

        public ClientController(ClientBusiness clientBusiness)
        {
            _clientBusiness = clientBusiness;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Client client)
        {
            try
            {
                var clientId = _clientBusiness.CreateClient(client);
                return Ok(clientId); // Devuelve solo el ID como número
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Client client)
        {
            try
            {
                if (client == null || id != client.ClientId)
                {
                    return BadRequest("Client ID mismatch");
                }

                bool success = _clientBusiness.UpdateClient(client);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool success = _clientBusiness.DeleteClient(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("company/{companyName}")]
        public IActionResult GetByCompanyName(string companyName)
        {
            try
            {
                var clients = _clientBusiness.GetClientsByCompanyName(companyName);
                return Ok(clients);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var client = _clientBusiness.GetClientById(id);
                if (client == null)
                {
                    return NotFound();
                }
                return Ok(client);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("contractname/{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var clients = _clientBusiness.GetClientsByName(name);
                return Ok(clients);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("contractlastname/{lastname}")]
        public IActionResult GetByLastname(string lastname)
        {
            try
            {
                var clients = _clientBusiness.GetClientsByLastname(lastname);
                return Ok(clients);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var clients = _clientBusiness.GetAllClients();
                return Ok(clients);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
