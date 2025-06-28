using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class OrderDetailData
    {
        private readonly string connectionString;

        public OrderDetailData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Obtener todos los detalles de una orden específica
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();

            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = @"SELECT order_id, productDetail_id, amount, line_price
                           FROM OrderDetail
                           WHERE order_id = @order_id";

            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", orderId);

            await connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderId = reader.GetInt32(0),
                    ProductDetailId = reader.GetInt32(1),
                    Amount = reader.GetInt32(2),
                    LinePrice = Convert.ToDecimal(reader[3])
                };

                details.Add(detail);
            }

            return details;
        }

        // Eliminar un detalle de orden por ID de orden y producto
        public async Task DeleteOrderDetailAsync(int orderId, int productDetailId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = "DELETE FROM OrderDetail WHERE order_id = @order_id AND productDetail_id = @productDetail_id";

            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", orderId);
            command.Parameters.AddWithValue("@productDetail_id", productDetailId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // Insertar un detalle usando el procedimiento almacenado que calcula line_price
        public async Task AddOrderDetailAsync(OrderDetail detail)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("InsertOrderDetail", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@order_id", detail.OrderId);
            command.Parameters.AddWithValue("@productDetail_id", detail.ProductDetailId);
            command.Parameters.AddWithValue("@amount", detail.Amount);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
