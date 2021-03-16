using System.ComponentModel.DataAnnotations;

namespace MileAway.Models
{
    public class Users
    {
        public int USER_ID { get; set; }

        [Required]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be at least {2} characters long.", MinimumLength = 6)]
        public string PASSWORD { get; set; }

        [Required]
        [Compare("PASSWORD", ErrorMessage = "The passwords do not match with eachother.")]
        public string CONFIRMPASSWORD { get; set; }

        [Required]
        public string FIRSTNAME { get; set; }

        [Required]
        public string LASTNAME { get; set; }

        public string USER_IMAGE { get; set; }
    }
}
