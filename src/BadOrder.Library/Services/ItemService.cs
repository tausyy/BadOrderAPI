using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Items.Dtos;
using BadOrder.Library.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Services
{
    public class ItemService : IItemService
    {
        private readonly ICrudRepository<Item> _itemRepository;
        private readonly string _traceId;

        public ItemService(ICrudRepository<Item> itemRepository, IHttpContextAccessor contextAccessor)
        {
            _itemRepository = itemRepository;
            
            var httpContext = contextAccessor.HttpContext;
            _traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        }

        public async Task<ItemResult> CreateAsync(NewItemRequest request)
        {
            var item = ToItem(request);
            var createdItem = await _itemRepository.CreateAsync(item);
            return new ItemCreated(createdItem);
        }

        public async Task<ItemResult> DeleteAsync(string id)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem is null)
            {
                return new ItemNotFound(ResultErrors.NotFound<Item>(_traceId, id));
            }

            await _itemRepository.DeleteAsync(id);
            return new ItemDeleted();
        }

        public async Task<ItemResult> GetAllAsync()
        {
            var result = await _itemRepository.GetAllAsync();
            return new AllItems(result);
        }

        public async Task<ItemResult> GetByIdAsync(string id)
        {
            var result = await _itemRepository.GetByIdAsync(id);
            return result is null
                ? new ItemNotFound(ResultErrors.NotFound<Item>(_traceId, id))
                : new ItemFound(result);
        }

        public async Task<ItemResult> UpdateAsync(string id, UpdateItemRequest request)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem is null) 
            {
                
                return new ItemNotFound(ResultErrors.NotFound<Item>(_traceId, id));
            }

            var updatedItem = ToUpdatedItem(existingItem, request);
            await _itemRepository.UpdateAsync(updatedItem);

            return new ItemUpdated();
        }

        private static Item ToItem(NewItemRequest item) => new()
        {
            ProductName = item.ProductName,
            ProductNumber = item.ProductNumber,
            UnitType = item.UnitType
        };

        private static Item ToUpdatedItem(Item item, UpdateItemRequest request)
        {
            Item updatedItem = item with
            {
                ProductName = request.ProductName,
                ProductNumber = request.ProductNumber,
                UnitType = request.UnitType
            };
            return updatedItem;
        }

    }
}
