using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.Services
{
    public interface IUserService
    {
        Task<UserResult> CreateAsync(NewUserRequest request);
        Task<UserResult> GetAllAsync();
        Task<UserResult> GetByIdAsync(string id);
        Task<UserResult> GetByEmailAsync(string email);
        Task<UserResult> UpdateAsync(string id, UpdateUserRequest request);
        Task<UserResult> DeleteAsync(string id);
        Task<UserResult> AuthenticateAsync(AuthenticateUserRequest request);
    }
}
