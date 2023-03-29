using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web;
using CI.Repository.Interface;
using System.Reflection;

namespace CI.Controllers
{
    public class StoryListingController : Controller
    {
        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public StoryListingController(CiPlatformContext db, IUserRepository Idb, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _Idb = Idb;
            _hostingEnvironment = hostingEnvironment;
        }
        #region StoryListing
        public IActionResult StoryListing(long id, int? pageIndex, int pg)

        {
            var userId = HttpContext.Session.GetString("userID");

            ViewBag.UserId = int.Parse(userId);
            List<Story> storylist = _db.Stories.ToList();
            List<VolunteeringVM> StoryList = new List<VolunteeringVM>();
            foreach (var story in storylist)
            {
                var storyuser = _db.Users.FirstOrDefault(x => x.UserId == story.UserId);
                var missiontheme = _Idb.MissionsList().FirstOrDefault(m => m.MissionId == story.MissionId).ThemeId;
                var storytheme = _Idb.ThemeList().FirstOrDefault(m => m.MissionThemeId == missiontheme).Title;
                var storymedia = _Idb.storyMedia().FirstOrDefault(m=>m.StoryId==story.StoryId);
                StoryList.Add(new VolunteeringVM
                {
                    StoryId = story.StoryId,
                    MissionId = story.MissionId,
                    UserId = story.UserId,
                    StoryTitle = story.Title,
                    Themename = storytheme,
                    ShortDescription = HttpUtility.HtmlDecode(story.Description),
                    username = storyuser.FirstName,
                    lastname = storyuser.LastName,
                    Useravtar=storyuser.Avatar!=null? storyuser.Avatar:"",
                    storymediapath= storymedia !=null ? storymedia.StoryPath : "",

                });;

            }
            var Storys = StoryList.OrderByDescending(m => m.CreatedDate).ToList(); ;
            ViewBag.StoryList = StoryList;


            const int pageSize = 6;
            if (pg < 1)
            {
                pg = 1;
            }

            int missionCount = Storys.Count();

            var PaginationModel = new PaginationModel(missionCount, pg, pageSize);

            int missionSkip = (pg - 1) * pageSize;
            ViewBag.Pagination = PaginationModel;

            var Finalstorys = Storys.Skip(missionSkip).Take(PaginationModel.PageSize).ToList();



            int totalCount = Storys.Count();
            ViewBag.totalcount = totalCount;

            return View(Finalstorys);
        }
        #endregion

        #region StoryDetail
        public IActionResult StoryDetail(long id, int StoryId)
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
        #endregion

        #region SendRecomandation
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
        public IActionResult ShareStory(int id)
        {
            var ShareStoryData = new ShareStoryViewModel();
            ShareStoryData.Missions = _Idb.MissionsList();
            ShareStoryData.MissionApplications = _Idb.missionApplications().Where(m => m.UserId == id).ToList();

            return View(ShareStoryData);
        }
        #endregion


        #region AddStory
        [HttpPost]
        public IActionResult AddStory(ShareStoryViewModel model)
        {
            var id = HttpContext.Session.GetString("userID");
            long userid = Convert.ToInt64(id);

            _Idb.addstory(model.MissionId, model.StoryTitle, model.date, model.editor1, userid);
            foreach (var i in model.attachment)
            {
                if (i != null)
                {
                    string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\story");
                    string FileName = i.FileName;
                    string FilePath = Path.Combine(UploadsFolder, FileName);

                    using (var FileStream = new FileStream(FilePath, FileMode.Create))
                    {
                        i.CopyTo(FileStream);
                    }
                    var type = i.ContentType;
                    _Idb.addstoryMedia(model.MissionId, i.ContentType.Split("/")[0], FileName, userid);
                }

            }
            return RedirectToAction("ShareStory", "StoryListing");


        }
        #endregion

    }
}
