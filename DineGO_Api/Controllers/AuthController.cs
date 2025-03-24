using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace DineGO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;

        public AuthController(ApplicationDbContext context, TokenService tokenService, HashService hashService)
        {
            _context = context;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.customers.SingleOrDefault(u => u.cus_username == loginRequest.Username);
            if (user == null || !_hashService.VerifyPassword(loginRequest.Password, user.cus_password))
                return Unauthorized("Invalid username or password.");
            var token = _tokenService.GenerateToken(loginRequest.Username);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok("This is a protected endpoint.");
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}