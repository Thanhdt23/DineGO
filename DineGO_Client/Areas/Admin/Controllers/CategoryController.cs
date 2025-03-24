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
    public class CategoryController : Controller
    {

        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public CategoryController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

      public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<Category>>(ApiEndpoints.CATEGORY);
            return View(response);           
        }
       public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(Category model)
    {
        if (ModelState.IsValid)
        {
           await _apiService.PutAsync<Category, Category>($"{ApiEndpoints.CATEGORY}/{id}", model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> UpdateCategory(int id)
    {
        var category = await _apiService.GetAsync<Category>($"{ApiEndpoints.CATEGORY}/{id}");
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    
    [HttpPost]
    public async Task<IActionResult> UpdateCategory(int id, Category model)
    {
        if (ModelState.IsValid)
        {
            await _apiService.PutAsync($"{ApiEndpoints.CATEGORY}/{id}", model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _apiService.DeleteAsync($"{ApiEndpoints.CATEGORY}/{id}");
        return RedirectToAction(nameof(Index));
    }
    }
}