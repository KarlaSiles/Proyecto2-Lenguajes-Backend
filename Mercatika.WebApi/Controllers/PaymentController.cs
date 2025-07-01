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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var payment = paymentBusiness.GetPaymentById(id);
            if (payment == null)
                return NotFound("Pago no encontrado.");

            return Ok(payment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Payment payment)
        {
            if (id != payment.PaymentId)
                return BadRequest("El ID de la URL no coincide con el ID del objeto.");

            bool success = paymentBusiness.UpdatePayment(id, payment.Estado, payment.CreditCardNum, payment.PaymentMethodId);

            if (!success)
                return NotFound("No se pudo actualizar el pago.");

            return NoContent();
        }
    }
}
