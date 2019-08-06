using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IUserDAL
    {
        /// <summary>
        /// Retrieves a user from the system by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Users GetUser(string username);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        void CreateUser(Users user);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(Users user);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="user"></param>
        void DeleteUser(Users user);
    }
}
