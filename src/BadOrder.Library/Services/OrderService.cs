using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Orders.Dtos;
using BadOrder.Library.Models.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly string _traceId;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;

            var httpContext = contextAccessor.HttpContext;
            _traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        }

        public async Task<OrderResult> New(IEnumerable<Claim> userClaims)
        {
            var emailFound = TryGetEmail(userClaims, out var userEmail);
            if (!emailFound) return new OrderUnauthorized(ResultErrors.Unauthorized(_traceId));

            await DeleteExistingOrder(userEmail);

            Order order = new()
            {
                OwnerEmail = userEmail,
                DateCreated = DateTimeOffset.UtcNow,
                OrderList = new List<OrderItem>()
            };

            //TODO figure out how to use mongo results
            var newOrder = await _orderRepository.CreateAsync(order);
            if (string.IsNullOrWhiteSpace(newOrder.Id))
                throw new Exception("Error in OrderService.New(). order creation failed.");
                
            return new OrderCreated(newOrder);
        }

        public async Task<OrderResult> Get(IEnumerable<Claim> userClaims)
        {
            var emailFound = TryGetEmail(userClaims, out var userEmail);
            if (!emailFound) return new OrderUnauthorized(ResultErrors.Unauthorized(_traceId));

            var order = await _orderRepository.GetByOwnerEmailAsync(userEmail);

            return order is null
                ? new OrderNotFound(ResultErrors.NotFound<Order>(_traceId, $"user/{userEmail}"))
                : new OrderFound(order);
        }

        public async Task<OrderResult> Update(UpdateOrderRequest request, IEnumerable<Claim> userClaims)
        {
            var emailFound = TryGetEmail(userClaims, out var userEmail);
            if (!emailFound) return new OrderUnauthorized(ResultErrors.Unauthorized(_traceId));

            var order = await _orderRepository.GetByOwnerEmailAsync(userEmail);
            if (order is null) return new OrderNotFound(ResultErrors.NotFound<Order>(_traceId, $"user/{userEmail}"));

            Order updatedOrder = order with
            {
                OrderList = request.OrderList
            };

            await _orderRepository.UpdateAsync(updatedOrder);

            return new OrderUpdated();
        }

        public async Task<OrderResult> Delete(IEnumerable<Claim> userClaims)
        {
            var emailFound = TryGetEmail(userClaims, out var userEmail);
            if (!emailFound) return new OrderUnauthorized(ResultErrors.Unauthorized(_traceId));

            var order = await _orderRepository.GetByOwnerEmailAsync(userEmail);
            if (order is null) return new OrderNotFound(ResultErrors.NotFound<Order>(_traceId, $"user/{userEmail}"));

            await _orderRepository.DeleteAsync(order.Id);

            return new OrderDeleted();
        }

        public Task<OrderResult> Send(IEnumerable<Claim> userClaims)
        {
            throw new NotImplementedException();
        }

        private async Task DeleteExistingOrder(string email)
        {
            var order = await _orderRepository.GetByOwnerEmailAsync(email);
            if (order is not null)
            {
                await _orderRepository.DeleteAsync(order.Id);
            }
        }

        private static bool TryGetEmail(IEnumerable<Claim> claims, out string email)
        {
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            email = nameClaim?.Value;
            
            if (string.IsNullOrWhiteSpace(email)) 
            {
                email = null;
            }

            return email is not null;
        }
    }
}
