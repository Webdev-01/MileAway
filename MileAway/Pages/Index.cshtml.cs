using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MileAway.Models;
using MileAway.Repositories;


namespace MileAway.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public List<Vehicles> Vehicles { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToPage("Login");
            }

            Vehicles = VehiclesRepository.GetVehiclesByEmail(HttpContext.Session.GetString("email"));
            //TODO: get fixed costs and get average costs
            return Page();
        }

    }
}
