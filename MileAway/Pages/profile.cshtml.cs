using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;

namespace MileAway.Pages.Shared
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public Users User { get; set; }
        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("email") != null)
            {
                User = UsersRepository.GetUserByEmail(HttpContext.Session.GetString("email"));
            }
        }
    }
}