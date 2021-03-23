using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MileAway.Models
{
    public class Vehicles
    {
        [MaxLength(8), Required, RegularExpression("/[^A-Za-z0-9]+/g")]
        public string License { get; set; }
        public string Email { get; set; }
        [Required]
        public string Brand_Name { get; set; }
        [Required]
        public string Model_Name { get; set; }
        public int Manufacturing_Year { get; set; }
        public string FuelType { get; set; }
        public string Color { get; set; }
        public int Mileage_Km { get; set; }
        public string Vehicle_Image { get; set; }
        public double AvgCosts { get; set; }
    }
}
