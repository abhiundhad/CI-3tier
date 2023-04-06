using CI_Entity.Models;
using System.ComponentModel.DataAnnotations;

namespace CI.Models
{
    public class UserprofileViewModel
    {

        //[Required]
        //public string OldPassword { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = "Password should contain atleast 8 charachter")]
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password should contain atleast one Capital letter , one small case letter, one Digit and one special symbol")]
        //public string NewPassword { get; set; }

        //[Required]
        //[DataType(DataType.Password)]

        //public string NewConfirmPassword { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string employeeid { get; set; }

        public string email { get; set; }
        public string oldpassword { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string whyivolunteered { get; set; }

        public string myprofile { get; set; }
        public string title { get; set; }
        public string manager { get; set; }
        public string department { get; set; }
        public string availability { get; set; }

        public string linkedinurl { get; set; }

        public string avatar { get; set; }
        public IFormFile files { get; set; }
        public long? cityid { get; set; }
        public long? countryid { get; set; }

        public List<Skill> allskills { get; set; }

    }
}
