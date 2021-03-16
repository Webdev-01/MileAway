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
    public class profileModel : PageModel
    {
        [BindProperty]
        public Users User { get; set; }
        public void OnGet()
        {
            if(HttpContext.Session.GetInt32("user_id") != null)
            { 
                User = UsersRepository.GetUserByID((int)HttpContext.Session.GetInt32("user_id"));
            }
        }
    }
}
