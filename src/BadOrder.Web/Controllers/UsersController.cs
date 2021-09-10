using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Dtos;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repo;

        public UsersController(IUsersRepository repo)
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
        public async Task<ActionResult<User>> CreateUserAsync(WriteUser newUser)
        {
            User user = new()
            {
                Name = newUser.Name,
                Password = newUser.Password,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Role = newUser.Role,
                DateAdded = DateTimeOffset.UtcNow
            };
            
            await _repo.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user.AsNewUser());
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