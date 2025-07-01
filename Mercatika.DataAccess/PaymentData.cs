using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class PaymentData
    {
        private readonly string connectionString;

        public PaymentData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        
        public List<Payment> GetAllPayments()
        {
            var payments = new List<Payment>();

            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT p.payment_id, p.order_id, p.payment_amount, p.date_payment, 
                                           p.creditCard_num, p.estado, pm.payment_method_id, pm.payment_method
                                      FROM Payments p
                                      LEFT JOIN PaymentMethod pm ON p.payment_method_id = pm.payment_method_id", connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var payment = new Payment
                {
                    PaymentId = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    PaymentAmount = reader.GetInt32(2),
                    DatePayment = reader.GetDateTime(3),
                    CreditCardNum = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Estado = reader.GetString(5),
                    PaymentMethod = reader.IsDBNull(6) ? null : new PaymentMethod
                    {
                        PaymentMethodId = reader.GetInt32(6),
                        PaymentMethodName = reader.GetString(7)
                    }
                };

                payments.Add(payment);
            }

            return payments;
        }

        
        public Payment? GetPaymentById(int paymentId)
        {
            Payment? payment = null;

            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT p.payment_id, p.order_id, p.payment_amount, p.date_payment, 
                                           p.creditCard_num, p.estado, pm.payment_method_id, pm.payment_method
                                      FROM Payments p
                                      LEFT JOIN PaymentMethod pm ON p.payment_method_id = pm.payment_method_id
                                      WHERE p.payment_id = @payment_id", connection);

            command.Parameters.AddWithValue("@payment_id", paymentId);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                payment = new Payment
                {
                    PaymentId = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    PaymentAmount = reader.GetInt32(2),
                    DatePayment = reader.GetDateTime(3),
                    CreditCardNum = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Estado = reader.GetString(5),
                    PaymentMethod = reader.IsDBNull(6) ? null : new PaymentMethod
                    {
                        PaymentMethodId = reader.GetInt32(6),
                        PaymentMethodName = reader.GetString(7)
                    }
                };
            }

            return payment;
        }

        
        public bool UpdatePayment(int paymentId, string estado, int? creditCardNum, int paymentMethodId)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"UPDATE Payments
                                      SET estado = @estado,
                                          creditCard_num = @creditCard_num,
                                          payment_method_id = @payment_method_id
                                      WHERE payment_id = @payment_id", connection);

            command.Parameters.AddWithValue("@estado", estado);
            command.Parameters.AddWithValue("@creditCard_num", (object?)creditCardNum ?? DBNull.Value);
            command.Parameters.AddWithValue("@payment_method_id", paymentMethodId);
            command.Parameters.AddWithValue("@payment_id", paymentId);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
