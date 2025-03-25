using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using DineGO_Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
           if (_context.customers.AsNoTracking().Any(u => u.cus_username == registerRequest.Username))
                return Ok(new { Message = "Username already exists." });

            var cus = new Customer
            {
                cus_name = registerRequest.Name,
                cus_username = registerRequest.Username,
                cus_password = _hashService.HashPassword(registerRequest.Password),
                cus_email = registerRequest.Email,
                cus_phone = registerRequest.Phone,
            };
            _context.customers.Add(cus);
            _context.SaveChanges();
            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.customers.SingleOrDefault(u => u.cus_username == loginRequest.Username);
            if (user == null || !_hashService.VerifyPassword(loginRequest.Password, user.cus_password))
                return Unauthorized("Invalid username or password.");
            var token = _tokenService.GenerateToken(loginRequest.Username);
            var cus_id = user.cus_id;
            return Ok(new { Token = token , Cus_id = cus_id});
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


        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Email {get; set;}
            public string Phone {get; set;}
        }
    }
}