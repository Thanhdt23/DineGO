using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public RestaurantController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }
    
        public async Task<IActionResult> Index(){
            var response = await _apiService.GetAsync<List<Restaurant>>(ApiEndpoints.RESTAURANT);
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync<Restaurant>($"{ApiEndpoints.RESTAURANT}/{id}");
            return View(response);
        }
    }
}