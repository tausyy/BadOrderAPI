using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Services;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<UserResult> CreateAsync(NewUserRequest request)
        {
            var userExists = await _userRepository.GetByEmailAsync(request.Email);
            if (userExists is not null)
            {
                return new EmailInUse(EmailInUserError(request.Email));
            }

            if (!_authService.IsAuthRole(request.Role))
            {
                return new InvalidUserRole(InvalidRoleError(request.Role));
            }

            var hashedPassword = _authService.HashPassword(request.Password);
            var secureUser = ToSecureUser(request, hashedPassword);

            var createdUser = await _userRepository.CreateAsync(secureUser);
            return new UserCreated(createdUser);
        }

        public async Task<UserResult> GetAllAsync()
        {
            var result = await _userRepository.GetAllAsync();
            return new AllUsers(result);
        }

        public async Task<UserResult> GetByIdAsync(string id)
        {
            var result = await _userRepository.GetAsync(id);
            return (result is null)
                ? new UserNotFound(NotFoundError(nameof(id), id))
                : new UserFound(result);
        }
        public async Task<UserResult> GetByEmailAsync(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            return (result is null)
                ? new UserNotFound(NotFoundError(nameof(email), email))
                : new UserFound(result);
        }

        public async Task<UserResult> UpdateAsync(string id, UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetAsync(id);
            if (existingUser is null)
            {
                return new UserNotFound(NotFoundError(nameof(id), id));
            }

            if (!_authService.IsAuthRole(request.Role))
            {
                return new InvalidUserRole(InvalidRoleError(request.Role));
            }

            var hashedPassword = _authService.HashPassword(request.Password);
            var updatedUser = AsUpdatedUser(existingUser, request, hashedPassword);

            await _userRepository.UpdateAsync(updatedUser);

            return NoContent();
        }

        public Task<UserResult> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        private static User AsUpdatedUser(User user, UpdateUserRequest request, string hashedPassword)
        {
            User updatedUser = user with
            {
                Name = request.Name,
                Password = hashedPassword,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role
            };
            return updatedUser;
        }

        private static User ToSecureUser(NewUserRequest user, string hashedPassword) => new()
        {
            Name = user.Name,
            Password = hashedPassword,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            DateAdded = DateTimeOffset.UtcNow
        };

        private static ErrorEntry NotFoundError(string field, string value) => new()
            { Field = field, Value = value, Message = "User not found" };
        private static ErrorEntry EmailInUserError(string email) => new()
            { Field = nameof(email), Value = email, Message = "Email or password provided is invalid" };
        private static ErrorEntry InvalidRoleError(string role) => new()
            { Field = nameof(role), Value = role, Message = "Invalid role" };
    }
}
