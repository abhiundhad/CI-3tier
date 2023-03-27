using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _Idb;

        public UserController(IUserRepository Idb)
        {
           
            _Idb = Idb;


        }
        public IActionResult Index()
        {
            List<User> Users = _Idb.alluser();
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
                _Idb.Adduser(user);

                return RedirectToAction("Login", "Login");

            }
            else
            {
                ViewBag.RegError = "Email already exist";

            }
            return View();
        }



    }
}
