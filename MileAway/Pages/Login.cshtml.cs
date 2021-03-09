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
    public class LoginModel : PageModel
    {
        public List<Users> users { get; set; }
        public void OnGet()
        {
            users = UsersRepository.GetUsers();
        }
    }
}
