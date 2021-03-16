using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;

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
            Vehicle.User_Id = (int)HttpContext.Session.GetInt32("user_id");
            new VehiclesRepository().AddVehicle(Vehicle);
        }
    }
}
