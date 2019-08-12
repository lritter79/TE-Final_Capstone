using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Web.Controllers
{    
    public class AccountController : Controller
    {
        private readonly IHostingEnvironment he;

        private readonly IAuthProvider authProvider;
        public AccountController(IAuthProvider authProvider, IHostingEnvironment e)
        {
            this.authProvider = authProvider;
            he = e;
        }
        
        //[AuthorizationFilter] // actions can be filtered to only those that are logged in
        [AuthorizationFilter("Admin", "Author", "Manager", "User")]  //<-- or filtered to only those that have a certain role
        [HttpGet]
        public IActionResult Index()
        {
            var user = authProvider.GetCurrentUser();
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.Username, loginViewModel.Password);
                if (validLogin)
                {
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("BioPage", "Account");
                }
            }

            return View(loginViewModel);
        }
        
        [HttpGet]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();

            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {            
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".
                authProvider.Register(registerViewModel.Email, registerViewModel.Username, registerViewModel.BirthDate, registerViewModel.HomeCity, registerViewModel.HomeState, registerViewModel.SelfDescription, registerViewModel.Password, role: "User"); 

                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult PerspectiveDates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostBioPage(User user)
        {
            //dao.SaveBioPage(User user); this is where the code will go to save this page.
            return RedirectToAction("PerspectiveDates", "Account");
        }


        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowProfile(string description, IFormFile pic)
        {
            ViewData["description"] = description;
            if(pic != null)
            {
                var profilePicsPath = he.WebRootPath + "\\images\\profile_pics";
                var fileName = Path.Combine(profilePicsPath, Path.GetFileName(pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));
                var imagePath = "/images/profile_pics/" + Path.GetFileName(pic.FileName);
                ViewData["fileLocation"] = imagePath;

            }
            return View();
        }

        [HttpGet]
        public IActionResult BioPage()
        {
            var user = authProvider.GetCurrentUser();
            return View(user);
        }

        [HttpPost]
        public IActionResult AddPic(IFormFile pic)
        {
            if (pic != null)
            {
                var profilePicsPath = he.WebRootPath + "\\images\\profile_pics";
                var fileName = Path.Combine(profilePicsPath, Path.GetFileName(pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));
                var imagePath = "/images/profile_pics/" + Path.GetFileName(pic.FileName);
                ViewData["fileLocation"] = imagePath;

                authProvider.AddPic(imagePath);

                return RedirectToAction("BioPage", "Account");

            }
            return View();
        }

        [HttpPost]
        public IActionResult AddDescription(string description)
        {

            authProvider.AddDescription(description);
            return RedirectToAction("BioPage", "Account");
        }

        [HttpPost]
        public IActionResult ChangePrivacy(int isPublic)
        {
            bool publicBool = isPublic == 1 ? true : false;
            authProvider.ChangePrivacy(publicBool);
            return RedirectToAction("BioPage", "Account");
        }

        [HttpPost]
        public IActionResult AddComposer(string composer)
        {
            authProvider.AddComposer(composer);
            return RedirectToAction("BioPage", "Account");
        }
    }
}