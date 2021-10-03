using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IOrderService
    {
        Task<OrderResult> New(NewOrderRequest order, IEnumerable<Claim> userClaims);
    }
}
