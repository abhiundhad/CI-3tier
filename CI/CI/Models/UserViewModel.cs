using CI_Entity.Models;

namespace CI.Models
{
    public class UserViewModel
    {
        //public User User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? MissionId { get; set; }
        public int? StoryId { get; set; }
    }
}
