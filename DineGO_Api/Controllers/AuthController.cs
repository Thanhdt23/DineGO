using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using DineGO_Api.Model;
using DineGO_Api.Repository;
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
                return BadRequest("Invalid username or password.");
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
        
        [HttpGet("forgetpassword")]
        public IActionResult ForgetPassword([FromQuery] string email)
        {
            var customer = _customerReository.IsMailExist(email);
            if (customer == null)
            {
                return NotFound(new { message = "Email does not exist." });
            }

            string newPassword = _hashService.GenerateRandomPassword();
            string hashedPassword = _hashService.HashPassword(newPassword);

            _customerReository.ChangPassword(email, hashedPassword);

            _mailSenderRepository.SendMail(email, "Reset Mật Khẩu", () => $"Mật khẩu mới của bạn là: {newPassword}");

            return Ok(new { message = "New password has been sent to your email." });
        }
    }
}