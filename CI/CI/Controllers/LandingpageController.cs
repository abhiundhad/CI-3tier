using CI.Models;
using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Management.Smo;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Web;
using User = CI_Entity.Models.User;

namespace CI.Controllers
{
    public class LandingpageController : Controller
    {
        //int co = 0;
        //int ci = 0;
        //int th = 0;
        //int co1 = 0;
        //int ci1 = 0;
        //int th1 = 0;
        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;
        private int? pageIndex;

        public LandingpageController(CiPlatformContext db, IUserRepository Idb)
        {
            _db = db;
            _Idb = Idb;


        }
        public IActionResult Landingpage(long id)

        {
            int? usrid = HttpContext.Session.GetInt32("userID");
            if (usrid == null)
            {
                TempData["Sessonerrormsg"] = "please login again";
                return RedirectToAction("Login", "Login");
            }
            else
            {


                return View();
            }
        }
        [HttpPost]
        public IActionResult Landingpage()
        {
            return View();
        }

        public async Task<IActionResult> Landingpage2(long id, int? pageIndex, String searchinput, string searchQuery, long[] fCountries, long[] fCitys, long[] fThemes, string sortorder)
        {
            int? usrid = HttpContext.Session.GetInt32("userID");
            //if (usrid == null)
            //{
            //    TempData["Sessonerrormsg"] = "please login again";
            //    return RedirectToAction("Login", "Login");

            //}
            //else
            //{
            //var userId = HttpContext.Session.GetString("userID");
            //ViewBag.UserId = int.Parse(userId);

            //List<City> cityname = new List<City>();
            //List<City> cityname1 = new List<City>();
            //List<Mission> mission = _db.Missions.ToList();
            //List<Mission> allmission = _db.Missions.ToList();
            //List<Mission> foundmission = new List<Mission>();
            //List<Mission> finalmissionfound = new List<Mission>();
            //List<Mission> finalmissionfound2 = new List<Mission>();
            //List<MissionRating> rats = new List<MissionRating>();
            //List<int> rats2 = new List<int>();
            //long[] missionempty;

            //List<City> Cityy = _db.Cities.ToList();
            //ViewBag.Cityy = Cityy;

            //List<Country> Country = _db.Countries.ToList();
            //ViewBag.Country = Country;

            //List<MissionTheme> Themes = _db.MissionThemes.ToList();
            //ViewBag.Themes = Themes;
            //List<GoalMission> goal = _db.GoalMissions.ToList();
            //ViewBag.Goal = goal;
            //int ratt = 0;
            //foreach (var item in mission)
            //{
            //    var City = _db.Cities.FirstOrDefault(u => u.CityId == item.CityId);
            //    var Theme = _db.MissionThemes.FirstOrDefault(u => u.MissionThemeId == item.ThemeId);

            //    var rat = _db.MissionRatings.Where(u => u.MissionId == item.MissionId).ToList();
            //    rats.AddRange(rat);
            //    if(rat.Count > 0)
            //    {
            //        int rating = 0;
            //        foreach (var items in rats)
            //        {

            //            rating = rating + int.Parse(items.Rating);

            //        }
            //        int finalrat = rating / rats.Count();
            //        rats2.Add(finalrat);
            //    }
            //    else
            //    {
            //        int rating = 0;
            //        int finalrat = rating / rats.Count();
            //        rats2.Add(finalrat);
            //    }




            //    ViewBag.Rating = rats2;
            //    ViewBag.ratingcount = rats2.Count();


            //    var favrioute = (id != null) ? _db.FavoriteMissions.Any(u => u.UserId == id && u.MissionId == item.MissionId) : false;

            //    ViewBag.FavoriteMissions = favrioute;

            //}



            //if (!string.IsNullOrEmpty(searchQuery))
            //{
            //    mission = _db.Missions.Where(m => m.Title.Contains(searchQuery)).ToList();
            //    ViewBag.Searchinput = Request.Query["searchQuery"];
            //    if (mission.Count() == 0)
            //    {
            //        return RedirectToAction("nomission", "Home");
            //    }
            //}
            //missionempty = fCountries;
            //if (fCountries != null && fCountries.Length > 0)
            //{
            //    foreach (var country in fCountries)
            //    {

            //        if (fThemes.Length>0|| fCitys.Length>0)
            //        {

            //        }

            //        else
            //        {

            //            if (co == 0)
            //            {
            //                mission = mission.Where(m => m.CountryId == country + 2500).ToList();

            //                co++;
            //            }
            //            foundmission = foundmission.Where(m => m.CountryId == country + 2500).ToList();
            //            foundmission = allmission.Where(m => m.CountryId == country).ToList();
            //            mission.AddRange(foundmission);
            //            finalmissionfound.AddRange(foundmission);
            //        }

            //        ViewBag.SearchCountryId = country;
            //        if (ViewBag.SearchCountryId != null)
            //        {
            //            var A = _db.Countries.Where(m => m.CountryId == country).ToList();
            //            if (co1 == 0)
            //            {
            //                Country = _db.Countries.Where(m => m.CountryId == country + 5000).ToList();
            //                co1++;
            //            }
            //            Country.AddRange(A);
            //            //ViewBag.SearchCountry = A.Name;
            //        }
            //        if (mission.Count() == 0)
            //        {
            //            return RedirectToAction("nomission", "Home", new { @id = id });
            //        }

            //    }
            //    ViewBag.SearchCountry = Country;

            //}
            //if (missionempty.Count() != 0)
            //{


            //    foreach (var a in missionempty)
            //    {
            //        cityname1 = _db.Cities.Where(m => m.CountryId == a).ToList();
            //        cityname.AddRange(cityname1);
            //    }



            //    ViewBag.Cityy = cityname;
            //}
            //    if (fCitys != null && fCitys.Length > 0)
            //{
            //    foreach (var city in fCitys)
            //    {
            //        if (fCountries.Length > 0 || fThemes.Length > 0)
            //        {
            //            if (ci == 0)
            //            {
            //                mission = mission.Where(m => m.CityId == city + 5500).ToList();
            //                finalmissionfound2 = finalmissionfound;
            //                finalmissionfound = finalmissionfound.Where(m => m.CityId == city + 5500).ToList();
            //                ci++;

            //            }
            //            foundmission = foundmission.Where(m => m.CityId == city + 5500).ToList();

            //            foundmission = finalmissionfound2.Where(m => m.CityId == city).ToList();
            //            mission.AddRange(foundmission);
            //            finalmissionfound.AddRange(foundmission);
            //        }
            //        else
            //        {

            //            if (ci == 0)
            //            {
            //                mission = mission.Where(m => m.CityId == city + 5500).ToList();
            //                ci++;
            //            }
            //            //missionfound = newmission.Where(m => m.CityId == city).ToList();
            //            //mission.AddRange(missionfound);
            //            //finalmissionfound.AddRange(missionfound);
            //            foundmission = foundmission.Where(m => m.CityId == city + 5500).ToList();
            //            foundmission = allmission.Where(m => m.CityId == city).ToList();
            //            mission.AddRange(foundmission);
            //            finalmissionfound.AddRange(foundmission);
            //        }
            //        ViewBag.SearchCity = city;
            //        if (ViewBag.SearchCity != null)
            //        {
            //            var A = _db.Cities.Where(u => u.CityId == city).ToList();
            //            if (ci1 == 0)
            //            {
            //                Cityy = _db.Cities.Where(m => m.CityId == city + 5000).ToList();
            //                ci1++;
            //            }
            //            //ViewBag.SearchCity = A.Name;
            //            Cityy.AddRange(A);
            //        }
            //        if (mission.Count() == 0)
            //        {
            //            return RedirectToAction("nomission", "Home", new { @id = id });
            //        }

            //    }
            //    ViewBag.SearchCity = Cityy;

            //}
            //if (fThemes != null && fThemes.Length > 0)
            //{
            //    foreach (var theme in fThemes)
            //    {
            //        if (fCountries.Length > 0 && fCitys.Length > 0)
            //        {
            //            if (th == 0)
            //            {
            //                mission = mission.Where(m => m.ThemeId == theme + 2500).ToList();
            //                finalmissionfound2 = finalmissionfound;
            //                finalmissionfound = finalmissionfound.Where(m => m.ThemeId == theme + 2500).ToList();
            //                th++;

            //            }
            //            foundmission = foundmission.Where(m => m.ThemeId == theme + 2500).ToList();

            //            foundmission = finalmissionfound2.Where(m => m.ThemeId == theme + 2500).ToList();
            //            finalmissionfound.AddRange(foundmission);
            //        }
            //        else
            //        {
            //            if (th == 0)
            //            {
            //                mission = mission.Where(m => m.ThemeId == theme + 2500).ToList();
            //                th++;
            //            }
            //            //missionfound = newmission.Where(m => m.ThemeId == theme).ToList();
            //            //mission.AddRange(missionfound);
            //            //finalmissionfound.AddRange(missionfound);
            //            foundmission = foundmission.Where(m => m.ThemeId == theme + 2500).ToList();
            //            foundmission = allmission.Where(m => m.ThemeId == theme).ToList();
            //            mission.AddRange(foundmission);
            //            finalmissionfound.AddRange(foundmission);
            //        }
            //        ViewBag.SearchTheme = theme;
            //        if (ViewBag.SearchTheme != null)
            //        {
            //            var A = _db.MissionThemes.Where(u => u.MissionThemeId == theme).ToList();
            //            if (th1 == 0)
            //            {
            //                Themes = _db.MissionThemes.Where(u => u.MissionThemeId == theme + 5000).ToList();
            //                th1++;
            //            }

            //            Themes.AddRange(A);
            //            //ViewBag.SearchTheme = A.Title;
            //        }

            //        if (mission.Count() == 0)
            //        {
            //            return RedirectToAction("nomission", "Home", new { @id = id });
            //        }

            //    }
            //    @ViewBag.SearchTheme = Themes;


            //}
            //switch (sortorder)
            //{

            //    case "Newest":
            //        mission = mission.OrderByDescending(m => m.StartDate).ToList();
            //        ViewBag.sortby = "Newest";
            //        break;
            //    case "Oldest":
            //        mission = mission.OrderBy(m => m.StartDate).ToList();
            //        ViewBag.sortby = "Oldest";
            //        break;
            //    case "Lowest seats":
            //        mission = mission.OrderBy(m => int.Parse(m.Availability)).ToList();
            //        break;
            //    case "Highest seats":
            //        mission = mission.OrderByDescending(m => int.Parse(m.Availability)).ToList();
            //        break;
            //    case "Registration deadline":
            //        mission = mission.OrderBy(m => m.EndDate).ToList();
            //        break;


            //}
            ViewBag.Country = _Idb.CountryList();
            ViewBag.Themes = _Idb.ThemeList();
            ViewBag.Cityy=_Idb.CityList();
        

              //List<Mission> allmission = _db.Missions.ToList();
            List<Mission> allmission = _Idb.MissionsList();
               List<VolunteeringVM> mission= new List<VolunteeringVM>();
                foreach (Mission item in allmission)
                {
                    var City = _Idb.CityList().FirstOrDefault(u => u.CityId == item.CityId);
                      var Theme = _Idb.ThemeList().FirstOrDefault(u => u.MissionThemeId == item.ThemeId);
                    var goalobj = _Idb.GoalsList().FirstOrDefault(m => m.MissionId == item.MissionId);
                    string[] Startdate1 = item.StartDate.ToString().Split(" ");
                    string[] Enddate2 = item.EndDate.ToString().Split(" ");
                    var favrioute = (id != null) ? _Idb.favoriteMissions().Any(u => u.UserId == id && u.MissionId == item.MissionId) : false;
                var Applybtn = (id != null) ? _Idb.missionApplications().Any(u => u.MissionId == item.MissionId && u.UserId == id) : false;
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
                    });
                }
                ViewBag.AllMissions = mission;


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

                //pagination
                int pageSize = 9;
                int skip = (pageIndex ?? 0) * pageSize;

                var Missions = mission.Skip(skip).Take(pageSize).ToList();

                //if (mission.Count() == 0)
                //{
                //    return RedirectToAction("NoMissionFound", new { });
                //}
                int totalMissions = mission.Count();
                ViewBag.TotalMission = totalMissions;
                ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
                ViewBag.CurrentPage = pageIndex ?? 0;

                UriBuilder uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host);
                if (Request.Host.Port.HasValue)
                {
                    uriBuilder.Port = Request.Host.Port.Value;
                }
                uriBuilder.Path = Request.Path;

                // Remove the query parameter you want to exclude
                var query = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                query.Remove("pageIndex");
                uriBuilder.Query = query.ToString();



                ViewBag.currentUrl = uriBuilder.ToString();

                
                return View(Missions);
            //}



        }

    }
}
