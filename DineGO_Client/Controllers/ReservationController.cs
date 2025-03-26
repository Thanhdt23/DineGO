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
    public class ReservationController : Controller
    {
        private readonly ILogger<Reservation> _logger;
        private readonly ApiService _apiService;

        public ReservationController(ILogger<Reservation> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var restaurant = await _apiService.GetAsync<Restaurant>($"{ApiEndpoints.RESTAURANT}/{id}");
            var cus_id = HttpContext.Session.GetInt32("cus_id");

            if (cus_id == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var customer = await _apiService.GetAsync<Customer>($"{ApiEndpoints.CUSTOMER}/{cus_id}");

            var viewModel = new BookingViewModel
            {
                Restaurant = restaurant,
                Customer = customer
            };

            return View(viewModel);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

    public class BookingViewModel
    {
        public Restaurant Restaurant { get; set; }
        public Customer Customer { get; set; }
    }

}