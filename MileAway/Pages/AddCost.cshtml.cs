using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        private IHostingEnvironment ihostingEnvironment;

        [BindProperty]
        public IFormFile Invoice { get; set; }

        public AddCostModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }

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
                if (Invoice != null)
                {
                    var path = Path.Combine(ihostingEnvironment.WebRootPath, "invoices", license + " - " + Costs.Invoice_Doc + " - " + Invoice.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Invoice.CopyToAsync(stream);
                    }

                    CostsRepository.AddCostRepairFile(Costs, Invoice.FileName);
                    if (Vehicles.Mileage_Km != Milage_KM)
                    {
                        VehiclesRepository.UpdateMileage_KM(license, Vehicles.Mileage_Km);
                    }

                }
                else
                {

                    CostsRepository.AddCostRepair(Costs);
                    if (Vehicles.Mileage_Km != Milage_KM)
                    {
                        VehiclesRepository.UpdateMileage_KM(license, Vehicles.Mileage_Km);
                    }
                }

            }

            return RedirectToPage("Index");
        }
    }
}
