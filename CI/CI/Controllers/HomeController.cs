using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CiPlatformContext _db;

        public HomeController(ILogger<HomeController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult register()
        {
            return View();
        }
        public IActionResult login()
        {
            return View();
        }
        public IActionResult landingpage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Forget()
        {
            return View();
        } 
 
       
    
        public IActionResult nomission(long ID)
        {
            List<Mission> mission = _db.Missions.ToList();
            List<City> Cityy = _db.Cities.ToList();
            ViewBag.Cityy = Cityy;

            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;

            return View();
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(model: new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}