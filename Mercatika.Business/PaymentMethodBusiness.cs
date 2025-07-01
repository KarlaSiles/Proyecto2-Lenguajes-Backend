using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using Mercatika.DataAccess;
using Mercatika.Domain;

namespace Mercatika.Business
{
    public class PaymentMethodBusiness
    {
        private readonly PaymentMethodData paymentMethodData;

        public PaymentMethodBusiness(string connectionString)
        {
            paymentMethodData = new PaymentMethodData(connectionString);
        }

        public List<PaymentMethod> GetAllMethods()
        {
            return paymentMethodData.GetAllMethods();
        }
    }
}
