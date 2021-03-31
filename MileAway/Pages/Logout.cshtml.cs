using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace MileAway.Pages
{
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Removes session and redirects to login
        /// </summary>
        /// <returns>Redirect to login</returns>
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("email");
            return RedirectToPage("Login");
        }
    }
}
