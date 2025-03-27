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
using DineGO_Client.Models.Custom;

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
            // Giả sử bạn có endpoint cho restaurant owner, ví dụ:
            string restaurantOwnerUrl = string.Format(ApiEndpoints.RESTAURANT_OWNER_BY_CUS_ID, cus_id);
            var restaurantOwners = await _apiService.GetAsync<List<RestaurantOwner>>(restaurantOwnerUrl);
            var restaurant = await _apiService.GetAsync<List<Restaurant>>(ApiEndpoints.RESTAURANT);
            var reservation = await _apiService.GetAsync<List<Reservation>>(ApiEndpoints.RESERVATION);

            var viewModel = new CustomProfileViewModel
            {
                Customer = customer,
                RestaurantOwners = restaurantOwners,
                Restaurant = restaurant,
                Reservation = reservation
            };
            return View(viewModel);
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
                TempData.Keep("SuccessMessage");
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại!";
                TempData.Keep("ErrorMessage");
            }
            return RedirectToAction("Profile", "Customer");
        }

        public async Task<IActionResult> OderHistory()
        {
            int customerId = HttpContext.Session.GetInt32("cus_id") ?? 0;

            var response = await _apiService.GetAsync<List<Reservation>>($"{ApiEndpoints.RESERVATION_BY_CUSID}{customerId}");

            return View(response ?? new List<Reservation>());
        }

        public async Task<IActionResult> PaymentHistory()
        {
            int customerId = HttpContext.Session.GetInt32("cus_id") ?? 0;

            var response = await _apiService.GetAsync<List<Payment>>($"{ApiEndpoints.PAYMENT_BY_CUSID}{customerId}");

            return View(response ?? new List<Payment>());
        }
    }
}