using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DineGO_Client.Models;
using DineGO_Client.Model;

namespace DineGO_Client.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class RestaurantOwnerController : Controller
    {
        private readonly ILogger<RestaurantOwnerController> _logger;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:5001/api/RestaurantOwner";

        public RestaurantOwnerController(ILogger<RestaurantOwnerController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var owners = await _httpClient.GetFromJsonAsync<List<RestaurantOwner>>(ApiUrl);
            return View(owners);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RestaurantOwner owner)
        {
            if (!ModelState.IsValid)
                return View(owner);

            await _httpClient.PostAsJsonAsync(ApiUrl, owner);
            return RedirectToAction("Index");
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var owner = await _httpClient.GetFromJsonAsync<RestaurantOwner>($"{ApiUrl}/{id}");
            if (owner == null) return NotFound();
            return View(owner);
        }
        public async Task<IActionResult> EditAuthorize(int resOwner_id, string resOwner_isAuthorize)
        {
            var owner = await _httpClient.GetFromJsonAsync<RestaurantOwner>($"{ApiUrl}/{resOwner_id}");
            if (owner == null) return NotFound();

            // Chuyển đổi giá trị từ form (string) sang bool
            owner.resOwner_isAuthorize = resOwner_isAuthorize.ToLower() == "true";

            // Gửi dữ liệu đã cập nhật lên API
            await _httpClient.PutAsJsonAsync($"{ApiUrl}/{resOwner_id}", owner);

            return RedirectToAction(nameof(Index));
        }





        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            return RedirectToAction("Index");
        }
    }
}
