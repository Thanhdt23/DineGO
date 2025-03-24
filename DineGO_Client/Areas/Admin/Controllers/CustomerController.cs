using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Controllers;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
      
        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;
        public CustomerController(ILogger<AuthController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<Customer>>(ApiEndpoints.CUSTOMER);
            return View(response);
        }
        public IActionResult AddCustomer()
        {
            return View();
        }
        public IActionResult UpdateCustomer()
        {
            return View();
        }
        public IActionResult DeleteCustomer()
        {
            return View();
        }
    }
}