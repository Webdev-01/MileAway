using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be at least {2} characters long.", MinimumLength = 6)]
        public string OldPassword { get; set; }

        public string Error { get; set; }

        private IHostingEnvironment ihostingEnvironment;

        [BindProperty]
        public IFormFile Photo { get; set; }

        public ProfileModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("email") != null)
            {
                User = UsersRepository.GetUserByEmail(HttpContext.Session.GetString("email"));
            }
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <returns>If success redirect to index, if fail redirect to same page</returns>
        public IActionResult OnPostUpdate()
        {
            if (ModelState.ErrorCount == 1)
            {
                User.Email = HttpContext.Session.GetString("email");

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
                    User.User_Image = "Avatar.png";
                }
                var user = UsersRepository.GetUserByEmail(User.Email);
                if (user != null)
                {
                    if (UserMethods.SecurePasswordHasher.Verify(OldPassword, user.Password))
                    {
                        User.Password = UserMethods.SecurePasswordHasher.Hash(User.Password);
                        if (UsersRepository.UpdateUser(User))
                            return RedirectToPage("Login");
                    }
                }
                Error = "Het oude wachtwoord komt niet overeen.";
            }
            return Page();
        }
    }
}