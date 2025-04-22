using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Constant;
using Core.Services;
using DineGO_Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Admin.Controllers
{
   
    public class ReservationController : Controller
    {
        private readonly ApiService _apiService;
         public ReservationController(ApiService apiService){
            _apiService = apiService;
         }
       public async Task<IActionResult> Reservation()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            var reservations = await _apiService.GetAsync<List<Reservation>>(ApiEndpoints.RESERVATION);
            foreach (var r in reservations)
            {
                // Ví dụ: nếu ApiEndpoints.CATEGORY_BY_ID không có dấu '/' cuối, hãy cẩn thận với url.
                var restaurant = await _apiService.GetAsync<Restaurant>($"{ApiEndpoints.RESTAURANT}/{r.res_id}");
                var customer = await _apiService.GetAsync<Customer>($"{ApiEndpoints.CUSTOMER}/{r.cus_id}");
                r.customer = customer;
                r.restaurant = restaurant;
            }
            return View(reservations);
        }
        public IActionResult AddReservation()
        {
            return View();
        }
        public IActionResult UpdateReservation()
        {
            return View();
        }

        public IActionResult DeleteReservation()
        {
            return View();
        }
    }
}