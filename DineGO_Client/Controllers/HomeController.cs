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
using Microsoft.AspNetCore.Http;

namespace DineGO_Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController( ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorMessage = HttpContext.Session.GetString("ErrorMessage");
            var model = new ErrorViewModel 
            { 
                ErrorMessage = errorMessage, 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            };
            return View(model);
        }
    }
}
