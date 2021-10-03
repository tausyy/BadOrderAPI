using BadOrder.Library.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IOrderRepository : ICrudRepository<Order>
    {
        Task<Order> GetByOwnerEmailAsync(string email);
    }
}
