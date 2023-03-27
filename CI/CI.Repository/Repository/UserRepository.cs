using CI.Repository.Interface;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class UserRepository:IUserRepository
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
        public User Login(string Email,string Password) 
        {
            return _db.Users.FirstOrDefault(u => u.Email == Email&&u.Password==Password);

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
            return  _db.FavoriteMissions.Any(u => u.UserId == id && u.MissionId == missionid);
        }
        public List<Mission> RelatedMission(long themeid, long missionid)
        {
            return _db.Missions.Where(m=>m.ThemeId==themeid && m.MissionId!=missionid).ToList();
        }
        public List<User> Adduser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return null;
        }
    }
}
