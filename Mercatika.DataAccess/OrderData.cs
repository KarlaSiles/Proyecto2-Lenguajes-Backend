using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class OrderData
    {
        private readonly string connectionString;

        public OrderData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            List<Order> orders = new List<Order>();

            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = "SELECT order_id, client_id, employee_id, order_date, address_trip, province_trip, country_trip, phone_trip, date_trip FROM [Order]";
            SqlCommand command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            List<int> orderIds = new List<int>();

            while (await reader.ReadAsync())
            {
                var order = new Order
                (
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7),
                    reader.GetDateTime(8)
                );

                orders.Add(order);
                orderIds.Add(order.OrderId);
            }

            await reader.CloseAsync();

            var detailData = new OrderDetailData(connectionString);

            //recorre todas las órdenes y obtiene los detalles de cada una
            foreach (var order in orders)
            {
                var details = await detailData.GetOrderDetailsByOrderIdAsync(order.OrderId);
                order.Details = new List<OrderDetail>(details);
            }

            return orders;
        }


        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = @"SELECT order_id, client_id, employee_id, order_date, address_trip, province_trip, country_trip, phone_trip, date_trip 
                   FROM [Order] WHERE order_id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var order = new Order
                (
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7),
                    reader.GetDateTime(8)
                );

                await reader.CloseAsync();

                var detailData = new OrderDetailData(connectionString);
                var details = await detailData.GetOrderDetailsByOrderIdAsync(order.OrderId);
                order.Details = new List<OrderDetail>(details);

                return order;
            }

            return null;
        }


        public async Task AddOrderAsync(Order order)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                // Insertar la orden sin order_id (lo genera automáticamente)
                string insertOrderSql = @"
            INSERT INTO [Order] 
            (client_id, employee_id, order_date, address_trip, province_trip, country_trip, phone_trip, date_trip) 
            VALUES (@client_id, @employee_id, @order_date, @address_trip, @province_trip, @country_trip, @phone_trip, @date_trip);

            SELECT SCOPE_IDENTITY();"; //recuperar el ID generado automáticamente por identity

                SqlCommand orderCommand = new SqlCommand(insertOrderSql, connection, transaction);
                orderCommand.Parameters.AddWithValue("@client_id", order.ClientId);
                orderCommand.Parameters.AddWithValue("@employee_id", order.EmployeeId);
                orderCommand.Parameters.AddWithValue("@order_date", order.OrderDate);
                orderCommand.Parameters.AddWithValue("@address_trip", order.AddressTrip);
                orderCommand.Parameters.AddWithValue("@province_trip", order.ProvinceTrip);
                orderCommand.Parameters.AddWithValue("@country_trip", order.CountryTrip);
                orderCommand.Parameters.AddWithValue("@phone_trip", order.PhoneTrip);
                orderCommand.Parameters.AddWithValue("@date_trip", order.DateTrip);

                // Recuperar el ID generado automáticamente y asignarlo a la orden (objeto orden)
                object result = await orderCommand.ExecuteScalarAsync();
                order.OrderId = Convert.ToInt32(result);

                // Insertar los detalles usando el SP que calcula el line_price (amount * price)
                foreach (var detail in order.Details)
                {
                    SqlCommand detailCmd = new SqlCommand("InsertOrderDetail", connection, transaction);
                    detailCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    detailCmd.Parameters.AddWithValue("@order_id", order.OrderId);
                    detailCmd.Parameters.AddWithValue("@productDetail_id", detail.ProductDetailId);
                    detailCmd.Parameters.AddWithValue("@amount", detail.Amount);

                    await detailCmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task UpdateOrderAsync(Order order)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = @"UPDATE [Order] 
                           SET client_id = @client_id,
                               employee_id = @employee_id,
                               order_date = @order_date,
                               address_trip = @address_trip,
                               province_trip = @province_trip,
                               country_trip = @country_trip,
                               phone_trip = @phone_trip,
                               date_trip = @date_trip
                           WHERE order_id = @order_id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", order.OrderId);
            command.Parameters.AddWithValue("@client_id", order.ClientId);
            command.Parameters.AddWithValue("@employee_id", order.EmployeeId);
            command.Parameters.AddWithValue("@order_date", order.OrderDate);
            command.Parameters.AddWithValue("@address_trip", order.AddressTrip);
            command.Parameters.AddWithValue("@province_trip", order.ProvinceTrip);
            command.Parameters.AddWithValue("@country_trip", order.CountryTrip);
            command.Parameters.AddWithValue("@phone_trip", order.PhoneTrip);
            command.Parameters.AddWithValue("@date_trip", order.DateTrip);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = "DELETE FROM [Order] WHERE order_id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = "SELECT COUNT(1) FROM [Order] WHERE order_id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            int count = (int)await command.ExecuteScalarAsync();

            return count > 0;
        }
    }
}
