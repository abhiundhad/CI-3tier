using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("Nav", 1);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            return View();
        }
        public IActionResult AdminMission()
        {
            HttpContext.Session.SetInt32("Nav", 3);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            return View();
        }
        public IActionResult AdminCms()
        {
            HttpContext.Session.SetInt32("Nav", 2);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            return View();
        }



    }
}
