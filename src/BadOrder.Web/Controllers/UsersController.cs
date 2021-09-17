using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _repo.GetUsersAsync();
            return users;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync(string id)
        {
            var user = await _repo.GetUserAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<User>> CreateUserAsync(WriteUser newUser)
        {
            var userExists = await _repo.GetUserByEmailAsync(newUser.Email);
            if (userExists is not null)
            {
                return Conflict("Email already in use");
            }

            if (newUser.Role != "Admin" && newUser.Role != "User")
            {
                return BadRequest("Invalid role type");
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
        public async Task<ActionResult> UpdateUserAsync(string id, WriteUser user)
        {
            var existingUser = await _repo.GetUserAsync(id);
            if (existingUser is null)
            {
                return NotFound();
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
        public async Task<ActionResult> DeleteUserAsync(string id)
        {
            var existingUser = await _repo.GetUserAsync(id);
            if (existingUser is null)
            {
                return NotFound();
            }

            await _repo.DeleteUserAsync(id);
            return NoContent();
        }

    }
}