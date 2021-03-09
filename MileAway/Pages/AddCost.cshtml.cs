using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MileAway.Pages
{
    public class AddCostModel : PageModel
    {
        public string costType { get; set; }
        public string costTypeString { get; set; }

        public ActionResult OnGet()
        {
            costType = HttpContext.Request.Query["type"];
            costTypeString = costType == "brandstof" ? "selected" : "";
            return Page();
        }
    }
}
