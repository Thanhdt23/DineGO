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
using Newtonsoft.Json;
using DineGO_Client.Models.Custom;

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

        public async Task<IActionResult> ProfileRestaurant(int id)
        {
            var response = await _apiService.GetAsync<Restaurant>($"{ApiEndpoints.RESTAURANT}/{id}");

            ViewBag.res_id = response.res_id;
            ViewBag.res_name = response.res_name;
            ViewBag.res_address = response.res_address;
            ViewBag.res_phone = response.res_phone;
            ViewBag.res_information = response.res_information;
            ViewBag.res_rate = response.res_rate;
            ViewBag.res_price = response.res_price;
            ViewBag.res_discount = response.res_discount;
            ViewBag.res_images = response.res_images;
            ViewBag.cate_id = response.cate_id;
            ViewBag.resOwner_id = response.resOwner_id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileRestaurant(Restaurant restaurant)
        {
            var updateData = new
            {
                res_id = restaurant.res_id,
                res_name = restaurant.res_name,
                res_address = restaurant.res_address,
                res_phone = restaurant.res_phone,
                res_information = restaurant.res_information,
                res_rate = restaurant.res_rate,
                res_price = restaurant.res_price,
                res_discount = restaurant.res_discount,
                // res_images = restaurant.res_images,
                cate_id = restaurant.cate_id,
                resOwner_id = restaurant.resOwner_id
            };
            var jsonContent = JsonConvert.SerializeObject(updateData);
            var res_id = HttpContext.Session.GetInt32("res_id");

            var response = await _apiService.PutAsync<object, dynamic>($"{ApiEndpoints.RESTAURANT}", updateData);
            if (response != null)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                TempData.Keep("SuccessMessage");
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại!";
                TempData.Keep("ErrorMessage");
            }
            return RedirectToAction("ProfileRestaurant", "RestaurantOwner", new { id = res_id });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string Name)
        {
            var cusId = HttpContext.Session.GetInt32("cus_id");

            if (!cusId.HasValue)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng!";
                return RedirectToAction("Profile", "RestaurantOwner");
            }

            var restaurantOwner = new RestaurantOwner
            {
                cus_id = cusId.Value,
                resOwner_name = Name,
                resOwner_createdDate = DateTime.Now,
                resOwner_isAuthorize = false
            };

            var response = await _apiService.PostAsync<ResOwnerResponse, dynamic>($"{ApiEndpoints.RESTAURANT_OWNER}", restaurantOwner);

            HttpContext.Session.SetInt32("resOwner_id", response.resOwner_id);

            return RedirectToAction("Profile", "RestaurantOwner" ,new { id = response.resOwner_id});
        }

        [HttpGet("Profile/{Id}")]
        public async Task<IActionResult> Profile(int Id)
        {
            // Tạo restaurant khi mở profile
            var restaurant = new
            {
                resOwner_id = Id,
                res_name = "New Restaurant",
                cate_id = 2
            };

            var responseRestaurant = await _apiService.PostAsync<ResResponse, dynamic>($"{ApiEndpoints.RESTAURANT}", restaurant);

            HttpContext.Session.SetInt32("res_id", responseRestaurant.res_id);

            if (responseRestaurant != null)
            {
                TempData["SuccessMessage"] = "Tạo nhà hàng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Tạo nhà hàng thất bại!";
            }

            return RedirectToAction("ProfileRestaurant", "RestaurantOwner", new { id = responseRestaurant.res_id });
        }
    }

    public class ResResponse
    {
        public int res_id { get; set; }
    }

    public class ResOwnerResponse
    {
        public int resOwner_id { get; set; }
    }

}