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
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<User> GetUserAsync(string id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task UpdateUserAsync(User user);

    }
}
