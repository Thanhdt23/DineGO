using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using DineGO_Api.Repository;
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
        private readonly ICustomerRepository _customerReository;
        private readonly IMailSenderRepository _mailSenderRepository;

        public AuthController(ApplicationDbContext context, TokenService tokenService, HashService hashService, ICustomerRepository customerRepository, IMailSenderRepository mailSenderRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _hashService = hashService;
            _customerReository = customerRepository;
            _mailSenderRepository = mailSenderRepository;
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

        [HttpGet("forgetpassword")]
        public IActionResult ForgetPassword([FromQuery] string email)
        {
            var customer = _customerReository.IsMailExist(email);
            if (customer == null)
            {
                return NotFound(new { message = "Email does not exist." });
            }

            string newPassword = _hashService.GenerateRandomPassword();
            string hashedPassword = _hashService.HashPassword(newPassword); // Hash mật khẩu

            _customerReository.ChangPassword(email, hashedPassword);

            _mailSenderRepository.SendMail(email, "Reset Mật Khẩu", () => $"Mật khẩu mới của bạn là: {newPassword}");

            return Ok(new { message = "New password has been sent to your email." });
        }
    }
}