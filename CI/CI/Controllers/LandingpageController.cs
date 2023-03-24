using CI.Models;
using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User = CI_Entity.Models.User;

namespace CI.Controllers
{
    public class LandingpageController : Controller
    {
        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;
        private int? pageIndex;

        public LandingpageController(CiPlatformContext db, IUserRepository Idb)
        { 
            _Idb = Idb;
            _db = db;


        }

        #region Landingpage
        public IActionResult Landingpage(long userId, int id, int missionid, string? search, int? pageIndex, string? sortValue, string[] country, string[] city, string[] theme)
        {
            var UserId = HttpContext.Session.GetString("userID");
            ViewBag.Country = _Idb.CountryList();
            ViewBag.Themes = _Idb.ThemeList();
            ViewBag.Cityy=_Idb.CityList();
            return View();
        }
        #endregion

        #region Filters
        public IActionResult Filters(long userId, int id, int missionid, string? search, int? pageIndex, string? sortValue, string[] country, string[] city, string[] theme)
        {
            var SessionUserId = HttpContext.Session.GetString("userID");

            List<User> alluser = _Idb.alluser();
            List<VolunteeringVM> allavailuser = new List<VolunteeringVM>();
            foreach (var all in alluser)
            {
                allavailuser.Add(new VolunteeringVM
                {
                    username = all.FirstName,
                    lastname = all.LastName,
                    userEmail = all.Email,
                    UserId = all.UserId,
                });

            }
            ViewBag.allavailuser = allavailuser;
            List<Mission> allmission = _Idb.MissionsList();
            List<VolunteeringVM> mission = new List<VolunteeringVM>();
            foreach (Mission item in allmission)
            {
                var City = _Idb.CityList().FirstOrDefault(u => u.CityId == item.CityId);
                var Country = _Idb.CountryList().FirstOrDefault(u => u.CountryId == item.CountryId);
                var Theme = _Idb.ThemeList().FirstOrDefault(u => u.MissionThemeId == item.ThemeId);
                var goalobj = _Idb.GoalsList().FirstOrDefault(m => m.MissionId == item.MissionId);
                string[] Startdate1 = item.StartDate.ToString().Split(" ");
                string[] Enddate2 = item.EndDate.ToString().Split(" ");
                var favrioute = (id != null) ? _Idb.favoriteMissions().Any(u => u.UserId == Convert.ToInt64(SessionUserId) && u.MissionId == item.MissionId) : false;
                var Applybtn = (id != null) ? _Idb.missionApplications().Any(u => u.MissionId == item.MissionId && u.UserId == Convert.ToInt64(SessionUserId)) : false;
                ViewBag.FavoriteMissions = favrioute;
                var rat = _Idb.missionRatingList().Where(u => u.MissionId == item.MissionId).ToList();
                int finalrat = 0;
                if (rat.Count > 0)
                {
                    int rating = 0;
                    foreach (var items in rat)
                    {

                        rating = rating + int.Parse(items.Rating);

                    }
                    finalrat = rating / rat.Count();

                }




                mission.Add(new VolunteeringVM
                {
                    MissionId = item.MissionId,
                    Cityname = City.Name,
                    Countryname = Country.Name,
                    Themename = Theme.Title,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription,
                    StartDate = Startdate1[0],
                    EndDate = Enddate2[0],
                    Availability = item.Availability,
                    OrganizationName = item.OrganizationName,
                    GoalObjectiveText = goalobj.GoalObjectiveText,
                    MissionType = item.MissionType,
                    AvrageRating = finalrat,
                    isFavriout = favrioute,
                    isApplied = Applybtn,
                     UserId= Convert.ToInt64(SessionUserId),
                });
            }
          
            var Missions = mission.ToList();
           

            //Seacrh
            if (search != null)
            {
                Missions = Missions.Where(m => m.Title.ToUpper().Contains(search.ToUpper())).ToList();
               
            }

            ////Sort By
            ViewBag.sort = sortValue;
            switch (sortValue)
            {

                case "Newest":
                    Missions = Missions.OrderByDescending(m => m.StartDate).ToList();
                    ViewBag.sortby = "Newest";
                    break;
                case "Oldest":
                    Missions = Missions.OrderBy(m => m.StartDate).ToList();
                    ViewBag.sortby = "Oldest";
                    break;
                case "Lowest seats":
                    Missions = Missions.OrderBy(m => int.Parse(m.Availability)).ToList();
                    break;
                case "Highest seats":
                    Missions = Missions.OrderByDescending(m => int.Parse(m.Availability)).ToList();
                    break;
                case "My favourites":
                    Missions = Missions.Where(m =>m.isFavriout==true).ToList();
                    break;
                case "Registration deadline":
                    Missions = Missions.OrderBy(m => m.EndDate).ToList();
                    break;
            }

            //filter
            if (country.Length > 0)
            {
                Missions = Missions.Where(s => country.Contains(s.Countryname)).ToList();
            }
            if (city.Length > 0)
            {
                Missions = Missions.Where(s => city.Contains(s.Cityname)).ToList();
            }
            if (theme.Length > 0)
            {
                Missions = Missions.Where(s => theme.Contains(s.Themename)).ToList();
            }

            //Pagination
            //int pageSize = 6;
            //int skip = (pageIndex ?? 0) * pageSize;
            //var Missionss = Missions.Skip(skip).Take(pageSize).ToList();
            //int totalMissions = mission.Count();

            //ViewBag.TotalMission = totalMissions;
            //ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            //ViewBag.CurrentPage = pageIndex ?? 0;
            var missionfinal = Missions;


            return PartialView("_Missioncards_partialView", missionfinal);
        }


        #endregion

    }
}
