using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class PaymentMethodData
    {
        private readonly string connectionString;

        public PaymentMethodData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        
        public List<PaymentMethod> GetAllMethods()
        {
            var methods = new List<PaymentMethod>();

            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT payment_method_id, payment_method FROM PaymentMethod", connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                methods.Add(new PaymentMethod
                {
                    PaymentMethodId = reader.GetInt32(0),
                    PaymentMethodName = reader.GetString(1)
                });
            }

            return methods;
        }
    }
}
