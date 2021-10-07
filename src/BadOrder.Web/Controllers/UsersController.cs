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
using BadOrder.Library.Abstractions.Services;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var requset = await _userService.GetAllAsync();
            return requset.AsActionResult();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var request = await _userService.GetByIdAsync(id);
            return request.AsActionResult();
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NewUser))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(409, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAsync(NewUserRequest newUser)
        {
            var result = await _userService.CreateAsync(newUser);
            return result.AsActionResult(nameof(GetByIdAsync), nameof(UsersController));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAsync(string id, UpdateUserRequest user)
        {
            var result = await _userService.UpdateAsync(id, user);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var request = await _userService.DeleteAsync(id);
            return request.AsActionResult();
        }

        [Route("auth")]
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateUserRequest user)
        {
            var request = await _userService.AuthenticateAsync(user);
            return request.AsActionResult();
        }

    }
}