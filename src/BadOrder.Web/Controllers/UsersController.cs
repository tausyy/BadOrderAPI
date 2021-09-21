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

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _repo.GetUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _repo.GetUserAsync(id);
            return (user is null) ? user.NotFound(id) : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NewUser))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(409, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUserAsync(WriteUser newUser)
        {
            var userExists = await _repo.GetUserByEmailAsync(newUser.Email);
            if (userExists is not null)
            {
                return newUser.EmailInUse();
            }

            if (newUser.Role != "Admin" && newUser.Role != "User")
            {
                return newUser.InvalidRole();
            }

            User user = new()
            {
                Name = newUser.Name,
                Password = newUser.Password,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Role = newUser.Role,
                DateAdded = DateTimeOffset.UtcNow
            };
            
            var createdUser = await _repo.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetUserAsync), new { id = createdUser.Id }, createdUser.AsNewUser());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateUserAsync(string id, WriteUser user)
        {
            var existingUser = await _repo.GetUserAsync(id);
            if (existingUser is null)
            {
                return existingUser.NotFound(id);
            }

            User updatedUser = existingUser with
            {
                Name = user.Name,
                Password = user.Password,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            await _repo.UpdateUserAsync(updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var existingUser = await _repo.GetUserAsync(id);
            if (existingUser is null)
            {
                return existingUser.NotFound(id);
            }

            await _repo.DeleteUserAsync(id);
            return NoContent();
        }

    }
}