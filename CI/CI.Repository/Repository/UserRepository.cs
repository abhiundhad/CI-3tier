using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CiPlatformContext _db;

        public UserRepository(CiPlatformContext db)
        {
            _db = db;


        }
        public User UserExist(string Email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == Email);
        }
        public User Login(string Email, string Password)
        {
            return _db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

        }
        public PasswordReset PasswordResets(string Email, string Token)
        {
            return _db.PasswordResets.FirstOrDefault(u => u.Email == Email && u.Token == Token);
        }

        public List<Country> CountryList()
        {
            return _db.Countries.ToList();
        }
        public List<City> CityList()
        {
            return _db.Cities.ToList();
        }
        public List<MissionTheme> ThemeList()
        {
            return _db.MissionThemes.ToList();
        }
        public List<Mission> MissionsList()
        {
            return _db.Missions.ToList();
        }
        public List<GoalMission> GoalsList()
        {
            return _db.GoalMissions.ToList();
        }
        public List<FavoriteMission> favoriteMissions()
        {
            return _db.FavoriteMissions.ToList();
        }
        public List<MissionApplication> missionApplications()
        {
            return _db.MissionApplications.ToList();
        }
        public List<MissionRating> missionRatingList()
        {
            return _db.MissionRatings.ToList();

        }
        public List<User> alluser()
        {
            return _db.Users.ToList();
        }
        public MissionRating RatingExist(long Id, long missionId)
        {
            return _db.MissionRatings.FirstOrDefault(u => u.MissionId == missionId && u.UserId == Id);

        }
        public void ApplyMission(long missionid, long id)
        {
            var Applied = new MissionApplication();
            {
                Applied.MissionId = missionid;
                Applied.UserId = id;
                Applied.ApprovalStatus = "1";
                Applied.AppliedAt = DateTime.Now;
            };
            _db.MissionApplications.Add(Applied);
            _db.SaveChanges();
        }
        public Comment comment(long id, long missionid, string comttext)
        {

            var newcmt = new Comment();
            {
                newcmt.UserId = id;
                newcmt.MissionId = missionid;
                newcmt.MissionTxt = comttext;

            };

            _db.Comments.Add(newcmt);

            _db.SaveChanges();
            return newcmt;
        }

        public bool FavMissByUserMissID(long missionid, long id)
        {
            return _db.FavoriteMissions.Any(u => u.UserId == id && u.MissionId == missionid);
        }
        public List<Mission> RelatedMission(long themeid, long missionid)
        {
            return _db.Missions.Where(m => m.ThemeId == themeid && m.MissionId != missionid).ToList();
        }
        public List<User> Adduser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return null;
        }

        public void addstory(long MissionId, string title, DateTime date, string discription, long id, long storyid)
        {
            if (storyid != 0)
            {
                var updatestory = _db.Stories.FirstOrDefault(x => x.StoryId == storyid);

                updatestory.MissionId = MissionId;
                updatestory.Title = title;
                updatestory.Description = discription;
                updatestory.Status = "1";
                updatestory.CreatedAt = date;
                updatestory.UpdatedAt= DateTime.Now;
                updatestory.UserId = id;
                _db.Update(updatestory);

            }
            else
            {
                var newstory = new Story();
                newstory.MissionId = MissionId;
                newstory.Title = title;
                newstory.Description = discription;
                newstory.Status = "1";
                newstory.CreatedAt = date;
                newstory.UserId = id;
                _db.Add(newstory);
            }
            _db.SaveChanges();
        }
        public void adddraftstory(long MissionId, string title, DateTime date, string discription, long id,long storyid)
            
        {
            if (storyid != 0)
            {
                 var updatedraftstory= _db.Stories.FirstOrDefault(x=>x.StoryId==storyid);
                updatedraftstory.MissionId = MissionId;
                updatedraftstory.Title = title;
                updatedraftstory.Description = discription;
                updatedraftstory.Status = "draft";
                updatedraftstory.CreatedAt = date;
                updatedraftstory.UpdatedAt = DateTime.Now;
                updatedraftstory.UserId = id;
                _db.Update(updatedraftstory);


            }
            else
            {
                var newstory = new Story();
                newstory.MissionId = MissionId;
                newstory.Title = title;
                newstory.Description = discription;
                newstory.Status = "draft";
                newstory.CreatedAt = date;
                newstory.UserId = id;
                _db.Add(newstory);
            }
         
            _db.SaveChanges();
        }
        public List<MissionMedium> MissionMediaList()
        {
            return _db.MissionMedia.ToList();
        }
        public List<PasswordReset> storepasswordResets(string Email, string Token)
        {
            var passwordReset = new PasswordReset
            {
                Email = Email,
                Token = Token
            };
            _db.PasswordResets.Add(passwordReset);
            _db.SaveChanges();
            return null;
        }
        public List<PasswordReset> Updateremovepassword(string Email, string Token, string password)
        {
            // Find the user by email
            //var user = _db.Users.FirstOrDefault(u => u.Email == rsp.Email);
            var user = _db.Users.FirstOrDefault(m => m.Email == Email);



            // Find the password reset record by email and token
            //var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == rsp.Email && pr.Token == rsp.Token);
            //var passwordReset = _db.PasswordResets.FirstOrDefault(m=>m.Email==Email&& m.Token==Token );
            var passwordreset = _db.PasswordResets.FirstOrDefault(m => m.Email == Email && m.Token == Token);

            // Update the user's password

            user.Password = password;
            _db.SaveChanges();

            // Remove the password reset record from the database
            _db.PasswordResets.Remove(passwordreset);
            _db.SaveChanges();
            return null;
        }

        public void addstoryMedia(long MissionId, string mediatype, string mediapath, long id)
        {
            var story = _db.Stories.OrderBy(s => s.CreatedAt).Where(s => s.MissionId == MissionId && s.UserId == id).FirstOrDefault();
            StoryMedium st = new StoryMedium();
            st.StoryId = story.StoryId;
            st.StoryType = mediatype;
            st.StoryPath = mediapath;
            _db.Add(st);
            _db.SaveChanges();
        }
        public List<StoryMedium> storyMedia()
        {
            return _db.StoryMedia.ToList();
        }
        public List<Story> StoryList()
        {
            return _db.Stories.ToList();
        }
    }
}
