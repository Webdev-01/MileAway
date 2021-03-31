using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;

namespace MileAway.Pages
{
    public class AddCostModel : PageModel
    {
        public string CostType { get; set; }
        public string License { get; set; }

        public int Milage_KM { get; set; }

        [BindProperty]
        public Costs Costs { get; set; }
        [BindProperty]
        public Vehicles Vehicles { get; set; }
        public ActionResult OnGet(string license)
        {
            CostType = HttpContext.Request.Query["type"];
            License = license;

            var vehicle = VehiclesRepository.GetVehicleByLicense(License);
            if (vehicle != null)
            {
                Milage_KM = vehicle.Mileage_Km;
            }

            return Page();
        }

        public IActionResult OnPost(string license)
        {
            CostType = HttpContext.Request.Query["type"];
            Costs.License = license;

            var vehicle = VehiclesRepository.GetVehicleByLicense(license);
            if(vehicle != null)
            {
                Milage_KM = vehicle.Mileage_Km;
            }
            

            if (CostType == "Brandstof")
            {
                if (Vehicles.Mileage_Km != Milage_KM)
                {
                    VehiclesRepository.UpdateMileage_KM(license, Vehicles.Mileage_Km);
                }
                CostsRepository.AddCostFuel(Costs);
            }
            else if (CostType == "Reparatie")
            {
                CostsRepository.AddCostRepair(Costs);
                if (Vehicles.Mileage_Km != Milage_KM)
                {
                    VehiclesRepository.UpdateMileage_KM(license, Vehicles.Mileage_Km);
                }
            }

            return RedirectToPage("Index");
        }
    }
}
