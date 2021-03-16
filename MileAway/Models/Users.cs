using System.ComponentModel.DataAnnotations;

namespace MileAway.Models
{
    public class Users
    {
        public int User_Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string User_Image { get; set; }
    }
}
