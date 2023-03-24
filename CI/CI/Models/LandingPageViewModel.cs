using CI_Entity.Models;

namespace CI.Models
{
    public class LandingPageViewModel
    {
        public List<Mission> mission { get; set; }
        public List<City> cities { get; set; }
        public List<Country> countries { get; set; }
        public List<MissionTheme> missionThemes { get; set; }
        public List<Skill> skills { get; set; }
        public List<MissionMedium> missionMedia { get; set; }
        public List<FavoriteMission> favoriteMissions { get; set; }
        public List<User> Users { get; set; }

        public List<GoalMission> goalMissions { get; set; }
        public List<MissionSkill> missionSkills { get; set; }

        public List<Comment> comments { get; set; }

    
        public List<User> users { get; set; }
      
        public List<Mission> relatedMission { get; set; }
        public List<MissionRating> missionRatings { get; set; }
  

        public List<Timesheet> timesheets { get; set; }
        public List<MissionDocument> missionDocuments { get; set; }

        public List<Story> stories { get; set; }
        public Mission singleMission { get; set; }

        public int avgrating { get; set; }
        public long missionId { get; set; }
        public long userId { get; set; }
        public Story storydetails { get; set; }
        public List<MissionApplication> application { get; set; }

    }
}
