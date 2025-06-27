using Microsoft.AspNetCore.Mvc;
using Mercatika.Domain;
using Mercatika.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderBusiness ordenBusiness;

        public OrdersController(OrderBusiness ordenBusiness)
        {
            this.ordenBusiness = ordenBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await ordenBusiness.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            var order = await ordenBusiness.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound("Orden no encontrada.");

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Order order)
        {
            if (order == null || string.IsNullOrWhiteSpace(order.AddressTrip))
                return BadRequest("Los datos de la orden son inválidos.");

            await ordenBusiness.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.OrderId)
                return BadRequest("El ID de la orden no coincide.");

            if (!await ordenBusiness.OrderExistsAsync(id))
                return NotFound("La orden no existe.");

            await ordenBusiness.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await ordenBusiness.OrderExistsAsync(id))
                return NotFound("La orden no existe.");

            await ordenBusiness.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
