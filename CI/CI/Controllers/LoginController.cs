

using CI.Models;
using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _Idb;
        
        public LoginController( IUserRepository Idb)
        {
       

            _Idb = Idb;
        }
        public IActionResult Login(long missionid ,int StoryId)
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Landingpage", "Landingpage");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (model.MissionId !=0 && model.MissionId != null )
            {
                // var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                var user = _Idb.Login(model.Email, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("userID", user.UserId.ToString());
                    HttpContext.Session.SetString("Firstname", user.FirstName);
                    return RedirectToAction("Volunteering", "Volunteering", new { @id = user.UserId, missionid = model.MissionId });

                }
                else 
                { 
                    return View(); 
                }

            }
            else if( model.StoryId != null)
            {
                //var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                var user = _Idb.Login(model.Email, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("userID", user.UserId.ToString());
                    HttpContext.Session.SetString("Firstname", user.FirstName);
                    return RedirectToAction("StoryDetail", "StoryListing", new { @id = user.UserId, StoryId = model.StoryId });

                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (ModelState.IsValid)
                {


                  
                    //var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                    var user = _Idb.Login(model.Email, model.Password);
                    var username = model.Email.Split("@")[0];
                    if (user != null)
                    {
                        HttpContext.Session.SetString("userID", user.UserId.ToString());
                        HttpContext.Session.SetString("Firstname", user.FirstName);

                        return RedirectToAction("Landingpage", "Landingpage", new { @id = user.UserId });
                    }
                    else
                    {
                        ViewBag.Error = "Email or Password is Incorrect";

                    }
                }
                return View();

            }


        }
        //    [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction(nameof(LoginController.Login), "Login");
        //}
    }

  
}
