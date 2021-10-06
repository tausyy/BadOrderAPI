using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.Services
{
    public interface IOrderService
    {
        Task<OrderResult> New(IEnumerable<Claim> userClaims);
        Task<OrderResult> Get(IEnumerable<Claim> userClaims);
        Task<OrderResult> Update(UpdateOrderRequest request, IEnumerable<Claim> userClaims);
        Task<OrderResult> Delete(IEnumerable<Claim> userClaims);
        Task<OrderResult> Send(IEnumerable<Claim> userClaims);
    }
}
