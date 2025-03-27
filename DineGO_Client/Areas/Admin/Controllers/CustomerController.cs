using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ApiService _apiService;

        public CustomerController(ILogger<CustomerController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            var customers = await _apiService.GetAsync<List<Customer>>(ApiEndpoints.CUSTOMER);
            return View(customers);
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var addData = new
                {
                    cus_id = customer.cus_id,
                    cus_username = customer.cus_username,
                    cus_password = customer.cus_password,
                    cus_name = customer.cus_name,
                    cus_email = customer.cus_email,
                    cus_phone = customer.cus_phone,
                    cus_address = customer.cus_address,
                    cus_birthday = customer.cus_birthday,
                    cus_gender = customer.cus_gender,
                    cus_image = customer.cus_image,
                    cus_isKYI = customer.cus_isKYI
                };
                var response = await _apiService.PostAsync<object, dynamic>($"{ApiEndpoints.CATEGORY}", addData);
                return RedirectToAction("Index");
            }
            return View(customer);
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