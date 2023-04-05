using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using CI.Repository.Interface;
using System.Reflection;
//using Microsoft.SqlServer.Management.Smo;

namespace CI.Controllers
{
    public class VolunteeringController : Controller
    {
        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;
        public VolunteeringController(CiPlatformContext db, IUserRepository Idb)
        {
            _db = db;
            _Idb = Idb;
        }
        #region AppliedMission
        [HttpPost]
        public async Task<IActionResult> AppliedMission(long missionid, long id)
        {
            _Idb.ApplyMission(missionid, id);
            //return Json(new { success = true });
            return RedirectToAction("Volunteering", "Volunteering", new { id = id, missionid = missionid });
        }
        #endregion
        #region addComment
        [HttpPost]
        public async Task<IActionResult> addComment(long id, long missionid, string comttext)
        {
            _Idb.comment(id, missionid, comttext);
            return RedirectToAction("Volunteering", new { id = id, missionid = missionid });



        }
        #endregion
        #region sendRecomndetion of Mission
        [HttpPost]
        public async Task<IActionResult> sendRecom(long missionid,  string[] Email)
        {
            var sessionUserId = HttpContext.Session.GetString("userID");
            var ID= int.Parse(sessionUserId);
            foreach (var email in Email)
            {
                var user = _Idb.UserExist(email);
                var sender = _db.Users.FirstOrDefault(m => m.UserId == ID);
                var sendername = sender.FirstName + $" " + sender.LastName;
                var userid = user.UserId;
                var resetLink = Url.Action("Volunteering", "Volunteering", new { missionid = missionid, id = userid }, Request.Scheme);
                // Send email to user with reset password link
                // ...
                var fromAddress = new MailAddress("communityempowermentportal@gmail.com", "Community Empowerment Portal");
                var toAddress = new MailAddress(email);
                var subject = "Recomanded Mission Mail";
                var body = $"Hi,<br /><br /> you are recomanded a mission by {sendername} Please click on the following link to see recomanded mission detail:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("communityempowermentportal@gmail.com", "bkdkuisqaxfsjgfp"),
                    EnableSsl = true
                };
                smtpClient.Send(message);

            }
            return Json(new { success = true });
        }
        #endregion
        #region Addrating
        [HttpPost]
        public async Task<IActionResult> Addrating(string rating, long Id, long missionId)
        {
            var ratingExists = _Idb.missionRatingList().FirstOrDefault(u => u.MissionId == missionId && u.UserId == Id);
            if (ratingExists != null)
            {
                ratingExists.Rating = rating;
                _db.Update(ratingExists);
                _db.SaveChanges();
                //return Json(new { success = true, ratingExists, isRated = true });
            }
            else
            {
                var ratingele = new MissionRating();
                ratingele.Rating = rating;
                ratingele.UserId = Id;
                ratingele.MissionId = missionId;
                _db.Add(ratingele);
                _db.SaveChanges();
                //return Json(new { success = true, ratingele, isRated = true });
            }
            return RedirectToAction("Volunteering", new { id = Id, missionid = missionId });
        }
        #endregion
        #region AddtoFavrioate
        [HttpPost]
        public async Task<IActionResult> Addfav(long Id, long missionId)
        {
            FavoriteMission fav = await _db.FavoriteMissions.FirstOrDefaultAsync(fm => fm.UserId == Id && fm.MissionId == missionId);

            if (fav != null)
            {

                _db.Remove(fav);

                _db.SaveChanges();
                return Json(new { success = true, favmission = "1" });
            }
            else
            {
                var ratingele = new FavoriteMission();

                ratingele.UserId = Id;
                ratingele.MissionId = missionId;
                _db.AddAsync(ratingele);
                _db.SaveChanges();
                return Json(new { success = true, favmission = "0" });
            }
        }
        #endregion
        #region Volunteering
        public IActionResult Volunteering(long id, long missionid, int pg)

