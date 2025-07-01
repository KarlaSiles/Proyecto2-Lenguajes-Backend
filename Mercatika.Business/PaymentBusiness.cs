using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.DataAccess;
using Mercatika.Domain;

namespace Mercatika.Business
{
    public class PaymentBusiness
    {
        private readonly PaymentData paymentData;

        public PaymentBusiness(string connectionString)
        {
            paymentData = new PaymentData(connectionString);
        }

        public List<Payment> GetAllPayments()
        {
            return paymentData.GetAllPayments();
        }

        public Payment? GetPaymentById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            return paymentData.GetPaymentById(id);
        }

        public bool UpdatePayment(int paymentId, string estado, int? creditCardNum, int? paymentMethodId)
        {
            if (paymentId <= 0)
                throw new ArgumentException("ID de pago inválido.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado no puede ser vacío.");

          
            return paymentData.UpdatePayment(paymentId, estado, creditCardNum, paymentMethodId ?? 0);
        }
    }
}
