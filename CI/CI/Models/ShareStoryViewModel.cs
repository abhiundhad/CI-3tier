using CI_Entity.Models;
//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;


namespace CI.Models
{
    public class ShareStoryViewModel
    {
        [Required( ErrorMessage = "Please Select Mission")]
        public long MissionId { get; set; }


        public List<Mission> Missions { get; set; }
        //public List<Story> Stories { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        //public List<Story> DraftStory { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select StoryTitle")]
        public string StoryTitle { get; set; }


        [Required(AllowEmptyStrings=false, ErrorMessage = "Please Write Story")]
        public string editor1 { get; set;}


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Date")]
        public DateTime date { get; set; }


        public long userID { get; set; }
        public long StoryID { get; set; }
        public List<IFormFile>? attachment { get; set; } 
    }
}
