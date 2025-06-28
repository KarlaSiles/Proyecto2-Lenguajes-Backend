using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.Domain;
using Microsoft.Data.SqlClient;

namespace Mercatika.DataAccess
{
    public class ClientData
    {
        private readonly string _connectionString;

        public ClientData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Insert(Client client)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_InsertClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    command.Parameters.AddWithValue("@company_name", client.CompanyName);
                    command.Parameters.AddWithValue("@contract_name", client.ContractName);
                    command.Parameters.AddWithValue("@contract_lastname", client.ContractLastname);
                    command.Parameters.AddWithValue("@contract_position", client.ContractPosition);
                    command.Parameters.AddWithValue("@address", client.Address);
                    command.Parameters.AddWithValue("@city", client.City);
                    command.Parameters.AddWithValue("@province", client.Province);
                    command.Parameters.AddWithValue("@zip_code", client.ZipCode);
                    command.Parameters.AddWithValue("@country", client.Country);
                    command.Parameters.AddWithValue("@phone", client.Phone);
                    command.Parameters.AddWithValue("@fax_number", client.FaxNumber);

                    // Parámetro de salida
                    var clientIdParam = new SqlParameter("@client_id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(clientIdParam);

                    command.ExecuteNonQuery();

                    return Convert.ToInt32(clientIdParam.Value);
                }
            }
        }

        public bool Update(Client client)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"UPDATE Client SET 
                company_name = @CompanyName,
                contract_name = @ContractName,
                contract_lastname = @ContractLastname,
                contract_position = @ContractPosition,
                address = @Address,
                city = @City,
                province = @Province,
                zip_code = @ZipCode,
                country = @Country,
                phone = @Phone,
                fax_number = @FaxNumber
              WHERE client_id = @ClientId", connection))
                {
                    command.Parameters.AddWithValue("@ClientId", client.ClientId);
                    command.Parameters.AddWithValue("@CompanyName", client.CompanyName);
                    command.Parameters.AddWithValue("@ContractName", client.ContractName);
                    command.Parameters.AddWithValue("@ContractLastname", client.ContractLastname);
                    command.Parameters.AddWithValue("@ContractPosition", client.ContractPosition);
                    command.Parameters.AddWithValue("@Address", client.Address);
                    command.Parameters.AddWithValue("@City", client.City);
                    command.Parameters.AddWithValue("@Province", client.Province);
                    command.Parameters.AddWithValue("@ZipCode", client.ZipCode);
                    command.Parameters.AddWithValue("@Country", client.Country);
                    command.Parameters.AddWithValue("@Phone", client.Phone);
                    command.Parameters.AddWithValue("@FaxNumber", client.FaxNumber);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool Delete(int clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "DELETE FROM Client WHERE client_id = @ClientId", connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public List<Client> GetByCompanyName(string companyName)
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Client WHERE company_name LIKE @CompanyName", connection);
                command.Parameters.AddWithValue("@CompanyName", $"%{companyName}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(MapClientFromReader(reader));
                    }
                }
            }

            return clients;
        }

        public Client GetById(int id)
        {
            Client client = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Client WHERE client_id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        client = MapClientFromReader(reader);
                    }
                }
            }

            return client;
        }

        public List<Client> GetByName(string name)
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Client WHERE contract_name LIKE @Name", connection);
                command.Parameters.AddWithValue("@Name", $"%{name}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(MapClientFromReader(reader));
                    }
                }
            }

            return clients;
        }

        public List<Client> GetByLastname(string lastname)
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Client WHERE contract_lastname LIKE @Lastname", connection);
                command.Parameters.AddWithValue("@Lastname", $"%{lastname}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(MapClientFromReader(reader));
                    }
                }
            }

            return clients;
        }

        public List<Client> GetAll()
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Client", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(MapClientFromReader(reader));
                    }
                }
            }

            return clients;
        }

        private Client MapClientFromReader(SqlDataReader reader)
        {
            return new Client
            {
                ClientId = Convert.ToInt32(reader["client_id"]),
                CompanyName = reader["company_name"].ToString(),
                ContractName = reader["contract_name"].ToString(),
                ContractLastname = reader["contract_lastname"].ToString(),
                ContractPosition = reader["contract_position"].ToString(),
                Address = reader["address"].ToString(),
                City = reader["city"].ToString(),
                Province = reader["province"].ToString(),
                ZipCode = Convert.ToInt32(reader["zip_code"]),
                Country = reader["country"].ToString(),
                Phone = Convert.ToInt32(reader["phone"]),
                FaxNumber = Convert.ToInt32(reader["fax_number"])
            };
        }
    }
}
