using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class UserSqlDALTests : DatabaseTests
    {
        [TestMethod]
        public void CreateUserTest()
        {
            UserSqlDAL dal = new UserSqlDAL(ConnectionString);
            User user = new User();
            user.BirthDate = "08/22/1990";
            user.Email = "fake@gmail.com";
            user.HomeCity = "pittsburgh";
            user.HomeState = "PA";
            user.Instrument = "lute";
            user.Password = "fake";
            user.Salt = "testSalt";
            user.Places = "testplaces";
            user.SelfDescription = "testdescription";
            user.Username = "fakeuser";

            dal.CreateUser(user);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT email FROM users WHERE username = 'fakeuser'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string userEmail = Convert.ToString(command.ExecuteScalar());

                Assert.AreEqual("fake@gmail.com", $"{userEmail}");
            }
        }
    }
    
}
