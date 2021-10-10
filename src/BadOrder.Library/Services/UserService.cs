using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Services;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly string _traceId;

        public UserService(IUserRepository userRepository, IAuthService authService, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _authService = authService;

            var httpContext = contextAccessor.HttpContext;
            _traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        }

        public async Task<UserResult> CreateAsync(NewUserRequest request)
        {
            var userExists = await _userRepository.GetByEmailAsync(request.Email);
            if (userExists is not null)
            {
                return new EmailInUse(ResultErrors.Conflict(_traceId, request.Email, "Email is already in use"));
            }

            if (!_authService.IsAuthRole(request.Role))
            {
                return new InvalidRole(ResultErrors.InvalidRole(_traceId, request.Role));
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
                ? new UserNotFound(ResultErrors.NotFound<User>(_traceId, id))
                : new UserFound(result);
        }
        public async Task<UserResult> GetByEmailAsync(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            return (result is null)
                ? new UserNotFound(ResultErrors.NotFound<User>(_traceId, email))
                : new UserFound(result);
        }

        public async Task<UserResult> UpdateAsync(string id, UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null)
            {
                return new UserNotFound(ResultErrors.NotFound<User>(_traceId, id));
            }

            if (!_authService.IsAuthRole(request.Role))
            {
                return new InvalidRole(ResultErrors.InvalidRole(_traceId, request.Role));
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
                return new UserNotFound(ResultErrors.NotFound<User>(_traceId, id));
            }

            await _userRepository.DeleteAsync(id);
            return new UserDeleted();
        }

        public async Task<UserResult> AuthenticateAsync(AuthenticateUserRequest request)
        {
            User user = await _userRepository.GetByEmailAsync(request.Email);

            if (!_authService.VerifyUserPassword(request.Password, user?.Password))
            {
                return new AuthenticateFailur(ResultErrors.Conflict(_traceId, null, "Email or password provided is invalid"));
            }

            var token = _authService.GenerateJwtToken(user);

            return new AuthenticateSuccess(new UserAuthenticated(token));
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
    }
}
