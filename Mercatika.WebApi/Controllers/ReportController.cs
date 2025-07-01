using Mercatika.Business;
using Microsoft.AspNetCore.Mvc;
using Mercatika.Domain;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportBusiness _business;

        public ReportController(IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");
            _business = new ReportBusiness(conn);
        }

        /// <summary>
        /// 1) Listado general
        /// GET /api/reports
        /// </summary>
        [HttpGet]
        public Task<IEnumerable<Invoice>> GetAll()
            => _business.GetAllAsync();

        /// <summary>
        /// 2) Filtrado
        /// GET /api/reports/filter?clientId=1&orderId=10&dateFrom=2025-06-01&dateTo=2025-06-30&estado=pagado
        /// </summary>
        [HttpGet("filter")]
        public Task<IEnumerable<Invoice>> GetFiltered(
            [FromQuery] int? clientId,
            [FromQuery] int? orderId,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] string? estado)
            => _business.GetFilteredAsync(clientId, orderId, dateFrom, dateTo, estado);

        /// <summary>
        /// 3) Factura individual
        /// GET /api/reports/orders/{orderId}/invoice
        /// </summary>
        [HttpGet("orders/{orderId}/invoice")]
        public async Task<ActionResult<Invoice>> GetInvoice(int orderId)
        {
            try
            {
                var inv = await _business.GetByOrderIdAsync(orderId);
                return Ok(inv);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
