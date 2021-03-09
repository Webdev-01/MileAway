using System.ComponentModel.DataAnnotations;

namespace MileAway.Models
{
    public class Users
    {
        public int USER_ID { get; set; }

        [Required]
        public string EMAIL { get; set; }

        [Required]
        public string PASSWORD { get; set; }

        [Compare("PASSWORD")]
        public string CONFIRMPASSWORD { get; set; }

        [Required]
        public string FIRSTNAME { get; set; }

        [Required]
        public string LASTNAME { get; set; }

        public string USER_IMAGE { get; set; }
    }
}
