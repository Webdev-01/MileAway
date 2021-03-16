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
        public int Vehicle_Id { get; set; }
        [Required]
        public int Cost { get; set; }
        public string Fuel_Type { get; set; }
        public int Fuel_Quantity { get; set; }
        public string Repair_Information { get; set; }
        [Required]
        public DateTime Date_Of_Cost { get; set; }
        public string Invoice_Doc { get; set; }
    }
}
