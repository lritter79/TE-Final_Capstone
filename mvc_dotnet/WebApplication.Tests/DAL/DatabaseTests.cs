using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebApplication.Web.Models;
using WebApplication.Web.DAL;
using System.Data;

using System.Transactions;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class DatabaseTests
    {

        public string ConnectionString = "Server=.\\SQLEXPRESS;Database=EarlyMusicDating;Trusted_Connection=True;";
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            // limits the scope of tranaction so that the app could be used and tested simultaneously?
            transaction = new TransactionScope();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                //Delete everything from our tables
                string cmdText = "delete from Users;delete from Instruments_Played;delete from Places;";
                SqlCommand command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();                

                ///Add row to user table
                cmdText = $"INSERT INTO Users VALUES('x@y.com','luteMan', 12/09/1990,'Pittsburgh','PA','Just a small-town girl','pep','salty', '1');SELECT SCOPE_IDENTITY();";
                command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();

                cmdText = "SELECT ID FROM users WHERE username = 'luteMan'";
                command = new SqlCommand(cmdText, connection);
                string userId = Convert.ToString(command.ExecuteScalar());

                cmdText = $"INSERT INTO Instruments_Played VALUES('{userId}','lute');INSERT INTO Instruments_Played VALUES('{userId}','archlute');INSERT INTO Instruments_Played VALUES('{userId}','theorbo');";
                command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();

                cmdText = $"INSERT INTO Places VALUES('{userId}','footown','barland','{DateTime.Now}','{DateTime.Now}');INSERT INTO Places VALUES('{userId}','fooville','baristan','{DateTime.Now}','{DateTime.Now}');";
                command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();

                cmdText = $"INSERT INTO Composers VALUES('{userId}','Purcell');INSERT INTO Composers VALUES('{userId}','Locke');";
                command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();

            }
        }

        [TestMethod]
        public void GetInfo()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT email FROM users WHERE username = 'luteMan'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string userEmail = Convert.ToString(command.ExecuteScalar());

                cmdText = "SELECT ID FROM users WHERE username = 'luteMan'";
                command = new SqlCommand(cmdText, connection);
                string userId = Convert.ToString(command.ExecuteScalar());

                cmdText = $"SELECT instrument_name FROM Instruments_Played WHERE user_id = '{userId}' ORDER BY instrument_name ASC";
                command = new SqlCommand(cmdText, connection);
                string firstInstrument = Convert.ToString(command.ExecuteScalar());

                cmdText = $"SELECT composer_name FROM Composers WHERE user_id = '{userId}' ORDER BY composer_name ASC";
                command = new SqlCommand(cmdText, connection);
                string firstComposer = Convert.ToString(command.ExecuteScalar());


                Assert.AreEqual("x@y.com", $"{userEmail}");
                Assert.AreEqual("archlute", $"{firstInstrument}");
                Assert.AreEqual("Locke", $"{firstComposer}");
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            // Roll back the transaction
            transaction.Dispose();
        }
    }

}

