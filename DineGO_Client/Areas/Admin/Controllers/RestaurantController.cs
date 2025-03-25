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
    public class RestaurantController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public RestaurantController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<Restaurant>>(ApiEndpoints.RESTAURANT);
            return View(response);
        }
        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpGet]
        [Route("Restaurant/UpdateRestaurant/{id:int:min(1)}")]
        public async Task<IActionResult> UpdateRestaurant(int id)
        {
            var restaurant = await _apiService.GetAsync<Restaurant>($"{ApiEndpoints.RESTAURANT}/{id}");
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRestaurant(Restaurant model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync<Restaurant, Restaurant>($"{ApiEndpoints.RESTAURANT}/{model.res_id}", model);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating restaurant.");
            }
            return View(model);
        }

        public IActionResult DeleteRestaurant()
        {
            return View();
        }
    }
}