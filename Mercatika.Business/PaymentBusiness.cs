using System;
using System.Collections.Generic;
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

        public bool UpdatePayment(int paymentId, int? creditCardNum, int paymentMethodId)
        {
            if (paymentId <= 0)
                throw new ArgumentException("ID de pago inválido.");

            return paymentData.UpdatePayment(paymentId, creditCardNum, paymentMethodId);
        }
    }
}
