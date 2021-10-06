using BadOrder.Library.Abstractions.Services;
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

#nullable enable

namespace BadOrder.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AuthSuccess))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            User? user = await _repo.GetByEmailAsync(loginUser.Email);

            if (!_authService.VerifyUserPassword(loginUser.Password, user?.Password))
            {
                return user.AuthFailed();
            }

            return Ok(new AuthSuccess { Token = _authService.GenerateJwtToken(user) });
        }
    }
}
