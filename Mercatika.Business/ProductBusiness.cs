
using Mercatika.Domain;
using Mercatika.DataAccess;
using Microsoft.Extensions.Configuration;


namespace Mercatika.Business
{
    public class ProductBusiness
    {
        private readonly ProductData productData;

        public ProductBusiness(IConfiguration configuration)
        {
            string conn = configuration.GetConnectionString("DefaultConnection");
            productData = new ProductData(conn);
        }

        public async Task<int> AddProductAsync(string name, decimal price, Category categoryCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");

            if (price <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            if (categoryCode == null || categoryCode.CategoryCode <= 0)
                throw new ArgumentException("La categoría del producto no es válida.");

            Product product = new Product
            {
                ProductName = name,
                Price = price,
                CategoryCode = categoryCode
            };

            return await productData.InsertProductAsync(product);
        }

        public async Task<int> AddProductDetailAsync(int productId, string? code, int stock, string? size)
        {
            if (productId <= 0)
                throw new ArgumentException("ID de producto no válido.");

            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            Product product = new Product { ProductId = productId };

            ProductDetail detail = new ProductDetail
            {
                Product = product,
                UniqueProductCode = code,
                StockAmount = stock,
                Size = size
            };

            return await productData.InsertProductDetailAsync(detail);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product.CategoryCode == null || product.CategoryCode.CategoryCode <= 0)
                throw new ArgumentException("La categoría del producto no es válida.");

            return await productData.UpdateProductAsync(product);
        }

        public async Task<bool> UpdateProductDetailAsync(int productId, string? code, int stock, string? size)
        {
            if (productId <= 0)
                throw new ArgumentException("ID de producto no válido.");

            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            var product = new Product { ProductId = productId };

            var detail = new ProductDetail
            {
                Product = product,
                UniqueProductCode = code,
                StockAmount = stock,
                Size = size
            };

            return await productData.UpdateProductDetailAsync(detail);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("ID de producto no válido.");

            return await productData.DeleteProductAsync(productId);
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío.");

            return await productData.SearchProductsAsync(searchTerm);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("ID de producto no válido.");

            return await productData.GetProductByIdAsync(productId);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            var product = await productData.GetProductByIdAsync(id);
            return product != null;
        }
    }
}

