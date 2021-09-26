using BadOrder.Library.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task DeleteOrderAsync(string id);
        Task<Order> GetOrderAsync(string id);
        Task<IEnumerable<Order>> GetOrderAsync();
        Task UpdateOrderAsync(Order order);
    }
}
