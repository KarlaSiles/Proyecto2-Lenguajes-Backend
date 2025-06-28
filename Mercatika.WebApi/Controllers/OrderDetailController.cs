using Microsoft.AspNetCore.Mvc;
using Mercatika.Domain;
using Mercatika.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailBusiness orderDetailBusiness;

        public OrderDetailsController(OrderDetailBusiness orderDetailBusiness)
        {
            this.orderDetailBusiness = orderDetailBusiness;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetByOrderId(int orderId)
        {
            var details = await orderDetailBusiness.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(details);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderDetail detail)
        {
            if (detail == null || detail.OrderId <= 0 || detail.ProductDetailId <= 0 || detail.Amount <= 0)
                return BadRequest("Datos inválidos para crear el detalle.");

            await orderDetailBusiness.AddOrderDetailAsync(detail);
            return Ok("Detalle de orden agregado correctamente.");
        }

        [HttpDelete("{orderId}/{productDetailId}")]
        public async Task<ActionResult> Delete(int orderId, int productDetailId)
        {
            await orderDetailBusiness.DeleteOrderDetailAsync(orderId, productDetailId);
            return NoContent();
        }
    }
}
