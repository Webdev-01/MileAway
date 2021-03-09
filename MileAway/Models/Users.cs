using System.ComponentModel.DataAnnotations;

namespace MileAway.Models
{
    public class Users
    {
        public int USER_ID { get; set; }

        public string EMAIL { get; set; }

        public string USERNAME { get; set; }

        public string PASSWORD { get; set; }

        public string FIRSTNAME { get; set; }

        public string LASTNAME { get; set; }

        public string USER_IMAGE { get; set; }
    }
}
