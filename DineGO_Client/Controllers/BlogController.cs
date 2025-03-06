using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ILogger<BlogController> _logger;
        public BlogController(ApiService apiService, ILogger<BlogController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<Blog>>(ApiEndpoints.BLOG);
            return View(response);
        }

        public async Task<IActionResult> ViewBlogDetail(int id)
        {
            var response = await _apiService.GetAsync<Blog>($"{ApiEndpoints.BLOG_BY_ID}{id}");
            return View(response); // Trả về model rỗng nếu lỗi
        }

    }
}