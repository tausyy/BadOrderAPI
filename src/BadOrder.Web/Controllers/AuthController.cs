using BadOrder.Library.Abstractions.Authentication;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using BadOrder.Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly AuthService _authService;

        public AuthController(IUserRepository repo, AuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AuthSuccess))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            var user = await _repo.GetByEmailAsync(loginUser.Email);
            if (user is null || 
                !_authService.VerifyUserPassword(loginUser.Password, user.Password))
            {
                var error = new ErrorEntry{ Message = "Email or password provided is invalid" };
                return BadRequest(new ErrorResponse { Errors = new[] { error } });
            }

            return Ok(new AuthSuccess { Token = _authService.GenerateJwtToken(user) });
        }
    }
}
