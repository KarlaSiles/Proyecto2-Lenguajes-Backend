using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class ProductData
    {
        private readonly string connectionString;

        public ProductData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> InsertProductAsync(Product product)
        {
            if (product.CategoryCode == null)
                throw new ArgumentException("La categoría del producto no puede ser nula.");

            int newProductId = 0;

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("InsertProduct", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@product_name", product.ProductName);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@category_code", product.CategoryCode.CategoryCode);

            SqlParameter outputIdParam = new SqlParameter("@product_id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            newProductId = (int)outputIdParam.Value;
            product.ProductId = newProductId;

            return newProductId;
        }

        public async Task<int> InsertProductDetailAsync(ProductDetail detail)
        {
            int newDetailId = 0;

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("InsertProductDetail", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@product_id", detail.Product.ProductId);
            cmd.Parameters.AddWithValue("@uniqueProduct_code", (object?)detail.UniqueProductCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stock_amount", detail.StockAmount);
            cmd.Parameters.AddWithValue("@size", (object?)detail.Size ?? DBNull.Value);

            SqlParameter outputIdParam = new SqlParameter("@productDetail_id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            newDetailId = (int)outputIdParam.Value;
            detail.ProductDetailtId = newDetailId;

            return newDetailId;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product.CategoryCode == null)
                throw new ArgumentException("La categoría del producto no puede ser nula.");

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(
                "UPDATE Product SET product_name = @name, price = @price, category_code = @category WHERE product_id = @id", connection);

            cmd.Parameters.AddWithValue("@name", product.ProductName);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@category", product.CategoryCode.CategoryCode);
            cmd.Parameters.AddWithValue("@id", product.ProductId);

            await connection.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE product_id = @id", connection);

            cmd.Parameters.AddWithValue("@id", productId);

            await connection.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            List<Product> products = new List<Product>();

            bool isNumeric = int.TryParse(searchTerm, out int searchId);

            string query = @"
                SELECT p.product_id, p.product_name, p.price, 
                       c.category_code, c.descripcion AS category_descripcion
                FROM Product p
                INNER JOIN Category c ON p.category_code = c.category_code
                WHERE (@isNumeric = 1 AND p.product_id = @searchId)
                   OR (p.product_name LIKE '%' + @searchTerm + '%')
                   OR (c.descripcion LIKE '%' + @searchTerm + '%')";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@isNumeric", isNumeric ? 1 : 0);
            cmd.Parameters.AddWithValue("@searchId", searchId);
            cmd.Parameters.AddWithValue("@searchTerm", searchTerm);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    ProductId = (int)reader["product_id"],
                    ProductName = reader["product_name"].ToString(),
                    Price = Convert.ToDecimal(reader["price"]),
                    CategoryCode = new Category
                    {
                        CategoryCode = (int)reader["category_code"],
                        Description = reader["category_descripcion"].ToString()
                    }
                };

                products.Add(product);
            }

            return products;
        }

        // También podrías agregar método GetProductByIdAsync si lo necesitas
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            Product? product = null;

            string query = @"
                SELECT p.product_id, p.product_name, p.price, 
                       c.category_code, c.descripcion AS category_descripcion
                FROM Product p
                INNER JOIN Category c ON p.category_code = c.category_code
                WHERE p.product_id = @id";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                product = new Product
                {
                    ProductId = (int)reader["product_id"],
                    ProductName = reader["product_name"].ToString(),
                    Price = Convert.ToDecimal(reader["price"]),
                    CategoryCode = new Category
                    {
                        CategoryCode = (int)reader["category_code"],
                        Description = reader["category_descripcion"].ToString()
                    }
                };
            }

            return product;
        }
    }
}
