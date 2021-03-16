using System.ComponentModel.DataAnnotations;

namespace MileAway.Models
{
    public class Users
    {
        public int User_Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The passwords do not match with eachother.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string User_Image { get; set; }
    }
}
