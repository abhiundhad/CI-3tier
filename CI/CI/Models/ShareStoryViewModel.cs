using CI_Entity.Models;

namespace CI.Models
{
    public class ShareStoryViewModel
    {

        public long MissionId { get; set; }
        public List<Mission> Missions { get; set; }
        //public List<Story> Stories { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        //public List<Story> DraftStory { get; set; }
        public string StoryTitle { get; set; }

        public string editor1 { get; set;}
        public DateTime date { get; set; }
        public long userID { get; set; }
        public long StoryID { get; set; }
        public List<IFormFile> attachment { get; set; }
    }
}
