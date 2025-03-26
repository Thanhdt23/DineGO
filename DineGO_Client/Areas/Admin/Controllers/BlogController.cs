using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly ApiService _apiService;

        public BlogController(ILogger<BlogController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            var categories = await _apiService.GetAsync<List<Blog>>(ApiEndpoints.BLOG);
            return View(categories);
        }
        public IActionResult AddBlog()
        {
            return View();
        }

        public IActionResult UpdateBlog()
        {
            return View();
        }

        public IActionResult DeleteBlog()
        {
            return View();
        }
    }
}