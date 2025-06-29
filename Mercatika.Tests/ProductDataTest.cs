using Mercatika.DataAccess;
using Mercatika.Domain;
using NUnit.Framework;


namespace Mercatika.test
{
    public class ProductDataTest
    {
        private ProductData productData;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=163.178.173.130;Database=Mercatika_Proyecto2_Lenguajes;User Id=Lenguajes;Password=lenguajesparaiso2025;TrustServerCertificate=True;";
            productData = new ProductData(connectionString);
        }

        [Test]
        public async Task InsertarProduct()
        {
            var product = new Product
            {
                ProductName = "Peluche verde",
                Price = 50,
                CategoryCode = new Category { CategoryCode = 4 }
            };

            int productId = await productData.InsertProductAsync(product);

            var detail = new ProductDetail
            {
                Product = product,
                UniqueProductCode = null,
                StockAmount = 15,
                Size = null
            };

            int detailId = await productData.InsertProductDetailAsync(detail);

            Assert.That(detailId, Is.GreaterThan(0));
            Assert.That(detail.ProductDetailId, Is.EqualTo(detailId));
        }

        [Test]
        public async Task SearchProducts()
        {
            var category = new Category
            {
                CategoryCode = 2,
                Description = "Ropa"
            };

            var product = new Product
            {
                ProductName = "Camisa roja",
                Price = 15,
                CategoryCode = category
            };

            int insertedId = await productData.InsertProductAsync(product);

            var resultById = await productData.SearchProductsAsync(insertedId.ToString());
            var resultByName = await productData.SearchProductsAsync("Camisa");
            var resultByCategory = await productData.SearchProductsAsync("Ropa");

            Assert.That(resultById.Any(p => p.ProductId == insertedId), "No se encontró el producto por ID.");
            Assert.That(resultByName.Any(p => p.ProductId == insertedId), "No se encontró el producto por nombre.");
            Assert.That(resultByCategory.Any(p => p.ProductId == insertedId), "No se encontró el producto por categoría.");
        }

        [Test]
        public async Task UpdateProduct()
        {
            var product = new Product
            {
                ProductName = "Producto Original",
                Price = 50.00M,
                CategoryCode = new Category { CategoryCode = 2 }
            };

            int id = await productData.InsertProductAsync(product);

            product.ProductName = "Producto Actualizado";
            product.Price = 99.99M;
            product.CategoryCode = new Category { CategoryCode = 3 };

            bool success = await productData.UpdateProductAsync(product);
            Assert.That(success, Is.True);

            var updated = (await productData.SearchProductsAsync(id.ToString())).FirstOrDefault();
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.ProductName, Is.EqualTo("Producto Actualizado"));
            Assert.That(updated.Price, Is.EqualTo(99.99M));
            Assert.That(updated.CategoryCode.CategoryCode, Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteProduct()
        {
            int productId = 27; // Usa un ID que exista en la base de datos para la prueba real

            bool success = await productData.DeleteProductAsync(productId);
            var deleted = (await productData.SearchProductsAsync(productId.ToString())).FirstOrDefault();

            Assert.That(success, Is.True);
            Assert.That(deleted, Is.Null);
        }



        [Test]
        public async Task UpdateProductDetail()
        {
            var product = new Product
            {
                ProductName = "Producto con detalle",
                Price = 80,
                CategoryCode = new Category { CategoryCode = 2 }
            };

            int productId = await productData.InsertProductAsync(product);
            product.ProductId = productId;

            var detail = new ProductDetail
            {
                Product = product,
                UniqueProductCode = null,
                StockAmount = 5,
                Size = null
            };

            int detailId = await productData.InsertProductDetailAsync(detail);
            detail.ProductDetailId = detailId;


            detail.StockAmount = 20;
            detail.Size = "XL";
            detail.UniqueProductCode = "XYZ789";

            bool success = await productData.UpdateProductDetailAsync(detail);

            Assert.That(success, Is.True, "No se pudo actualizar el detalle del producto.");
        }

    }
}