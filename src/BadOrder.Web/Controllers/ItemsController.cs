using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Items.Dtos;
using BadOrder.Library.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201, Type = typeof(Item))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(409, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateItemAsync(NewItemRequest request)
        {
            var item = await _itemService.CreateAsync(request);
            return item.AsActionResult(nameof(GetByIdAsync), nameof(ItemsController));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Item>))]
        public async Task<IActionResult> GetItemsAsync()
        {
            var items = await _itemService.GetAllAsync();
            return items.AsActionResult();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(Item))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var item = await _itemService.GetByIdAsync(id);
            return item.AsActionResult();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateItemAsync(string id, UpdateItemRequest request)
        {
            var item = await _itemService.UpdateAsync(id, request);
            return item.AsActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteItemAsync(string id)
        {
            var item = await _itemService.DeleteAsync(id);
            return item.AsActionResult();
        }
    }
}
