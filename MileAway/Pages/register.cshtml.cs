using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MileAway.Models;
using MileAway.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MileAway.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Users User { get; set; }

        public string Error { get; set; }

        private IHostingEnvironment ihostingEnvironment;

        [BindProperty]
        public IFormFile Photo { get; set; }

        public RegisterModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", User.Email + " - Avatar.png");
                    var stream = new FileStream(path, FileMode.Create);
                    Photo.CopyToAsync(stream);
                }
                else
                {
                    System.IO.File.Copy(Path.Combine(ihostingEnvironment.WebRootPath, "images" + "/profile.png"), Path.Combine(ihostingEnvironment.WebRootPath, "images/" + User.Email + " - Avatar.png"));
                }

                User.Password = SecurePasswordHasher.Hash(User.Password);
                var registerUser = UsersRepository.RegisterUser(User);
                if (registerUser)
                {
                    return RedirectToPage("Login");
                }
                else
                {
                    Error = "This emailadress is already in use";
                }
            }
            return Page();
        }
    }
}