        {
            var sessionUserId = HttpContext.Session.GetString("userID");
            ViewBag.UserId = Convert.ToInt64(sessionUserId);
            if (ViewBag.UserId != id)
            {
                TempData["Sessonerrormsg"] = "please login again";
                return RedirectToAction("Login", "Login", new { missionid = missionid });
            }
            else
            {

                var volmission = _Idb.MissionsList().FirstOrDefault(m => m.MissionId == missionid);
                var theme = _Idb.ThemeList().FirstOrDefault(m => m.MissionThemeId == volmission.ThemeId);
                var City = _Idb.CityList().FirstOrDefault(m => m.CityId == volmission.CityId);
                var themeobjective = _Idb.GoalsList().FirstOrDefault(m => m.MissionId == missionid);
                string[] Startdate = volmission.StartDate.ToString().Split(" ");
                string[] Enddate = volmission.EndDate.ToString().Split(" ");
                var favrioute = (id != null) ? _Idb.favoriteMissions().Any(u => u.UserId == Convert.ToInt64(sessionUserId) && u.MissionId == volmission.MissionId) : false;
                var Applybtn = (id != null) ? _Idb.missionApplications().Any(u => u.MissionId == volmission.MissionId && u.UserId == Convert.ToInt64(sessionUserId)) : false;
                int Applycunt = _Idb.missionApplications().Where(m => m.MissionId == volmission.MissionId).ToList().Count();
                var givrats = _Idb.missionRatingList().FirstOrDefault(u => u.MissionId == volmission.MissionId && u.UserId == Convert.ToInt64(sessionUserId));
                int seatleft = Convert.ToInt32(volmission.Availability) - Applycunt;
                var rat = _Idb.missionRatingList().Where(u => u.MissionId == volmission.MissionId).ToList();
                ViewBag.ratusercouny = rat.Count();
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


                VolunteeringVM volunteeringVM = new VolunteeringVM();
                volunteeringVM.MissionId = missionid;
                volunteeringVM.Title = volmission.Title;
                volunteeringVM.ShortDescription = volmission.ShortDescription;
                volunteeringVM.OrganizationName = volmission.OrganizationName;
                volunteeringVM.Description = volmission.Description;
                volunteeringVM.OrganizationDetail = volmission.OrganizationDetail;
                volunteeringVM.Availability = seatleft;
                volunteeringVM.MissionType = volmission.MissionType;
                volunteeringVM.Cityname = City.Name;
                volunteeringVM.Themename = theme.Title;
                volunteeringVM.EndDate = Enddate[0];
                volunteeringVM.StartDate = Startdate[0];
                volunteeringVM.GoalObjectiveText = themeobjective != null ? themeobjective.GoalObjectiveText : "Null";
                volunteeringVM.isFavriout = favrioute;
                volunteeringVM.isApplied = Applybtn;
                volunteeringVM.Givenrating = givrats != null ? Convert.ToInt64(givrats.Rating) : 0;
                volunteeringVM.AvrageRating = finalrat;
                volunteeringVM.UserId = Convert.ToInt64(sessionUserId);


                ViewBag.Missiondetail = volunteeringVM;

                List<VolunteeringVM> relatedlist = new List<VolunteeringVM>();
                var relatedmission = _Idb.RelatedMission(volmission.ThemeId, missionid);
                foreach (var item in relatedmission.Take(3))
                {

                    var relcity = _Idb.CityList().FirstOrDefault(m => m.CityId == item.CityId);
                    var reltheme = _Idb.ThemeList().FirstOrDefault(m => m.MissionThemeId == item.ThemeId);
                    var relgoalobj = _Idb.GoalsList().FirstOrDefault(m => m.MissionId == item.MissionId);
                    string[] Startdate1 = item.StartDate.ToString().Split(" ");
                    string[] Enddate2 = item.EndDate.ToString().Split(" ");
                    var relfavrioute = (id != null) ? _Idb.favoriteMissions().Any(u => u.UserId == Convert.ToInt64(sessionUserId) && u.MissionId == item.MissionId) : false;
                    var relApplybtn = (id != null) ? _Idb.missionApplications().Any(u => u.MissionId == item.MissionId && u.UserId == Convert.ToInt64(sessionUserId)) : false;
                    int Applycunts = _Idb.missionApplications().Where(m => m.MissionId == item.MissionId).ToList().Count();
                    int relseatleft = Convert.ToInt32(item.Availability) - Applycunts;
                    var relrat = _Idb.missionRatingList().Where(u => u.MissionId == item.MissionId).ToList();
                    var missionpath = _Idb.MissionMediaList().FirstOrDefault(m => m.MissionId == item.MissionId);
                    var c = relrat.Count();
                    int relfinalrat = 0;
                    if (relrat.Count > 0)
                    {
                        int rating = 0;
                        foreach (var items in relrat)
                        {

                            rating = rating + int.Parse(items.Rating);

                        }
                        relfinalrat = rating / c;

                    }



                    relatedlist.Add(new VolunteeringVM
                    {
                        MissionId = item.MissionId,
                        Cityname = relcity.Name,
                        Themename = reltheme.Title,
                        Title = item.Title,
                        ShortDescription = item.ShortDescription,
                        StartDate = Startdate1[0],
                        EndDate = Enddate2[0],
                        Availability = relseatleft,
                        OrganizationName = item.OrganizationName,
                        GoalObjectiveText = relgoalobj != null ? relgoalobj.GoalObjectiveText : "Null",
                        MissionType = item.MissionType,
                        isFavriout = relfavrioute,
                        isApplied = relApplybtn,
                        AvrageRating = relfinalrat,
                        missionmediapath = missionpath != null ? missionpath.MediaPath : "",
                    }
                    );

                }
                ViewBag.relatedmission = relatedlist.Take(3);
                ViewBag.relatedmissioncount = relatedlist.Count();


                List<VolunteeringVM> recentvolunteredlist = new List<VolunteeringVM>();
                //var recentvolunttered = from U in CID.Users join MA in CiMainContext.MissionApplications on U.UserId equals MA.UserId where MA.MissionId == mission.MissionId select U;
                var recentvoluntered = from U in _db.Users join MA in _db.MissionApplications on U.UserId equals MA.UserId where MA.MissionId == missionid select U;
                foreach (var item in recentvoluntered)
                {


                    recentvolunteredlist.Add(new VolunteeringVM
                    {
                        username = item.FirstName,
                        Useravtar = item.Avatar != null ? item.Avatar : "",

                    });

                }
                var rcentvolunteering = recentvolunteredlist;

                const int pageSize = 6;
                if (pg < 1)
                {
                    pg = 1;
                }

                int missionCount = rcentvolunteering.Count();

                var PaginationModel = new PaginationModel(missionCount, pg, pageSize);

                int missionSkip = (pg - 1) * pageSize;
                ViewBag.Pagination = PaginationModel;

                var FinalMissions = rcentvolunteering.Skip(missionSkip).Take(PaginationModel.PageSize).ToList();


                ViewBag.recentvolunteered = FinalMissions;
                int totalCount = rcentvolunteering.Count();
                ViewBag.totalcount = totalCount;

                List<User> alluser= _db.Users.ToList();

                //List<User> alluser = _db.Users.ToList();
                List<VolunteeringVM> allavailuser = new List<VolunteeringVM>();
                foreach (var all in alluser)
                {
                    allavailuser.Add(new VolunteeringVM
                    {
                        username = all.FirstName,
                        lastname = all.LastName,
                        userEmail = all.Email,
                        UserId = all.UserId,
                        Useravtar = all.Avatar != null ? all.Avatar : "",
                    });

                }
                ViewBag.allavailuser = allavailuser;


                List<VolunteeringVM> misComment = new List<VolunteeringVM>();
                var missioncomment = _db.Comments.Where(c => c.MissionId == missionid).ToList();
                foreach (var comment in missioncomment)
                {
                    var cmtuser = _db.Users.FirstOrDefault(u => u.UserId == comment.UserId);
                    misComment.Add(new VolunteeringVM

                    {

                        Commenttext = comment.MissionTxt,
                        username = cmtuser.FirstName,
                        lastname = cmtuser.LastName,
                        CreatedDate = comment.CreatedAt,
                        Day = comment.CreatedAt.Day.ToString(),
                        Useravtar = cmtuser.Avatar != null ? cmtuser.Avatar : "",
                        UserId = cmtuser.UserId,
                        Commentid = comment.CommentId,

                    });
                }

                misComment.Reverse();
                ViewBag.missioncomment = misComment.OrderByDescending(m => m.CreatedDate).ToList();


                return View(new { sucess = true });
            }

        }
        #endregion
        public IActionResult cmtdelete(long cmtid, long id, long missionid)

