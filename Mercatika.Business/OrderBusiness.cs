using System.Collections.Generic;
using System.Threading.Tasks;
using Mercatika.Domain;
using Mercatika.DataAccess;

namespace Mercatika.Business
{
    public class OrderBusiness //recordar incluir reglas de negocio etc
    {
        private readonly OrderData ordenData;

        public OrderBusiness(string connectionString)
        {
            ordenData = new OrderData(connectionString);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return ordenData.GetAllOrdersAsync();
        }

        public Task<Order?> GetOrderByIdAsync(int id)
        {
            return ordenData.GetOrderByIdAsync(id);
        }

        public Task AddOrderAsync(Order order)
        {
            return ordenData.AddOrderAsync(order);
        }

        public Task UpdateOrderAsync(Order order)
        {
            return ordenData.UpdateOrderAsync(order);
        }

        public Task DeleteOrderAsync(int id)
        {
            return ordenData.DeleteOrderAsync(id);
        }

        public Task<bool> OrderExistsAsync(int id)
        {
            return ordenData.OrderExistsAsync(id);
        }
    }
}
