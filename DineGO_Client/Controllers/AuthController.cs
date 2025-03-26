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
using System.Text.Json;
using System.Net.Http;
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
            try
            {
                var loginData = new { Username = username, Password = password };
                var response = await _apiService.PostAsync<LoginResponse, dynamic>("auth/login", loginData);
                HttpContext.Session.SetString("token", response.token);
                HttpContext.Session.SetString("cus_name", response.cus_name);
                HttpContext.Session.SetInt32("cus_id", response.cus_id);
                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(string cus_name, string cus_username, string cus_password, string cus_email, string cus_phone)
        {
            var registerData = new { Username = cus_username, Password = cus_password, Name = cus_name, Email = cus_email, Phone = cus_phone };
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
            public string cus_name { get; set; }
        }

        public class RegisterResponse
        {
            public string Message { get; set; }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Vui lòng nhập email.";
                return View();
            }

            // Gửi yêu cầu đến API
            var response = await _apiService.GetAsync<JsonElement>($"auth/forgetpassword?email={email}");

            if (response.TryGetProperty("message", out JsonElement messageElement) && messageElement.GetString() == "Email does not exist.")
            {
                ViewBag.Message = "Email không tồn tại trong hệ thống.";
                return View();
            }

            ViewBag.Message = "Mật khẩu mới đã được gửi đến email của bạn.";
            return View();
        }

    }
}