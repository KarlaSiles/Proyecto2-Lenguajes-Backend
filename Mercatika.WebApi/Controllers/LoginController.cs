using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Proyecto.Business;
using Proyecto.Domain;
using System.Threading.Tasks;

namespace Proyecto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginBusiness _business;

        public LoginController(IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");
            _business = new LoginBusiness(conn!);
        }

        /// <summary>
        /// POST /api/login
        /// Body: { "username": "...", "password": "..." }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Login credentials)
        {
            if (credentials is null
                || string.IsNullOrWhiteSpace(credentials.Username)
                || string.IsNullOrWhiteSpace(credentials.Password))
            {
                return BadRequest("Debe enviar usuario y contraseña.");
            }

            var ok = await _business.ValidateAsync(credentials.Username, credentials.Password);
            if (ok)
                return Ok(new { message = "Login exitoso" });
            else
                return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}
