using Microsoft.AspNetCore.Mvc;
using Mercatika.Business;
using Mercatika.Domain;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentBusiness paymentBusiness;

        public PaymentsController(PaymentBusiness paymentBusiness)
        {
            this.paymentBusiness = paymentBusiness;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var payments = paymentBusiness.GetAllPayments();
            return Ok(payments);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PaymentUpdateDto dto)
        {
            if (id != dto.PaymentId)
                return BadRequest("El ID de la URL no coincide con el ID del objeto.");

            bool success = paymentBusiness.UpdatePayment(id, dto.CreditCardNum, dto.PaymentMethodId);

            if (!success)
                return NotFound("No se pudo actualizar el pago.");

            return NoContent();
        }
    }
}
