﻿using CI_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IUserRepository
    {
        public User UserExist(string Email);
        public User Login(string Email ,string Password);
        public PasswordReset PasswordResets(string Email, string Token);
     
        public List<Country> CountryList();
        public List<City> CityList();
        public List<MissionTheme> ThemeList();    
        public List<Mission> MissionsList();
        public List<GoalMission> GoalsList();
        public List<FavoriteMission> favoriteMissions();
        public List<MissionApplication> missionApplications();
        public List<MissionRating> missionRatingList();
        public List<User> alluser();
        public MissionRating RatingExist(long Id, long missionId);
        public void ApplyMission(long missionid,long id);
        public Comment comment(long id, long missionid, string comttext);
        public bool FavMissByUserMissID(long missionid,long id);
        public List<Mission> RelatedMission(long themeid , long missionid);
        //public void Adduser(string user);
        public List<User> Adduser(User user);
        public void addstory(long MissionId, string title, DateTime date, string discription, long id);
        public List<MissionMedium> MissionMediaList( );

    }
}
