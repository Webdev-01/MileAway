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
        public Vehicles vehicles { get; set; }
        public void OnGet()
        {
        }
    }
}
