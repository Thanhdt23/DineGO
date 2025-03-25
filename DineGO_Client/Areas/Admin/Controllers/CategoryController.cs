using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var response = await _apiService.GetAsync<List<Category>>(ApiEndpoints.CATEGORY);
            return View(response);
        }

      [HttpGet]
        public IActionResult AddCategory()
        {
            return View(new Category());
        }

        // POST: /Admin/Category/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category model)
        {
            _logger.LogInformation("Received POST request to add category: {@Category}", model);
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _apiService.PostAsync<Category, Category>(ApiEndpoints.CATEGORY, model);
                    if (response != null)
                    {
                        _logger.LogInformation("Category added successfully: {@Category}", response);
                        return RedirectToAction(nameof(Index));
                    }
                    _logger.LogWarning("API returned null when adding category.");
                    ModelState.AddModelError("", "Error adding category.");
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "API POST failed: {Message}", ex.Message);
                    ModelState.AddModelError("", $"API error: {ex.Message}");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Model state invalid. Errors: {@Errors}", errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        // public IActionResult AddCategory()
        // {
        //     return View();
        // }

        // [HttpPost]
        // public async Task<IActionResult> AddCategory(Category model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         await _apiService.PostAsync<Category, Category>($"{ApiEndpoints.CATEGORY}", model);
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(model);
        // }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _apiService.GetAsync<Category>($"{ApiEndpoints.CATEGORY}/{id}");
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        // [HttpPost]
        // public async Task<IActionResult> UpdateCategory(int id, Category model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         await _apiService.PutAsync<Category, Category>($"{ApiEndpoints.CATEGORY}/{id}", model);
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(model);
        // }


        // [HttpPost]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     await _apiService.DeleteAsync<object>($"{ApiEndpoints.CATEGORY}/{id}");
        //     return RedirectToAction(nameof(Index));
        // }
    }
}