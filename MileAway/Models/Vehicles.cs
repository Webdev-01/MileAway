using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MileAway.Models
{
    public class Vehicles
    {
        public int Vehicle_Id { get; set; }
        public int User_Id { get; set; }
        public string License { get; set; }
        public string Brand_Name { get; set; }
        public string Model_Name { get; set; }
        public int Manufacturing_Year { get; set; }
        public string Color { get; set; }
        public int Mileage_Km { get; set; }
        public string Vehicle_Image { get; set; }
    }
}
