using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Orders.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, User")]
    public class OrdersServiceController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersServiceController(IOrderService orderService)
        {
            _orderService = orderService;       
        }


        [HttpPost]
        public async Task<IActionResult> New(NewOrderRequest orderRequst)
        {
            var principal = HttpContext.User;
            var claims = principal.Claims;
            var newOrder = await _orderService.New(orderRequst, claims);

            return newOrder is null ? new BadRequestResult() : Ok(newOrder);
        }
    }
}
