using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;



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
            detail.ProductDetailId = newDetailId;

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

        public async Task<bool> UpdateProductDetailAsync(ProductDetail detail)
        {
            if (detail.Product == null || detail.Product.ProductId <= 0)
                throw new ArgumentException("El producto asociado no puede ser nulo o tener un ID inválido.");

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using SqlCommand cmd = new SqlCommand(
                @"UPDATE ProductDetail 
          SET uniqueProduct_code = @code, 
              stock_amount = @stock, 
              size = @size 
          WHERE product_id = @productId", connection);

            cmd.Parameters.AddWithValue("@code", (object?)detail.UniqueProductCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", detail.StockAmount);
            cmd.Parameters.AddWithValue("@size", (object?)detail.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@productId", detail.Product.ProductId);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("DeleteProductWithDetails", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", productId);

            await connection.OpenAsync();

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
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

            var productIds = new List<int>();

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
                    },
                    ProductDetails = new List<ProductDetail>()
                };

                products.Add(product);
                productIds.Add(product.ProductId);
            }

            await reader.CloseAsync();

            if (productIds.Count == 0)
                return products;


            string detailsQuery = @"
        SELECT pd.productDetail_id, pd.product_id, pd.uniqueProduct_code, pd.stock_amount, pd.size
        FROM ProductDetail pd
        WHERE pd.product_id IN (" + string.Join(",", productIds) + ")";

            using SqlCommand detailsCmd = new SqlCommand(detailsQuery, connection);
            using SqlDataReader detailsReader = await detailsCmd.ExecuteReaderAsync();

            while (await detailsReader.ReadAsync())
            {
                int prodId = (int)detailsReader["product_id"];
                var detail = new ProductDetail
                {
                    ProductDetailId = (int)detailsReader["productDetail_id"],
                    UniqueProductCode = detailsReader["uniqueProduct_code"] as string,
                    StockAmount = (int)detailsReader["stock_amount"],
                    Size = detailsReader["size"] as string
                };

                var product = products.FirstOrDefault(p => p.ProductId == prodId);
                product?.ProductDetails.Add(detail);
            }

            return products;
        }


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
                    },
                    ProductDetails = new List<ProductDetail>()
                };
            }

            await reader.CloseAsync();

            if (product == null)
                return null;

           
            string detailsQuery = @"
        SELECT productDetail_id, uniqueProduct_code, stock_amount, size
        FROM ProductDetail
        WHERE product_id = @productId";

            using SqlCommand detailsCmd = new SqlCommand(detailsQuery, connection);
            detailsCmd.Parameters.AddWithValue("@productId", id);

            using SqlDataReader detailsReader = await detailsCmd.ExecuteReaderAsync();

            while (await detailsReader.ReadAsync())
            {
                var detail = new ProductDetail
                {
                    ProductDetailId = (int)detailsReader["productDetail_id"],
                    UniqueProductCode = detailsReader["uniqueProduct_code"] as string,
                    StockAmount = (int)detailsReader["stock_amount"],
                    Size = detailsReader["size"] as string
                };

                product.ProductDetails.Add(detail);
            }

            return product;
        }
    }
}

