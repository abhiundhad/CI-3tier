using CI_Entity.Models;
using Microsoft.Build.Framework;

namespace CI.Models
{
    public class TimesheetViewModel
    {

            public long missionId { get; set; }
            public int? action { get; set; }
            public List<Mission> missions { get; set; }
            public List<MissionApplication> missionapplication { get; set; }
            public List<Timesheet> timesheet { get; set; }

            public DateTime date { get; set; }
            public long hiddenInput { get; set; }
            public int? hour { get; set; }
            public int? minute { get; set; }

            public string? message { get; set; }
            public string? title { get; set; }
    }
}


