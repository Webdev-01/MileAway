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
    public class registerModel : PageModel
    {
        [BindProperty]
        public Users User { get; set; }

        public string Error { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                var registerUser = UsersRepository.RegisterUser(User);
                if (registerUser)
                {
                    return RedirectToPage("Login");
                } else
                {
                    Error = "This emailadress is already in use";
                    return Page();
                }
            }
            return Page();
        }
    }
}