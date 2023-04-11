using CI.Models;
using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using System.Diagnostics;

namespace CI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;


        public HomeController(ILogger<HomeController> logger, CiPlatformContext db, IUserRepository Idb)
        {
            _logger = logger;
            _db = db;
            _Idb = Idb;
        }
        public IActionResult Index()
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
        public IActionResult Userprofile()
        {
            try
            {

            
            var sessionUserId = HttpContext.Session.GetString("userID");
            var id = Convert.ToInt64(sessionUserId);
            var user = _Idb.alluser().FirstOrDefault(u => u.UserId == id);
            UserprofileViewModel userVM = new UserprofileViewModel();

            userVM.employeeid = user.EmployeeId;
            userVM.firstname = user.FirstName;
            userVM.lastname = user.LastName;
            userVM.email = user.Email;
            userVM.whyivolunteered = user.WhyIVolunteer;
            userVM.title = user.Title;
            userVM.cityid = user.CityId;
            userVM.countryid = user.CountryId;
            //userVM.availability = user.
            userVM.myprofile = user.ProfileText;
            userVM.linkedinurl = user.LinkedInUrl;
            userVM.avatar = user.Avatar!=null?user.Avatar:"";
            userVM.department = user.Department;
            userVM.cityid = user.CityId;
            userVM.countryid = user.CountryId;
            userVM.availability = user.Availability;
            var allskills = _Idb.skillList();
            ViewBag.allskills = allskills;
            var skills = from US in _db.UserSkills
                         join S in _db.Skills on US.SkillId equals S.SkillId
                         select new { US.SkillId, S.SkillName, US.UserId };
            var uskills = skills.Where(e => e.UserId == id).ToList();
            ViewBag.userskills = uskills;
            foreach (var skill in uskills)
            {
                var rskill = allskills.FirstOrDefault(e => e.SkillId == skill.SkillId);
                allskills.Remove(rskill);
            }
            ViewBag.remainingSkills = allskills;
            ViewBag.allcities = _Idb.CityList();
            ViewBag.allcountry = _Idb.CountryList();
            return View(userVM);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }
//        var sessionUserId = HttpContext.Session.GetString("userID");
//        var ID = Convert.ToInt64(sessionUserId);
//        var user = _Idb.alluser().FirstOrDefault(x => x.UserId == ID);
//            if (user.Password != model.OldPassword)
//            {
//                ViewBag.wrongoldpass = "Wrong Old password";
//            }
//            else
//            {
//                _Idb.changepassword(model.NewPassword, ID);
//            }
//TempData["userpasschang"] = "Password change Successfully";
         [HttpPost]
        public async Task<IActionResult> Userprofile(UserprofileViewModel model, IFormFileCollection files)
        {
            try
            {

            
            var sessionUserId = HttpContext.Session.GetString("userID");
                var id = Convert.ToInt64(sessionUserId);
            //long id = Convert.ToInt64(userid);
            //long storyid = model.storyId;
            var userdetail = _Idb.alluser().FirstOrDefault(u => u.UserId == id);
            userdetail.FirstName = model.firstname;
            userdetail.LastName = model.lastname;
            userdetail.WhyIVolunteer = model.whyivolunteered;
            userdetail.Title = model.title;
            
            userdetail.EmployeeId = model.employeeid;
            userdetail.ProfileText = model.myprofile;
            userdetail.LinkedInUrl = model.linkedinurl;
            userdetail.UpdatedAt = DateTime.Now;
            userdetail.Department = model.department;
            userdetail.CountryId = model.countryid;
            userdetail.CityId = model.cityid;
            userdetail.Availability = model.availability;

            if (files.Count()==0)
            {
                model.avatar = userdetail.Avatar;
            }
            else
            {
                userdetail.Avatar = model.avatar;
           
            }
            foreach (var file in files)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    userdetail.Avatar = "data:image/png;base64," + base64String;
                    model.avatar = "data:image/png;base64," + base64String;
                    HttpContext.Session.SetString("useravtar", "data:image/png;base64," + base64String);
                }
            }

            var allskills = _Idb.skillList();
            ViewBag.allskills = allskills;
            var skills = from US in _db.UserSkills
                         join S in _db.Skills on US.SkillId equals S.SkillId
                         select new { US.SkillId, S.SkillName, US.UserId };
            var uskills = skills.Where(e => e.UserId == id).ToList();
            ViewBag.userskills = uskills;
            foreach (var skill in uskills)
            {
                var rskill = allskills.FirstOrDefault(e => e.SkillId == skill.SkillId);
                allskills.Remove(rskill);
            }
            ViewBag.remainingSkills = allskills;
            ViewBag.allcities = _Idb.CityList();
            ViewBag.allcountry = _Idb.CountryList();

            _Idb.updateuser(userdetail);
            return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> SaveUserSkills(long[] selectedSkills)
        {
            try
            {

            
            var sessionUserId = HttpContext.Session.GetString("userID");
            var id = Convert.ToInt64(sessionUserId);
            var abc = _db.UserSkills.Where(e => e.UserId == id).ToList();
            _db.RemoveRange(abc);
            _db.SaveChanges();
            foreach (var skills in selectedSkills)
            {


                _Idb.AddUserSkills(skills, id);


            }

            return RedirectToAction("Userprofile", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }
        public IActionResult Adminindex()
        {
            return View();
        }
        public IActionResult Privacypolicy()
        {
            return View();
        }
    
        



    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(model: new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}