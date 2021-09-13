using BadOrder.Library.Abstractions.Authentication;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models.Dtos;
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
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult> Login(LoginUser loginUser)
        {
            var user = await _repo.GetUserByEmailAsync(loginUser.Email);
            if (user is null)
            {
                return BadRequest(new ErrorResponse("Email or password provided is invalid"));
            }
            if (!_authService.VerifyUserPassword(loginUser.Password, user.Password))
            {
                return BadRequest(new ErrorResponse("Email or password provided is invalid"));
            }

            return Ok(new { token = _authService.GenerateJwtToken(user) });
        }
    }
}
