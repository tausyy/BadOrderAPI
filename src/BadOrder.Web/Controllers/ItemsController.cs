using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Items.Dtos;
using BadOrder.Library.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateItemAsync(NewItemRequest request)
        {
            var res = await _itemService.CreateAsync(request);
            return res.AsActionResult(nameof(GetByIdAsync));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item>))]
        public async Task<IActionResult> GetItemsAsync()
        {
            var items = await _itemService.GetAllAsync();
            return items.AsActionResult();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var item = await _itemService.GetByIdAsync(id);
            return item.AsActionResult();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateItemAsync(string id, UpdateItemRequest request)
        {
            var item = await _itemService.UpdateAsync(id, request);
            return item.AsActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteItemAsync(string id)
        {
            var item = await _itemService.DeleteAsync(id);
            return item.AsActionResult();
        }
    }
}
