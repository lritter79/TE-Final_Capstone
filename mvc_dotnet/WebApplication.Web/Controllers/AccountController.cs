using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using WebApplication.Web.DAL;

namespace WebApplication.Web.Controllers
{    
    public class AccountController : Controller
    {
        private IUserDAL UserDAL { get; }
        private readonly IAuthProvider authProvider;
        public AccountController(IAuthProvider authProvider, IUserDAL UserDAL)
        {
            this.authProvider = authProvider;
            this.UserDAL = UserDAL;
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
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            // Check that they provided correct credentials
             /*   bool validLogin = authProvider.SignIn(loginViewModel.Email, loginViewModel.Password);
                if (!validLogin)
                {
                    
                    return View(loginViewModel);
                }
                else
            {
                //dao.*/
            
            // Redirect the user where you want them to go after successful login
            return RedirectToAction("BioPage", "Account");
        }

        [HttpGet]
        public IActionResult BioPage(string username)
        {
            User user = UserDAL.GetUser(username);
            return View(user);
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
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".
                //authProvider.Register(user.Email, user.PasswordHash);
                UserDAL.CreateUser(user);


                // Redirect the user where you want them to go after registering
                return RedirectToAction("RegistrationComplete");
            }
            

            else 
            {
                return View(user);
                
            }
        }


        [HttpGet]
        public IActionResult RegistrationComplete()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Register(RegisterViewModel registerViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Register them as a new user (and set default role)
        //        // When a user registeres they need to be given a role. If you don't need anything special
        //        // just give them "User".
        //        authProvider.Register(registerViewModel.Email, registerViewModel.Password, role: "User");

        //        // Redirect the user where you want them to go after registering
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(registerViewModel);
        //}
    }
}