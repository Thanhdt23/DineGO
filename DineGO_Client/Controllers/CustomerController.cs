using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Services;
using Core.Constant;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DineGO_Client.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<Customer> _logger;
        private readonly ApiService _apiService;

        public CustomerController(ILogger<Customer> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var cus_id = HttpContext.Session.GetInt32("cus_id");
            var customer = await _apiService.GetAsync<Customer>($"{ApiEndpoints.CUSTOMER}/{cus_id}");
            return View(customer);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Customer customer, IFormFile imageFile)
        {
            if (customer == null)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ!");
                return View("Profile", customer);
            }

            var cus_id = HttpContext.Session.GetInt32("cus_id");

            // Xử lý upload ảnh đại diện
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/client/images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn vào customer
                customer.cus_image = uniqueFileName;
            }

            var updateData = new
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
            var jsonContent = JsonConvert.SerializeObject(updateData);

            var response = await _apiService.PutAsync<object, dynamic>($"{ApiEndpoints.CUSTOMER}/{cus_id}", updateData);
            if (response != null)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại!";
            }
            return View("Profile", customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}