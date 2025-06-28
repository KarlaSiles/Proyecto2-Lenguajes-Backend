using Mercatika.Business;
using Mercatika.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mercatika.WebApi.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class ProductsController : ControllerBase
        {
            private readonly ProductBusiness productBusiness;

            public ProductsController(ProductBusiness productBusiness)
            {
                this.productBusiness = productBusiness;
            }

            // GET: api/products?searchTerm=algo
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] string? searchTerm)
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("Debe proporcionar un término de búsqueda.");

                var products = await productBusiness.SearchProductsAsync(searchTerm);
                return Ok(products);
            }

            // GET: api/products/5
            [HttpGet("{id:int}")]
            public async Task<ActionResult<Product>> GetById(int id)
            {
                if (id <= 0)
                    return BadRequest("ID inválido.");

                var product = await productBusiness.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }

            // POST: api/products
            [HttpPost]
            public async Task<ActionResult<Product>> Create([FromBody] Product product)
            {
                if (product == null || string.IsNullOrWhiteSpace(product.ProductName))
                    return BadRequest("Datos del producto inválidos.");

                if (product.CategoryCode == null || product.CategoryCode.CategoryCode <= 0)
                    return BadRequest("Categoría inválida.");

                var newProductId = await productBusiness.AddProductAsync(product.ProductName, product.Price, product.CategoryCode);
                product.ProductId = newProductId;

                return CreatedAtAction(nameof(GetById), new { id = newProductId }, product);
            }

            // PUT: api/products/5
            [HttpPut("{id:int}")]
            public async Task<ActionResult> Update(int id, [FromBody] Product product)
            {
                if (product == null || id != product.ProductId)
                    return BadRequest("Datos inválidos.");

                if (product.CategoryCode == null || product.CategoryCode.CategoryCode <= 0)
                    return BadRequest("Categoría inválida.");

                bool exists = await productBusiness.ProductExistsAsync(id);
                if (!exists)
                    return NotFound();

                bool updated = await productBusiness.UpdateProductAsync(product);
                if (!updated)
                    return StatusCode(500, "Error actualizando el producto.");

                return NoContent();
            }

            // DELETE: api/products/5
            [HttpDelete("{id:int}")]
            public async Task<ActionResult> Delete(int id)
            {
                bool exists = await productBusiness.ProductExistsAsync(id);
                if (!exists)
                    return NotFound();

                bool deleted = await productBusiness.DeleteProductAsync(id);
                if (!deleted)
                    return StatusCode(500, "Error eliminando el producto.");

                return NoContent();
            }
        }
    }


