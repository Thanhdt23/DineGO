using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Core.Common;
namespace DineGO_Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public AuthController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            var response = await _apiService.PostAsync<LoginResponse, dynamic>("auth/login", loginData);
            if (response != null)
            {
                HttpContext.Session.SetString("token", response.token);
                HttpContext.Session.SetInt32("cus_id", response.cus_id);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string username, string password, string email, string phone)
        {
            var registerData = new { Username = username, Password = password, Name = name, Email = email, Phone = phone };
            var response = await _apiService.PostAsync<RegisterResponse, dynamic>("auth/register", registerData);

            if (response != null && response.Message == "User registered successfully.")
            {
                ViewBag.Success = "Registration successful. Please login.";
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Error = "Username already exists.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("cus_id");
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        public class LoginResponse
        {
            public string token { get; set; }
            public int cus_id { get; set; }
        }

        public class RegisterResponse
        {
            public string Message { get; set; }
        }

    }
}