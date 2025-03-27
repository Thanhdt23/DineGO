using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect về page Login (giả sử controller Auth nằm bên Root area)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
      
    }
}