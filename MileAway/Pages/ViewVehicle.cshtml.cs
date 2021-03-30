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
    public class ViewVehicleModel : PageModel
    {
        [BindProperty]
        public Vehicles Vehicle { get; set; }
        [BindProperty]
        public List<Costs> Costs { get; set; }

        [BindProperty]
        public FixedCosts FixedCosts { get; set; }

        public void OnGet(string license)
        {
            Vehicle = VehiclesRepository.GetVehicleByLicense(license);
            Costs = CostsRepository.GetCostsByLicenseInner(license);
            FixedCosts = CostsRepository.GetFixedCostsByLicense(license);
        }

        public IActionResult OnGetDeleteVehicle(string license)
        {
            if (CostsRepository.DeleteVehicleCosts(license))
                if (VehiclesRepository.DeleteVehicle(license))
                    return RedirectToPage("Index");
            return Page();
        }
    }
}
