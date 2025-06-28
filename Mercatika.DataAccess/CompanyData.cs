using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class CompanyData
    {
        private readonly string _connectionString;

        public CompanyData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Update(Company company)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_UpdateCompany", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@idsetup", company.Idsetup);
                    command.Parameters.AddWithValue("@sale_tax", company.Sale_tax);
                    command.Parameters.AddWithValue("@name_company", company.Name_company);
                    command.Parameters.AddWithValue("@address_company", company.Address_company);
                    command.Parameters.AddWithValue("@city_company", company.City_company);
                    command.Parameters.AddWithValue("@state_or_province", company.State_or_province);
                    command.Parameters.AddWithValue("@zip_code_company", company.Zip_code_company);
                    command.Parameters.AddWithValue("@country_company", company.Country_company);
                    command.Parameters.AddWithValue("@phone_company", company.Phone_company);
                    command.Parameters.AddWithValue("@fax_number_company", company.Fax_number_company);
                    command.Parameters.AddWithValue("@payments_terms", company.Payments_terms);
                    command.Parameters.AddWithValue("@message", company.Message);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Company GetById(int id)
        {
            Company company = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetCompany", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idsetup", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            company = new Company(
                                Convert.ToInt32(reader["idsetup"]),
                                Convert.ToDouble(reader["sale_tax"]),
                                reader["name_company"].ToString(),
                                reader["address_company"].ToString(),
                                reader["city_company"].ToString(),
                                reader["state_or_province"].ToString(),
                                Convert.ToInt32(reader["zip_code_company"]),
                                reader["country_company"].ToString(),
                                Convert.ToInt32(reader["phone_company"]),
                                Convert.ToInt32(reader["fax_number_company"]),
                                reader["payments_terms"].ToString(),
                                reader["message"].ToString()
                            );
                        }
                    }
                }
            }

            return company;
        }
    }
}