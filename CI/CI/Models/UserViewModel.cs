using CI_Entity.Models;
using System.ComponentModel.DataAnnotations;

namespace CI.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? MissionId { get; set; }
        public int? StoryId { get; set; }
    }
}
