using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Orders.Dtos;
using BadOrder.Library.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("api/orders/service")]
    [Authorize(Roles = "Admin, User")]
    public class OrdersServiceController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEnumerable<Claim> _userClaims;

        public OrdersServiceController(IOrderService orderService, IHttpContextAccessor accessor)
        {
            _orderService = orderService;
            _userClaims = accessor.HttpContext.User.Claims;
        }

        [HttpPost]
        public async Task<IActionResult> New()
        {
            var orderResult = await _orderService.New(_userClaims);
            return orderResult.AsActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orderResult = await _orderService.Get(_userClaims);
            return orderResult.AsActionResult();
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateOrderRequest request)
        {
            var orderResult = await _orderService.Update(request, _userClaims);
            return orderResult.AsActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var orderResult = await _orderService.Delete(_userClaims);
            return orderResult.AsActionResult();
        }

        [Route("send")]
        [HttpPost]
        public IActionResult Send()
        {
            return Ok();
        }
    }
}
