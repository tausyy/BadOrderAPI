using BadOrder.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface ICrudRepository<T> where T : ModelBase
    {
        Task<T> CreateAsync(T model);
        Task DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T model);
    }
}
