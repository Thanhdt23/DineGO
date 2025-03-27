using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation(
            [FromForm] Reservation model,
            [FromForm] string reser_date_date,
            [FromForm] string reser_date_time)
        {
            int customerId = HttpContext.Session.GetInt32("cus_id") ?? 0;
            if (customerId == 0)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập!";
                return RedirectToAction("Index", new { id = model.res_id });
            }


            string dateTimeString = $"{reser_date_date} {reser_date_time}";

            DateTime reservationDate;
            if (!DateTime.TryParseExact(
                dateTimeString,
                new[] { "yyyy-MM-dd HH:mm", "yyyy-MM-dd H:mm" },  // Chấp nhận cả định dạng H:mm
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out reservationDate))
            {
                TempData["ErrorMessage"] = "Ngày giờ không hợp lệ! Định dạng phải là yyyy-MM-dd HH:mm.";
                return RedirectToAction("Index", new { id = model.res_id });
            }

            if (reservationDate < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Không thể đặt chỗ trong quá khứ!";
                return RedirectToAction("Index", new { id = model.res_id });
            }
            var reservation = new Reservation
            {
                cus_id = customerId,
                res_id = model.res_id,
                reser_date = reservationDate,
                reser_quantity = model.reser_quantity,
                reser_status = "Pending",
                reser_note = model.reser_note
            };

            var response = await _apiService.PostAsync<Reservation, Reservation>($"{ApiEndpoints.RESERVATION}", reservation);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Đặt chỗ thành công!";
                return RedirectToAction("Index", new { id = model.res_id });
            }

            TempData["ErrorMessage"] = "Đặt chỗ thất bại!";
            return RedirectToAction("Index", new { id = model.res_id });
        }

        [HttpGet("Payment")]
        public IActionResult Payment()
        {
            return View();
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