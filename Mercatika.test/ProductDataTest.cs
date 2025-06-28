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
        public void InsertarProduct()
        {
            // Paso 1: Insertar primero un producto
            var product = new Product
            {
                ProductName = "Platos",
                Price = 80,
                CategoryCode = 3
            };
            int productId = productData.InsertProduct(product);

            // Paso 2: Crear el detalle asociado
            var detail = new ProductDetail
            {
                Product = product,
                UniqueProductCode = "DETAIL-001",
                StockAmount = 12,
                Size = null
            };

            // Paso 3: Insertar el detalle
            int detailId = productData.InsertProductDetail(detail);

            // Validar
            Assert.That(detailId, Is.GreaterThan(0));
            Assert.That(detail.ProductDetailtId, Is.EqualTo(detailId));
        }
    }
}
