using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MileAway.Pages
{
    public class IndexModel : PageModel
    {
        private bool isLoggedIn { get; set; } = true;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (isLoggedIn)
                return RedirectToPage("Overview");
            return RedirectToPage("Login");
        }
    }
}
