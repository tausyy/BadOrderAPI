using BadOrder.Library.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.DataAccess
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<User> GetUserAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersAsync();
        Task UpdateUserAsync(User user);

    }
}
