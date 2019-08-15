using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using WebApplication.Web.DAL;
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
                bool validLogin = false;
                // Check that they provided correct credentials
                try
                {
                    validLogin = authProvider.SignIn(loginViewModel.Username, loginViewModel.Password);

                }
                catch
                {
                    ModelState.AddModelError("Username", "That user name is not connected to an account");
                    //ModelState.AddModelError("Email", "That user email is taken, please enter another");
                    return View(loginViewModel);
                }

                if (validLogin)
                {
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("BioPage", "Account");

                }
            }
            ModelState.AddModelError("Password", "Incorrect password");
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
                // just give them "User
                try
                {

                    authProvider.Register(registerViewModel.Email, registerViewModel.Username, registerViewModel.BirthDate, registerViewModel.HomeCity, registerViewModel.HomeState, registerViewModel.Gender, registerViewModel.Seeking, registerViewModel.SelfDescription, registerViewModel.Password, role: "User");

                }

                catch
                {
                    ModelState.AddModelError("Username", "That user name or email is taken, please enter another");
                    //ModelState.AddModelError("Email", "That user email is taken, please enter another");
                    return View(registerViewModel);
                }
                // Redirect the user where you want them to go after registering
                if (registerViewModel.Gender == 0)
                {
                    authProvider.AddPic("/images/profile_pics/female avatar.jpg");
                }
                else
                {
                    authProvider.AddPic("/images/profile_pics/male avatar.jpg");
                }

                return RedirectToAction("RegistrationComplete", "Account");
            }
            
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult RegistrationComplete()
        {
            return View();

        }

        [HttpGet]
        public IActionResult PerspectiveDates()
        {
            MembersModel members = new MembersModel();
            members.Members = authProvider.GetAllUsers();
            members.CurrentUser = authProvider.GetCurrentUser();
            return View(members);
        }

        [HttpGet]
        public IActionResult Inbox()
        {
            var user = authProvider.GetCurrentUser();
            var inbox = authProvider.GetMessagesByUsername(user);  
            
            return View(inbox);
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

        [HttpGet]
        public IActionResult ShowProfile(int id)
        {
            ShowProfileModel showProf = new ShowProfileModel();
            MembersModel members = new MembersModel();
            members.Members = authProvider.GetAllUsers();
            foreach(User user in members.Members)
            {
                if(user.Id == id)
                {
                    showProf.User = user;
                }
            }
            showProf.Notes = authProvider.GetNotes(id);
            return View(showProf);
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

        [HttpPost]
        public IActionResult BlockUser(int blockedUserId)
        {
            authProvider.BlockUser(blockedUserId);
            return RedirectToAction("BioPage", "Account");
        }
        
        [HttpPost]
        public IActionResult UnBlockUser(int unBlockedUserId)
        {
            authProvider.UnBlockUser(unBlockedUserId);
            return RedirectToAction("BioPage", "Account");
        }

        [HttpGet]
        public IActionResult Conversation(string otherUsername)
        {
            var User = authProvider.GetCurrentUser();
            var OtherUser = authProvider.GetUserByUsername(otherUsername);
           
            List<Message> messages = authProvider.GetConversation(otherUsername, User.Username);
            ConversationModel convo = new ConversationModel();
            
            convo.Messages = messages;
            convo.CurrentUser = User;
            convo.OtherUser = OtherUser;

            return View(convo);

            //Model.sender = sender;
            //return View(Model);
        }

        [HttpPost]
        public IActionResult AddNote(int pageId, string note)
        {
            authProvider.AddNote(pageId, note);
            return RedirectToAction("ShowProfile", "Account", new { id = pageId });
        }

        [HttpPost]
        public IActionResult SendMessage(int receiverId, string message)
        {
            authProvider.SendMessage(receiverId, message);
            return RedirectToAction("ShowProfile", "Account", new { id = receiverId });
        }

        [HttpPost]
        public IActionResult Reply(int receiverId, string message, string username)
        {
            authProvider.SendMessage(receiverId, message);
            return RedirectToAction("Conversation", "Account",new { otherUsername = username });

        }
        [HttpPost]
        public IActionResult AutoMessage(int receiverId, string message)
        {
            authProvider.SendMessage(receiverId, message);
            return RedirectToAction("PerspectiveDates", "Account");
        }

        [HttpPost]
        public IActionResult DeleteNote(int pageId, int noteId)
        {
            authProvider.DeleteNote(noteId);
            return RedirectToAction("ShowProfile", "Account", new { id = pageId });
        }
    }
}