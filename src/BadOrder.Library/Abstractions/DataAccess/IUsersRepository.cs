using BadOrder.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IUsersRepository
    {
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task UpdateItemAsync(User user);

    }
}
