using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;

namespace MileAway.Pages
{
    public class AddVehicleModel : PageModel
    {
        [BindProperty]
        public Vehicles Vehicle { get; set; }

        [BindProperty]
        public double Road_Tax { get; set; }
        [BindProperty]
        public double Insurance { get; set; }

        public void OnGet()
        {
        }

        public void OnPostConfirm()
        {
            var test = Vehicle;
        }
    }
}
