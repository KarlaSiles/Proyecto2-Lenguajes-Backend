using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using Proyecto.Domain;

namespace Proyecto.DataAccess
{
    public class LoginData
    {
        private readonly string _conn;
        public LoginData(string conn) => _conn = conn;

        /// <summary>
        /// Devuelve el registro de login para un username, o null si no existe.
        /// </summary>
        public async Task<Login?> GetByUsernameAsync(string username)
        {
            using var conn = new SqlConnection(_conn);
            await conn.OpenAsync();

            const string sql = @"
                SELECT login_id, username, password
                FROM [Login]
                WHERE username = @username";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync())
                return null;

            return new Login
            {
                LoginId = rdr.GetInt32(0),
                Username = rdr.GetString(1),
                Password = rdr.GetString(2)
            };
        }
    }
}
