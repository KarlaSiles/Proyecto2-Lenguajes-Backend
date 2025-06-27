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

            while (await reader.ReadAsync())
            {
                orders.Add(new Order
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
                ));
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
                return new Order
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
            }

            return null;
        }

        public async Task AddOrderAsync(Order order)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string sql = @"INSERT INTO [Order] (client_id, employee_id, order_date, address_trip, province_trip, country_trip, phone_trip, date_trip) 
                           VALUES (@client_id, @employee_id, @order_date, @address_trip, @province_trip, @country_trip, @phone_trip, @date_trip)";
            SqlCommand command = new SqlCommand(sql, connection);
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
