using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using BadOrder.Library.Models;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Services;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo, AuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _repo.GetAsync(id);
            return (user is null) ? user.NotFound(id) : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NewUser))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(409, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUserAsync(WriteUser newUser)
        {
            var userExists = await _repo.GetByEmailAsync(newUser.Email);
            if (userExists is not null)
            {
                return newUser.EmailInUse();
            }

            if (!_authService.IsAuthRole(newUser.Role))
            {
                return newUser.InvalidRole();
            }

            var hashedPassword = _authService.HashPassword(newUser.Password);
            var createdUser = await _repo.CreateAsync(newUser.AsSecureUser(hashedPassword));

            return CreatedAtAction(nameof(GetUserAsync), new { id = createdUser.Id }, createdUser.AsNewUser());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateUserAsync(string id, WriteUser user)
        {
            var existingUser = await _repo.GetAsync(id);
            if (existingUser is null)
            {
                return existingUser.NotFound(id);
            }

            if (!_authService.IsAuthRole(user.Role))
            {
                return user.InvalidRole();
            }

            var hashedPassword = _authService.HashPassword(user.Password);
            await _repo.UpdateAsync(existingUser.AsUpdatedUser(user, hashedPassword));

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var existingUser = await _repo.GetAsync(id);
            if (existingUser is null)
            {
                return existingUser.NotFound(id);
            }

            await _repo.DeleteAsync(id);
            return NoContent();
        }

    }
}