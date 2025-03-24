using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Controllers;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
         private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public BlogController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<Blog>>(ApiEndpoints.BLOG);
            return View(response);
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