using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DineGO_Client.Models;
using DineGO_Client.Model;
using Core.Services;
using Core.Constant;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace DineGO_Client.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class RestaurantOwnerController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ILogger<RestaurantOwnerController> _logger;

        public RestaurantOwnerController(ApiService apiService, ILogger<RestaurantOwnerController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            var response = await _apiService.GetAsync<List<RestaurantOwner>>(ApiEndpoints.RESTAURANT_OWNER);
            return View(response);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantOwner owner)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync<RestaurantOwner, RestaurantOwner>(ApiEndpoints.RESTAURANT_OWNER, owner);
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        public async Task<IActionResult> Edit(int id)
        {

            try
            {
                var response = await _apiService.GetAsync<RestaurantOwner>($"{ApiEndpoints.RESTAURANT_OWNER}/{id}");
                if (response == null)
                {
                    _logger.LogError($"No RestaurantOwner found with ID: {id}");
                    return NotFound();
                }
                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching RestaurantOwner with ID {id}: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RestaurantOwner owner)
        {

            var updateData = new
            {
                resOwner_id = owner.resOwner_id,
                cus_id = owner.cus_id,
                resOwner_name = owner.resOwner_name,
                resOwner_createdDate = owner.resOwner_createdDate,
                resOwner_isAuthorize = owner.resOwner_isAuthorize,
            };
            var jsonContent = JsonConvert.SerializeObject(updateData);

            var response = await _apiService.PutAsync<object, dynamic>($"{ApiEndpoints.RESTAURANT_OWNER}/{owner.resOwner_id}", updateData);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại!";
                return View(owner);
            }
        }

        // Add this to your RestaurantOwnerController.cs
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _apiService.GetAsync<RestaurantOwner>($"{ApiEndpoints.RESTAURANT_OWNER}/{id}");
                if (response == null)
                {
                    _logger.LogError($"No RestaurantOwner found with ID: {id}");
                    return NotFound();
                }
                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching RestaurantOwner with ID {id}: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {   
            Console.WriteLine("aaaaaaaaa" + id);
            try
            {
                var response = await _apiService.DeleteAsync<object>($"{ApiEndpoints.RESTAURANT_OWNER}/{id}");
                TempData["SuccessMessage"] = "Xóa chủ nhà hàng thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting RestaurantOwner with ID {id}: {ex.Message}");
                TempData["ErrorMessage"] = "Xóa chủ nhà hàng thất bại!";
                return RedirectToAction("Index");
            }
        }
    }
}