        {
            _Idb.cmtdelete(cmtid);
            return RedirectToAction("Volunteering", "Volunteering", new { id = id, missionid = missionid });
        }

        //public IActionResult Volunteeringtimesheet()
        //{
        //    var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));

        //    var ShareStoryData = new TimesheetViewModel();
        //    ShareStoryData.Missions = _Idb.MissionsList();
        //    ShareStoryData.MissionApplications = _Idb.missionApplications().Where(m => m.UserId == userId).ToList();
        //    ShareStoryData.timesheetslist=_Idb.TimesheetList().Where(x=>x.UserId==userId).ToList();



        //    return View(ShareStoryData);
        //}
        //#region AddTimeMIssionTimesheet
        //[HttpPost]
        //public IActionResult AddTimeMIssionTimesheet(TimesheetViewModel timesheet)
        //{
        //    var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));
        //    if (timesheet != null)
        //    {
        //        _Idb.AddTimeMIssionTimesheetdata(timesheet.missionId, userId, timesheet.hours, timesheet.minutes, timesheet.action, timesheet.date, timesheet.message);
        //    }
        //    return RedirectToAction("Volunteeringtimesheet" , "Volunteering" , new {  Id = userId });
        //}
        //#endregion
        //#region deletedata
        //public IActionResult deletedata(long timesheetid)
        //{
        //    var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));
        //    _Idb.deletedatatimesheet(timesheetid);
        //    return RedirectToAction("Volunteeringtimesheet", "Volunteering", new { Id = userId });
        //}
        //#endregion
        public IActionResult VolunteeringTimeSheet()
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));
            TimesheetViewModel timeVm = new TimesheetViewModel();

            timeVm.missions = _Idb.MissionsList();
            timeVm.missionapplication = _Idb.missionApplications().Where(m => m.UserId == userId).ToList();
            var lists= _Idb.TimesheetList().Where(t => t.UserId == userId).ToList();
            timeVm.timesheet = lists.OrderByDescending(x=>x.CreatedAt).ToList();



            return View(timeVm);
        }
        [HttpPost]
        public async Task<IActionResult> addTimesheet(TimesheetViewModel model)
        {

            var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));
           
            //long storyid = model.storyId;
            _Idb.AddTimeMIssionTimesheetdata(model.missionId, userId, model.hour, model.minute, model.date, model.message, model.action, model.hiddenInput);





            return RedirectToAction("Volunteeringtimesheet", "Volunteering");


        }
        [HttpPost]
        public async Task<IActionResult> deletetimesheet(long timesheetid)
        {

            var userId = Convert.ToInt64(HttpContext.Session.GetString("userID"));
            //long storyid = model.storyId;
            _Idb.deletedatatimesheet(timesheetid);





            return RedirectToAction("Volunteeringtimesheet", "Volunteering");


        }

        [HttpPost]
        public async Task<IActionResult> editsheet(long timesheetid)
        {

            int? userid = HttpContext.Session.GetInt32("userIDforfavmission");
            long id = Convert.ToInt64(userid);
            //long storyid = model.storyId;
            var timesheet = _Idb.TimesheetList().Where(t => t.TimesheetId == timesheetid).FirstOrDefault();

            return Json(new { success = true, timesheet = timesheet });


        }

    }
}
