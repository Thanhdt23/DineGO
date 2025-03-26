using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Controllers
{
    [Route("[controller]")]
    public class RestaurantOwnerController : Controller
    {
        private readonly ILogger<RestaurantOwnerController> _logger;
        private readonly ApiService _apiService;

        public RestaurantOwnerController(ILogger<RestaurantOwnerController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public IActionResult Index(int Id)
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string Name)
        {
            var restaurantOwner = new RestaurantOwner
            {
                cus_id = HttpContext.Session.GetInt32("cus_id").Value,
                resOwner_name = Name,
                resOwner_createdDate = DateTime.Now,
                resOwner_isAuthorize = false
            };
            var response = await _apiService.PostAsync<dynamic, RestaurantOwner>($"{ApiEndpoints.RESTAURANT_OWNER}", restaurantOwner);
            TempData["SuccessMessage"] = "Tạo nhà hàng thành công!";
            TempData.Keep("SuccessMessage");
            return RedirectToAction("Profile", "Customer");
        }
    }
}