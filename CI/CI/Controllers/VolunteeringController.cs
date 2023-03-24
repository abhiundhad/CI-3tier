using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using CI.Repository.Interface;

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
            return Json(new { success = true });
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
        public async Task<IActionResult> sendRecom(long Id, long missionid, string[] Email)
        {
            foreach (var email in Email)
            {
                var user = _Idb.UserExist(email);
                var sender = _db.Users.FirstOrDefault(m => m.UserId == Id);
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
            //MissionRating ratingExists = _Idb.RatingExist(missionId, Id);
            var ratingExists = _Idb.missionRatingList().FirstOrDefault(u => u.MissionId == missionId&&u.UserId==Id);
            if (ratingExists != null)
            {
                ratingExists.Rating = rating;
                _db.Update(ratingExists);
                _db.SaveChanges();
                return Json(new { success = true, ratingExists, isRated = true });
            }
            else
            {
                var ratingele = new MissionRating();
                ratingele.Rating = rating;
                ratingele.UserId = Id;
                ratingele.MissionId = missionId;
                _db.Add(ratingele);
                _db.SaveChanges();
                return Json(new { success = true, ratingele, isRated = true });
            }
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
        public IActionResult Volunteering(long id, long missionid)

        {
            var sessionUserId = HttpContext.Session.GetString("userID");
            ViewBag.UserId = int.Parse(sessionUserId);
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
                var givrat = _Idb.missionRatingList().FirstOrDefault(u => u.MissionId == volmission.MissionId&&u.UserId== Convert.ToInt64(sessionUserId));
                VolunteeringVM volunteeringVM = new VolunteeringVM();
                volunteeringVM.MissionId = missionid;
                volunteeringVM.Title = volmission.Title;
                volunteeringVM.ShortDescription = volmission.ShortDescription;
                volunteeringVM.OrganizationName = volmission.OrganizationName;
                volunteeringVM.Description = volmission.Description;
                volunteeringVM.OrganizationDetail = volmission.OrganizationDetail;
                volunteeringVM.Availability = volmission.Availability;
                volunteeringVM.MissionType = volmission.MissionType;
                volunteeringVM.Cityname = City.Name;
                volunteeringVM.Themename = theme.Title;
                volunteeringVM.EndDate = Enddate[0];
                volunteeringVM.StartDate = Startdate[0];
                volunteeringVM.GoalObjectiveText = themeobjective.GoalObjectiveText;
                volunteeringVM.isFavriout = favrioute;
                volunteeringVM.isApplied = Applybtn;
                volunteeringVM.Givenrating = Convert.ToInt64(givrat.Rating);
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



                    relatedlist.Add(new VolunteeringVM
                    {
                        MissionId = item.MissionId,
                        Cityname = relcity.Name,
                        Themename = reltheme.Title,
                        Title = item.Title,
                        ShortDescription = item.ShortDescription,
                        StartDate = Startdate1[0],
                        EndDate = Enddate2[0],
                        Availability = item.Availability,
                        OrganizationName = item.OrganizationName,
                        GoalObjectiveText = relgoalobj.GoalObjectiveText,
                        MissionType = item.MissionType,
                        isFavriout = relfavrioute,
                        isApplied = relApplybtn,
                        AvrageRating = finalrat,
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

                    });

                }
                ViewBag.recentvolunteered = recentvolunteredlist;

                List<User> alluser = _db.Users.ToList();
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

                    });
                }
                ViewBag.missioncomment = misComment;


                return View();
            }

        }
        #endregion
    }
}
