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

        [BindProperty]
        public Costs Costs { get; set; }
        [BindProperty]
        public Vehicles Vehicles { get; set; }

        public ActionResult OnGet(string license)
        {
            CostType = HttpContext.Request.Query["type"];
            License = license;
            return Page();
        }

        public IActionResult OnPostAddCost()
        {
            //TODO: do something in the post :P
            return Page();
        }
    }
}
