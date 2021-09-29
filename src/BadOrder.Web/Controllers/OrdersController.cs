using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models.Orders;
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
    public class OrdersController : ControllerBase
    {
        private readonly ICrudRepository<Order> _repo;

        public OrdersController(ICrudRepository<Order> orderRepository)
        {
            _repo = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAsync()
        {
            var orders = await _repo.GetAllAsync();
            return Ok(orders);
        }



    }
}
