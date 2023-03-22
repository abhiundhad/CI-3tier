using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class UserController : Controller
    {

        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;

        public UserController(CiPlatformContext db, IUserRepository Idb)
        {
            _db = db;
            _Idb = Idb;


        }
        public IActionResult Index()
        {
            List<User> Users = _db.Users.ToList();
            return View(Users);
        }



        public IActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var obj = _Idb.UserExist(user.Email);
            if (obj == null)
            {

                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login", "Login");
                //return RedirectToAction("Register", "Home");  
            }
            else
            {
                ViewBag.RegError = "Email already exist";

            }
            return View();
        }


    }
}
