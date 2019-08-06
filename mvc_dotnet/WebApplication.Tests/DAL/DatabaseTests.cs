﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        protected string ConnectionString = "Server=.\\SQLEXPRESS;Database=EarlyMusicDating;Trusted_Connection=True;";
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
                string cmdText = "delete from Users; ";
                SqlCommand command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();                

                ///Add row to user table
                cmdText = $"INSERT INTO Users VALUES('x@y.com','luteMan', 12/09/1990,'Pittsburgh','PA','Just a small-town girl','pep','salty');SELECT SCOPE_IDENTITY();";
                command = new SqlCommand(cmdText, connection);
                command.ExecuteNonQuery();


            }
        }

        [TestMethod]
        public void GetEmailAddress()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT email FROM users WHERE username = 'luteMan'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string userEmail = Convert.ToString(command.ExecuteScalar());
                

                Assert.AreEqual("x@y.com", $"{userEmail}");
            }
        }

        [TestMethod]
        public void GetUserID()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT id FROM users WHERE username = 'luteMan'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                int userId = Convert.ToInt32(command.ExecuteScalar());


                Assert.AreEqual(1, $"{userId}");
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

