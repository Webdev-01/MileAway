using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MileAway.Models
{
    public class FixedCosts
    {
        public int Cost_Id { get; set; }
        public double Insurance { get; set; }
        public DateTime Insurance_Date { get; set; }
        public double Road_Tax { get; set; }
        public DateTime Road_Tax_Date { get; set; }

    }
}
