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
    public class ViewVehicleModel : PageModel
    {
        [BindProperty]
        public Vehicles Vehicle { get; set; }
        [BindProperty]
        public List<Costs> Costs { get; set; }
        public string Error { get; set; }

        private IHostingEnvironment ihostingEnvironment;

        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public FixedCosts FixedCosts { get; set; }

        public ViewVehicleModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }
        public void OnGet(string license)
        {
            Vehicle = VehiclesRepository.GetVehicleByLicense(license);
            Costs = CostsRepository.GetCostsByLicenseInner(license);
        }

        /// <summary>
        /// Updates a vehicle
        /// </summary>
        /// <param name="license">License of vehicle</param>
        /// <returns>Redirect to index if success, if fail to same page.</returns>
        public IActionResult OnPostUpdate(string license)
        {
            if (ModelState.ErrorCount == 1)
            {
                Vehicle.License = license;
                if (Photo != null)
                {
                    var path = Path.Combine(ihostingEnvironment.WebRootPath, "images",
                        Vehicle.License + " - " + "car.png");

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Photo.CopyToAsync(stream);
                        Vehicle.Vehicle_Image = "car.png";
                    }
                }
                if (VehiclesRepository.UpdateVehicle(Vehicle))
                    return RedirectToPage("Index");
            }
            return Page();
        }

        /// <summary>
        /// Deletes vehicle
        /// </summary>
        /// <param name="license">License of vehicle</param>
        /// <returns>Redirect to index if success, if fail to same page</returns>
        public IActionResult OnGetDeleteVehicle(string license)
        {
            if (CostsRepository.DeleteVehicleCosts(license))
                if (VehiclesRepository.DeleteVehicle(license))
                    return RedirectToPage("Index");
            return Page();
        }

        public IActionResult OnGetDeleteCost(int id)
        {
            CostsRepository.DeleteCost(id);
            return RedirectToPage("Index");
        }
    }
}
