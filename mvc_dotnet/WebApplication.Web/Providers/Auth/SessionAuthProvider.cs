﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Web.Providers.Auth
{
    /// <summary>
    /// An implementation of the IAuthProvider that saves data within session.
    /// </summary>
    public class SessionAuthProvider : IAuthProvider
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserDAL userDAL;
        private readonly IMessageSqlDAL messageSqlDAL;
        public static string SessionKey = "Auth_User";

        public SessionAuthProvider(IHttpContextAccessor contextAccessor, IUserDAL userDAL, IMessageSqlDAL messageSqlDAL)
        {
            this.contextAccessor = contextAccessor;
            this.userDAL = userDAL;
            this.messageSqlDAL = messageSqlDAL;
        }

        /// <summary>
        /// Gets at the session attached to the http request.
        /// </summary>
        ISession Session => contextAccessor.HttpContext.Session;

        /// <summary>
        /// Returns true if the user is logged in.
        /// </summary>
        public bool IsLoggedIn => !String.IsNullOrEmpty(Session.GetString(SessionKey));

        /// <summary>
        /// Signs the user in and saves their username in session.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SignIn(string username, string password)
        {
            var user = userDAL.GetUser(username);
            var hashProvider = new HashProvider();                        
            
            if (user != null && hashProvider.VerifyPasswordMatch(user.Password, password, user.Salt))
            {                
                Session.SetString(SessionKey, user.Username);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs the user out by clearing their session data.
        /// </summary>
        public void LogOff()
        {
            Session.Clear();
        }

        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        /// <param name="existingPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string existingPassword, string newPassword)
        {            
            var hashProvider = new HashProvider();
            var user = GetCurrentUser();
            
            // Confirm existing password match
            if (user != null && hashProvider.VerifyPasswordMatch(user.Password, existingPassword, user.Salt))
            {
                // Hash new password
                var newHash = hashProvider.HashPassword(newPassword);
                user.Password = newHash.Password;
                user.Salt = newHash.Salt;

                // Save into the db
                userDAL.UpdateUser(user);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the user using the current username in session.
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            var username = Session.GetString(SessionKey);

            if (!String.IsNullOrEmpty(username))
            {
                return userDAL.GetUser(username);
            }
            
            return null;
        }

        public List<User> GetAllUsers()
        {
            var username = Session.GetString(SessionKey);

            if (!String.IsNullOrEmpty(username))
            {
                return userDAL.GetUsers(GetCurrentUser().Id);
            }

            return null;
        }

        public Dictionary<string, Message> GetMessagesByUsername(User user)
        {
            var username = Session.GetString(SessionKey);

            if (!String.IsNullOrEmpty(username))
            {
                return messageSqlDAL.GetMessagesByUsername(user);
            }

            return null;
        }

        public void AddPic(string filename)
        {
            userDAL.UpdatePic(GetCurrentUser(), filename);
        }

        public void AddDescription(string description)
        {
            userDAL.UpdateDescription(GetCurrentUser(), description);
        }

        public void ChangePrivacy(bool isPublic)
        {
            userDAL.UpdatePrivacy(GetCurrentUser(), isPublic);
        }

        public void AddComposer(string composer)
        {
            userDAL.AddComposer(GetCurrentUser(), composer);
        }

        /// <summary>
        /// Creates a new user and saves their username in session.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public void Register(string email, string username, DateTime birthdate, string homeCity, string homeState, int gender, int seeking, string selfDescription, string password, string role)
        {
            var hashProvider = new HashProvider();
            var passwordHash = hashProvider.HashPassword(password);

            var user = new User
            {
                Email = email,
                Username = username,
                BirthDate = birthdate,
                HomeCity = homeCity,
                HomeState = homeState,
                Gender = gender,
                Seeking = seeking,
                SelfDescription = selfDescription,
                Password = passwordHash.Password,
                Salt = passwordHash.Salt,
                IsPublic = true,
                Role = role
               
            };

            userDAL.CreateUser(user);
            Session.SetString(SessionKey, user.Username);            
        }

        /// <summary>
        /// Checks to see if the user has a given role.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool UserHasRole(string[] roles)
        {            
            var user = GetCurrentUser();
            return (user != null) && 
                roles.Any(r => r.ToLower() == user.Role.ToLower());
        }
    }
}
