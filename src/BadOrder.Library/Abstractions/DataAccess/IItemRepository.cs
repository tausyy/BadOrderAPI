using BadOrder.Library.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IItemRepository
    {
        Task<Item> CreateItemAsync(Item item);
        Task DeleteItemAsync(string id);
        Task<Item> GetItemAsync(string id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task UpdateItemAsync(Item item);
    }
}
