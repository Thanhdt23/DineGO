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
              _logger.LogInformation($"Response for ID {id}: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
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
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync<RestaurantOwner, RestaurantOwner>(ApiEndpoints.RESTAURANT_OWNER, owner);
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<int>($"{ApiEndpoints.RESTAURANT_OWNER_BY_ID}{id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting restaurant owner with ID {id}: {ex.Message}");
                return RedirectToAction("Index");
            }
        }


    }
}
