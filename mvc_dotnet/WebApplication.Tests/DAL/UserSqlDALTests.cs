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

            //populates our fake user with info
            //user.Age = 19;
            user.Email = "fake@gmail.com";
            user.HomeCity = "pittsburgh";
            user.HomeState = "PA";
            user.IsPublic = true;
            user.Password = "fake";
            user.Salt = "testSalt";
            user.Role = "User";
            user.SelfDescription = "testdescription";
            user.Username = "fakeuser";

            Instrument horn = new Instrument("Horn");

            Instrument violin = new Instrument("Violin");

            Instrument viola = new Instrument("Viola");


            user.ListOfInstruments.Add(horn);
            user.ListOfInstruments.Add(violin);
            user.ListOfInstruments.Add(viola);

            Composer b = new Composer("Bach");

            Composer l = new Composer("Lully");

            Composer r = new Composer("Rameau");


            user.ListOfComposers.Add(l);
            user.ListOfComposers.Add(b);
            user.ListOfComposers.Add(r);

            Place firstPlace = new Place("foo", "bar", DateTime.Today, DateTime.Today);
            Place secondPlace = new Place("fooburgh", "barland", DateTime.Today, DateTime.Today);


            user.ListOfPlaces.Add(firstPlace);
            user.ListOfPlaces.Add(secondPlace);

            dal.CreateUser(user);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT email FROM users WHERE username = 'fakeuser'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string userEmail = Convert.ToString(command.ExecuteScalar());

                cmdText = "SELECT ID FROM users WHERE username = 'fakeuser'";
                command = new SqlCommand(cmdText, connection);
                string userId = Convert.ToString(command.ExecuteScalar());


                cmdText = $"SELECT instrument_name FROM Instruments_Played WHERE user_id = '{userId}' ORDER BY instrument_name ASC";
                command = new SqlCommand(cmdText, connection);
                string userInstrument = Convert.ToString(command.ExecuteScalar());

                cmdText = $"SELECT from_date FROM PLaces WHERE user_id = '{userId}'";
                command = new SqlCommand(cmdText, connection);
                string userDate = Convert.ToString(command.ExecuteScalar());

                cmdText = $"SELECT composer_name FROM Composers WHERE user_id = '{userId}' ORDER BY composer_name ASC";
                command = new SqlCommand(cmdText, connection);
                string userComposer = Convert.ToString(command.ExecuteScalar());

                Assert.AreEqual("fake@gmail.com", $"{userEmail}");
                Assert.AreEqual("Horn", $"{userInstrument}");
                Assert.AreEqual($"{DateTime.Today}", actual: $"{userDate}");
                Assert.AreEqual($"Bach", actual: $"{userComposer}");

            }
        }

        [TestMethod]
        public void GetUserTest()
        {
            UserSqlDAL dal = new UserSqlDAL(ConnectionString);
            string email = "x@y.com";
            User user = dal.GetUser(email);
            Assert.IsNotNull(user);
        }

        //[TestMethod]
        //public void GetUsersListTest()
        //{
        //    UserSqlDAL dal = new UserSqlDAL(ConnectionString);

        //    List<User> users = dal.GetUsers();
        //    Assert.IsNotNull(users);
        //}
    }

}
