using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Items.Dtos;
using BadOrder.Library.Models.Orders;
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
    public class ItemsController : ControllerBase
    {
        private readonly ICrudRepository<Item> _repo;
        public ItemsController(ICrudRepository<Item> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Item>))]
        public async Task<IActionResult> GetItemsAsync()
        {
            var items = await _repo.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(Item))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetItemAsync(string id)
        {
            var item = await _repo.GetByIdAsync(id);
            return (item is null) ? item.NotFound(id) : Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201, Type = typeof(Item))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(409, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateItemAsync(WriteItem newItem)
        {
            Item item = new()
            {
                ProductName = newItem.ProductName,
                ProductNumber = newItem.ProductNumber,
                UnitType = newItem.UnitType
            };

            var createdItem = await _repo.CreateAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateItemAsync(string id, WriteItem item)
        {
            var existingItem = await _repo.GetByIdAsync(id);
            if (existingItem is null)
            {
                return existingItem.NotFound(id);
            }

            Item updatedItem = existingItem with
            {
                ProductName = item.ProductName,
                ProductNumber = item.ProductNumber,
                UnitType = item.UnitType
            };

            await _repo.UpdateAsync(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteItemAsync(string id)
        {
            var existingItem = await _repo.GetByIdAsync(id);
            if (existingItem is null)
            {
                return existingItem.NotFound(id);
            }

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
