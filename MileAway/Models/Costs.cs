using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MileAway.Models
{
    public class Costs
    {
        public int Cost_Id { get; set; }
        public int Typecost_Id { get; set; }
        public string License { get; set; }
        [Required]
        public double Cost { get; set; }
        public double Fuel_Quantity { get; set; }
        [Required]
        public DateTime Date_Of_Cost { get; set; }
        public string Invoice_Doc { get; set; }
    }
}
