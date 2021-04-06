using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace MileAway.Pages
{
    public class LoginModel : PageModel
    {
        public List<Users> users { get; set; }

        [BindProperty]
        public Users User { get; set; }

        public void OnGet()
        {
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns>Page redirect to index if success or login if fail</returns>
        public IActionResult OnPostLogin()
        {
            var user = UsersRepository.GetUserByLogin(User);
            if (user != null)
            {
                //verify ( input , password DB)
                var result = UserMethods.SecurePasswordHasher.Verify(User.Password, user.Password);
                if (result)
                {
                    //Maak session aan en redirect naar homepage
                    HttpContext.Session.SetString("email", user.Email);
                    TempData["SuccesMessage"] = "U bent succesvol ingelogd.";
                    return RedirectToPage("Index");
                }
            }
            TempData["ErrorMessage"] = "De ingevulde gegevens komen niet overeen.";
            return Page();
        }
    }
}