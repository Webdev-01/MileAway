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

        //TODO: look into validation, if everything is correct
        public IActionResult OnPostConfirm()
        {
            Vehicle.Email = HttpContext.Session.GetString("email");
            if (VehiclesRepository.AddVehicle(Vehicle))
                if (CostsRepository.AddFixedCosts(Insurance, Road_Tax, Vehicle.License))
                    return RedirectToPage("Index");
            return Page();
        }
    }
}
