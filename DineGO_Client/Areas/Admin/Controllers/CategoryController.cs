using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApiService _apiService;

        public CategoryController(ILogger<CategoryController> logger, ApiService apiService)
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
            var categories = await _apiService.GetAsync<List<Category>>(ApiEndpoints.CATEGORY);
            return View(categories);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            
            if (ModelState.IsValid)
            {
                var addData = new
                {
                    cate_id = category.cate_id,
                    cate_type = category.cate_type,
                    cate_description = category.cate_description
                };
                var response = await _apiService.PostAsync<object, dynamic>($"{ApiEndpoints.CATEGORY}", addData);
                return RedirectToAction("Index");
            }
            return View(category);
        }



        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {    
            var category = await _apiService.GetAsync<Category>($"{ApiEndpoints.CATEGORY}/id?ID={id}"); 
            System.Console.WriteLine("cate"  + category);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(category));

            if (category == null)
            {
                ModelState.AddModelError("", "Dữ liệu category không hợp lệ!");
                return View(category);
            }

            var updateData = new
            {
                cate_id = category.cate_id,
                cate_type = category.cate_type,
                cate_description = category.cate_description

            };
            System.Console.WriteLine("updateData" + updateData);

            // Gọi API để cập nhật
            var response = await _apiService.PutAsync<object, dynamic>($"{ApiEndpoints.CATEGORY}", updateData);


            return RedirectToAction("Index");
        }
        public IActionResult DeleteCategory()
        {
            return View();
        }
    }
}

