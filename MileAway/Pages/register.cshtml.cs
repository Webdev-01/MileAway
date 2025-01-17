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

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>If success redirect to login, if fail redirect to same page</returns>
        public IActionResult OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", User.Email + " - Avatar.png");
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Photo.CopyToAsync(stream);
                        User.User_Image = "Avatar.png";
                    }
                }
                else
                {
                    System.IO.File.Copy(Path.Combine(ihostingEnvironment.WebRootPath, "images" + "/profile.png"), Path.Combine(ihostingEnvironment.WebRootPath, "images/" + User.Email + " - Avatar.png"));
                }

                User.Password = UserMethods.SecurePasswordHasher.Hash(User.Password);
                if (UsersRepository.RegisterUser(User))
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