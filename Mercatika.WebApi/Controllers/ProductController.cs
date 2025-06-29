using Mercatika.Business;
using Mercatika.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductBusiness productBusiness;

    public ProductsController(ProductBusiness productBusiness)
    {
        this.productBusiness = productBusiness;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] string? searchTerm)
    {
        var products = string.IsNullOrWhiteSpace(searchTerm)
            ? await productBusiness.SearchProductsAsync("")
            : await productBusiness.SearchProductsAsync(searchTerm);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("ID inválido.");

        var product = await productBusiness.GetProductByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        if (product == null || string.IsNullOrWhiteSpace(product.ProductName))
            return BadRequest("Datos del producto inválidos.");

        if (product.Price <= 0)
            return BadRequest("El precio debe ser mayor que cero.");

        if (product.CategoryCode == null || product.CategoryCode.CategoryCode <= 0)
            return BadRequest("Categoría inválida.");

        
        var newId = await productBusiness.AddProductAsync(product.ProductName, product.Price, product.CategoryCode);
        product.ProductId = newId;

        return CreatedAtAction(nameof(GetById), new { id = newId }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] Product product)
    {
        if (product == null || id != product.ProductId)
            return BadRequest("Datos inválidos.");

        if (product.CategoryCode == null || product.CategoryCode.CategoryCode <= 0)
            return BadRequest("Categoría inválida.");

        bool updated = await productBusiness.UpdateProductAsync(product);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool deleted = await productBusiness.DeleteProductAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    // POST: api/products/{productId}/details
    [HttpPost("{productId:int}/details")]
    public async Task<ActionResult> AddProductDetail(int productId, [FromBody] ProductDetail detail)
    {
        if (detail == null)
            return BadRequest("Datos del detalle inválidos.");

        if (detail.StockAmount < 0)
            return BadRequest("El stock no puede ser negativo.");

        var inserted = await productBusiness.AddProductDetailAsync(productId, detail.UniqueProductCode, detail.StockAmount, detail.Size);

        return Ok(new { DetailId = inserted });
    }

    // PUT: api/products/{productId}/details
    [HttpPut("{productId:int}/details")]
    public async Task<ActionResult> UpdateProductDetail(int productId, [FromBody] ProductDetail detail)
    {
        if (detail == null)
            return BadRequest("Datos inválidos.");

        if (detail.StockAmount < 0)
            return BadRequest("El stock no puede ser negativo.");

        var updated = await productBusiness.UpdateProductDetailAsync(productId, detail.UniqueProductCode, detail.StockAmount, detail.Size);

        if (!updated)
            return StatusCode(500, "Error actualizando el detalle del producto.");

        return NoContent();
    }

}
