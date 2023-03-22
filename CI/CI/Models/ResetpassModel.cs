using System.ComponentModel.DataAnnotations;

namespace CI.Models
{
    public class ResetpassModel
    {
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set;}
    }
}
