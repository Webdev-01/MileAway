using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;
using Microsoft.AspNetCore.Http;

namespace MileAway.Pages
{
    public class LoginModel : PageModel
    {
        public List<Users> users { get; set; }

        [BindProperty]
        public Users User { get; set; }

        public void OnGet()
        {
            users = UsersRepository.GetUsers();
        }

        public IActionResult OnPostLogin()
        {
            var user = UsersRepository.GetUserByLogin(User);
            if (user != null)
            {
                //Maak session aan en redirect naar homepage
                HttpContext.Session.SetString("user_id", user.USER_ID.ToString());
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}