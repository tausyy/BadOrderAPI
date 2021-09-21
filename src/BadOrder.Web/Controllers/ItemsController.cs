﻿using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Items.Dtos;
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
        private readonly IItemRepository _repo;
        public ItemsController(IItemRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Item>))]
        public async Task<IActionResult> GetItemsAsync()
        {
            var items = await _repo.GetItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Item))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetItemAsync(string id)
        {
            var item = await _repo.GetItemAsync(id);
            return (item is null) ? item.NotFound(id) : Ok(item);
        }

        [HttpPost]
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

            var createdItem = await _repo.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateItemAsync(string id, WriteItem item)
        {
            var existingItem = await _repo.GetItemAsync(id);
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

            await _repo.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteItemAsync(string id)
        {
            var existingUser = await _repo.GetItemAsync(id);
            if (existingUser is null)
            {
                return existingUser.NotFound(id);
            }

            await _repo.DeleteItemAsync(id);
            return NoContent();
        }
    }
}