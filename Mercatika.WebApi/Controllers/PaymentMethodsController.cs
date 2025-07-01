using Microsoft.AspNetCore.Mvc;
using Mercatika.Business;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly PaymentMethodBusiness paymentMethodBusiness;

        public PaymentMethodsController(PaymentMethodBusiness paymentMethodBusiness)
        {
            this.paymentMethodBusiness = paymentMethodBusiness;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var methods = paymentMethodBusiness.GetAllMethods();
            return Ok(methods);
        }
    }
}
