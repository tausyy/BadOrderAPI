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
                return new InvalidRole(InvalidRoleError(request.Role));
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
            var result = await _userRepository.GetByIdAsync(id);
            return (result is null)
                ? new NotFound(NotFoundError(nameof(id), id))
                : new UserFound(result);
        }
        public async Task<UserResult> GetByEmailAsync(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            return (result is null)
                ? new NotFound(NotFoundError(nameof(email), email))
                : new UserFound(result);
        }

        public async Task<UserResult> UpdateAsync(string id, UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null)
            {
                return new NotFound(NotFoundError(nameof(id), id));
            }

            if (!_authService.IsAuthRole(request.Role))
            {
                return new InvalidRole(InvalidRoleError(request.Role));
            }

            var hashedPassword = _authService.HashPassword(request.Password);
            var updatedUser = ToUpdatedUser(existingUser, request, hashedPassword);

            await _userRepository.UpdateAsync(updatedUser);

            return new UserUpdated();
        }

        public async Task<UserResult> DeleteAsync(string id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null)
            {
                return new NotFound(NotFoundError(nameof(id), id));
            }

            await _userRepository.DeleteAsync(id);
            return new UserDeleted();
        }

        public async Task<UserResult> AuthenticateAsync(AuthenticateUserRequest request)
        {
            User user = await _userRepository.GetByEmailAsync(request.Email);

            if (!_authService.VerifyUserPassword(request.Password, user?.Password))
            {
                return new AuthenticateFailur(AuthenticateFailedError());
            }

            var token = _authService.GenerateJwtToken(user);

            return new AuthenticateSuccess(new { token });
        }

        private static User ToUpdatedUser(User user, UpdateUserRequest request, string hashedPassword)
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

        public static ErrorEntry AuthenticateFailedError() => new()
            { Message = "Email or password provided is invalid" };

    }
}
