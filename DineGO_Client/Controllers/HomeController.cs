using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DineGO_Client.Models;
using Core.Services;
using Core.Constant;
using DineGO_Client.Model;

namespace DineGO_Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApiService apiService, ILogger<HomeController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

      public async Task<IActionResult> Search(string name, string address)
{
    if (string.IsNullOrEmpty(address))
    {
        address = "";
    }

    string searchUrl = string.Format(ApiEndpoints.RESTAURANT_SEARCH, name ?? "", address);
    var restaurants = await _apiService.GetAsync<List<Restaurant>>(searchUrl);
    return Json(restaurants);
}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
