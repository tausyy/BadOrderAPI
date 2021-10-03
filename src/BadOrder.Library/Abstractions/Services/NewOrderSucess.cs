using BadOrder.Library.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.Services
{
    public record NewOrderSucess : OrderResult
    {
        public Order Result { get; init; }
        public NewOrderSucess(Order newOrder) => Result = newOrder;
    }
}
