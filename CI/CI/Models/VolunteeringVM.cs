namespace CI.Models
{
    public class VolunteeringVM
    {
        public long MissionId { get; set; }

        public long CityId { get; set; }
        public string Cityname { get; set; }
        public string username { get; set; }
        public string lastname { get; set; }
        public long CountryId { get; set; } 
        public string Countryname { get; set; } 

        public long ThemeId { get; set; }
        public string Themename { get; set; }


        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
        public string MissionType { get; set; } = null!;
        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }
        public string? GoalObjectiveText { get; set; }
      

        public string GoalValue { get; set; } = null!;
        public int?  AvrageRating { get; set; }
        public bool isFavriout { get; set; }
        public bool isApplied { get; set; }
        public string userEmail { get; set; }
        public long? UserId { get; set; }
        public string Commenttext { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Day { get; set; }
        public long StoryId { get; set; }
        public string? StoryTitle { get; set; }
        public string? StoryDescription { get; set; }

        
    }
}
