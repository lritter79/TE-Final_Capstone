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
    public class MessageSqlDALTests : DatabaseTests
    {
        [TestMethod]
        public void CreateMessageTest()
        {
            MessageSqlDAL dal = new MessageSqlDAL(ConnectionString);
            Message m = new Message("text", DateTime.Now, -1, -2);

            dal.CreateMessage(m);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string cmdText = "SELECT sender_id FROM message_table WHERE reciever_id = '-2';";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string senderId = Convert.ToString(command.ExecuteScalar());

                Assert.AreEqual("-1", $"{senderId}");

            }

        }

        [TestMethod]
        public void GetMessagesTest()
        {

            UserSqlDAL userDal = new UserSqlDAL(ConnectionString);
            User user = userDal.GetUser("luteMan");
            MessageSqlDAL messageSql = new MessageSqlDAL(ConnectionString);
            Dictionary<string, Message> messages = null;

            messages = messageSql.GetMessagesByUsername(user);

            Assert.IsNotNull(messages);
            
        }

    }
}
