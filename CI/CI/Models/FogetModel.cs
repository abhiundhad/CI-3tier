using System.ComponentModel.DataAnnotations;

namespace CI.Models
{
    public class FogetModel
    {
        [Required(ErrorMessage = "please enter password")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? Token { get; set; }

      

    }
}
