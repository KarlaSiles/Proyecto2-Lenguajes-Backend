using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Mercatika.Domain
{
    public class Payment
    {
        private int paymentId;
        private int orderId;
        private int paymentAmount;
        private DateTime datePayment;
        private int? creditCardNum;
        private string estado;
        private PaymentMethod? paymentMethod;

        public Payment()
        {
        }

        public Payment(int paymentId, int orderId, int paymentAmount, DateTime datePayment, int? creditCardNum, string estado, PaymentMethod? paymentMethod)
        {
            this.paymentId = paymentId;
            this.orderId = orderId;
            this.paymentAmount = paymentAmount;
            this.datePayment = datePayment;
            this.creditCardNum = creditCardNum;
            this.estado = estado;
            this.paymentMethod = paymentMethod;
        }

        public int PaymentId
        {
            get => paymentId;
            set => paymentId = value;
        }

        public int OrderId
        {
            get => orderId;
            set => orderId = value;
        }

        public int PaymentAmount
        {
            get => paymentAmount;
            set => paymentAmount = value;
        }

        public DateTime DatePayment
        {
            get => datePayment;
            set => datePayment = value;
        }

        public int? CreditCardNum
        {
            get => creditCardNum;
            set => creditCardNum = value;
        }

        public string Estado
        {
            get => estado;
            set => estado = value;
        }

        public PaymentMethod? PaymentMethod
        {
            get => paymentMethod;
            set => paymentMethod = value;
        }

        public int? PaymentMethodId
        {
            get => paymentMethod?.PaymentMethodId;
        }
    }
}
