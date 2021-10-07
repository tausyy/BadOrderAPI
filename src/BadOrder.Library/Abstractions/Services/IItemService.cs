using BadOrder.Library.Models.Items.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.Services
{
    public interface IItemService
    {
        Task<ItemResult> CreateAsync(NewItemRequest request);
        Task<ItemResult> GetAllAsync();
        Task<ItemResult> GetByIdAsync(string id);
        Task<ItemResult> UpdateAsync(string id, UpdateItemRequest request);
        Task<ItemResult> DeleteAsync(string id);
    }
}
