using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace CI.Controllers
{
    public class StoryListingController : Controller
    {
        private readonly CiPlatformContext _db;

        public StoryListingController(CiPlatformContext db)
        {
            _db = db;


        }
        public IActionResult StoryListing(long id, int? pageIndex)

        {
            var userId = HttpContext.Session.GetString("userID");

            ViewBag.UserId = int.Parse(userId);
            List<Story> storylist = _db.Stories.ToList(); 
           List<VolunteeringVM> StoryList = new List<VolunteeringVM>();
            foreach(var story in storylist) 
            { var storyuser=_db.Users.FirstOrDefault(x => x.UserId == story.UserId);    
                StoryList.Add(new VolunteeringVM
                {
                    StoryId=story.StoryId,
                    MissionId=story.MissionId,
                    UserId=story.UserId,
                    StoryTitle=story.Title,
                     ShortDescription=story.Description,
                     username=storyuser.FirstName,
                     lastname=storyuser.LastName,

                });

            }
            ViewBag.StoryList = StoryList;

            //pagination
            int pageSize = 9;
            int skip = (pageIndex ?? 0) * pageSize;

            var Storycards = StoryList.Skip(skip).Take(pageSize).ToList();

            //if (mission.Count() == 0)
            //{
            //    return RedirectToAction("NoMissionFound", new { });
            //}
            int totalMissions = StoryList.Count();
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

            return View(Storycards);
        }

        public IActionResult StoryDetail(long id,int StoryId)
        {
            var userId = HttpContext.Session.GetString("userID");

            ViewBag.UserId = int.Parse(userId);
            if (ViewBag.UserId != id)
            {
                TempData["Sessonerrormsg"] = "please login again";
                return RedirectToAction("Login", "Login", new { StoryId = StoryId });
            }
            else
            {
                var storydetail = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
                List<VolunteeringVM> StoryDetail = new List<VolunteeringVM>();
                if (storydetail != null)
                {
                    var userdetail = _db.Users.FirstOrDefault(s => s.UserId == storydetail.UserId);
                    StoryDetail.Add(new VolunteeringVM
                    {
                        username = userdetail.FirstName,
                        lastname = userdetail.LastName,
                        StoryTitle = storydetail.Title,
                        StoryDescription = storydetail.Description,
                        MissionId = storydetail.MissionId,
                        StoryId = storydetail.StoryId,


                    });
                }
                ViewBag.StoryDetail = StoryDetail;

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
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> sendRecom(long Id, long storyid, string[] Email)
        {
            foreach (var email in Email)
            {
                var user = _db.Users.FirstOrDefault(m => m.Email == email);
                var sender = _db.Users.FirstOrDefault(m => m.UserId == Id);
                var sendername = sender.FirstName + $" " + sender.LastName;
                var userid = user.UserId;
                var resetLink = Url.Action("StoryDetail", "StoryListing", new { storyid = storyid, id = userid }, Request.Scheme);
                // Send email to user with reset password link
                // ...
                var fromAddress = new MailAddress("communityempowermentportal@gmail.com", "Community Empowerment Portal");
                var toAddress = new MailAddress(email);
                var subject = "Recomanded Story  Mail";
                var body = $"Hi,<br /><br /> you are recomanded a Story by {sendername} Please click on the following link to see recomanded mission detail:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
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
    }
}
