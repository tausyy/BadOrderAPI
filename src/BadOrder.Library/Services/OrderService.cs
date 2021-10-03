using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<OrderResult> New(NewOrderRequest orderRequest, IEnumerable<Claim> userClaims)
        {
            var nameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var userEmail = nameClaim?.Value;
            
            if (userEmail is null)
            {
                var error = new ErrorEntry { Field = null, Value = null, Message = "Unauthorized" };
                return new NewOrderFailur(error);
            }

            await DeleteExistingOrder(userEmail);

            Order order = new()
            {
                OwnerEmail = userEmail,
                DateCreated = DateTimeOffset.UtcNow,
                OrderList = orderRequest.OrderList
            };

            //TODO figure out how to use mongo results
            var newOrder = await _orderRepository.CreateAsync(order);
            if (string.IsNullOrWhiteSpace(newOrder.Id))
                throw new Exception("Error in OrderService.New(). order creation failed.");
                
            return new NewOrderSucess(newOrder);
        }

        private async Task DeleteExistingOrder(string email)
        {
            var order = await _orderRepository.GetByOwnerEmailAsync(email);
            if (order is not null)
            {
                await _orderRepository.DeleteAsync(order.Id);
            }
        }
    }
}
