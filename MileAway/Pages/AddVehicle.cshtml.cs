using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MileAway.Pages
{
    public class AddVehicleModel : PageModel
    {
        [BindProperty]
        public Vehicles Vehicle { get; set; }

        [BindProperty]
        public FixedCosts FixedCosts { get; set; }

        public List<Apidata> Apidata { get; set; }

        private IHostingEnvironment ihostingEnvironment;

        [BindProperty]
        public IFormFile Photo { get; set; }

        public AddVehicleModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }
        public void OnGet()
        {
        }
        //TODO: look into validation, if everything is correct
        /// <summary>
        /// Registers new vehicle
        /// </summary>
        /// <returns>Redirect to index if success, if fail redirect to same page</returns>
        public IActionResult OnPostConfirm()
        {
            Vehicle.Email = HttpContext.Session.GetString("email");

            if (Photo != null)
            {
                var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", Vehicle.License + " - " + Photo.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Photo.CopyToAsync(stream);
                    Vehicle.Vehicle_Image = Photo.FileName;
                }
            }

            if (VehiclesRepository.AddVehicle(Vehicle))
                if (CostsRepository.AddFixedCosts(FixedCosts, Vehicle.License))
                    return RedirectToPage("Index");
            //if (Vehicle.Vehicle_Image != null)

            return Page();
        }

        /// <summary>
        /// Gets RDW api
        /// </summary>
        /// <param name="kenteken">License of vehicle</param>
        /// <returns>JsonResult</returns>
        public IActionResult OnGetApicall(string kenteken)
        {
            if (kenteken != null)
            {
                kenteken = kenteken.Replace("-", "");
                const string URL2 = "https://opendata.rdw.nl/resource/m9d7-ebf2.json";
                const string URL = "https://opendata.rdw.nl/resource/8ys7-d773.json";
                string urlParameters = "?kenteken=" + kenteken;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // List data response.
                    HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        List<Apidata> Apidata2 = response.Content.ReadAsAsync<List<Apidata>>().Result;
                        var brandstof = Apidata2[0].brandstof_omschrijving;

                        client.Dispose();

                        //call second api
                        HttpClient client2 = new HttpClient();
                        client2.BaseAddress = new Uri(URL2);
                        // Add an Accept header for JSON format.
                        client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response2 = client2.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.

                        if (response2.IsSuccessStatusCode)
                        {
                            //dataObjects = response.Content.ReadAsAsync<IEnumerable<Apidata>>().Result;
                            List<Apidata> Apidata = response2.Content.ReadAsAsync<List<Apidata>>().Result;
                            Apidata[0].brandstof_omschrijving = brandstof;
                            client2.Dispose();
                            return new JsonResult(Apidata);

                            //return Apidata;
                        }
                        else
                        {
                            client2.Dispose();
                            return new JsonResult(false);
                        }
                    }
                    else
                    {
                        client.Dispose();
                        return new JsonResult(false);
                    }
                }
                catch
                {
                    return new JsonResult(false);
                }
            }
            else
            {
                return new JsonResult(false);
            }
        }
    }
}
