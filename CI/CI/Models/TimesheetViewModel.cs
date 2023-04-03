using CI_Entity.Models;
using Microsoft.Build.Framework;

namespace CI.Models
{
    public class TimesheetViewModel
    {
        [Required]
        public long missionId { get; set; }
        [Required]
        public int action { get; set; }
  
        public List<Mission> Missions { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        public List<Timesheet> timesheetslist { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int? Goal { get; set; }
        public DateTime date { get; set; }
        public string message { get; set; } 

    }
}
