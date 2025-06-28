using System.Collections.Generic;
using System.Threading.Tasks;
using Mercatika.Domain;
using Mercatika.DataAccess;

namespace Mercatika.Business
{
    public class OrderDetailBusiness
    {
        private readonly OrderDetailData orderDetailData;

        public OrderDetailBusiness(string connectionString)
        {
            orderDetailData = new OrderDetailData(connectionString);
        }

        public Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return orderDetailData.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public Task AddOrderDetailAsync(OrderDetail detail)
        {
            return orderDetailData.AddOrderDetailAsync(detail);
        }

        public Task DeleteOrderDetailAsync(int orderId, int productDetailId)
        {
            return orderDetailData.DeleteOrderDetailAsync(orderId, productDetailId);
        }
    }
}
