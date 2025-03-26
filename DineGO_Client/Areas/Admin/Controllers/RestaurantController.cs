using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestaurantController : Controller
    {
         private readonly ApiService _apiService;
         public RestaurantController(ApiService apiService){
            _apiService = apiService;
         }
        
        public async Task<IActionResult> Restaurant()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            var restaurants = await _apiService.GetAsync<List<Restaurant>>(ApiEndpoints.RESTAURANT);
            return View(restaurants);
        }
        [HttpGet]
        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRestaurant(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync<dynamic, Restaurant>($"{ApiEndpoints.RESTAURANT}", restaurant);
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }
        public IActionResult UpdateRestaurant()
        {
            return View();
        }
        public IActionResult DeleteRestaurant()
        {
            return View();
        }
    }
}