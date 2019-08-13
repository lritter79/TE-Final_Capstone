using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;


namespace WebApplication.Web.DAL
{
    public class MessageSqlDAL
    {
        private readonly string connectionString;

        public MessageSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Message> GetConversation(int SenderId, int RecieverId)
        {
            List<Message> Conversation = new List<Message>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM message_table WHERE sender_id = @sender AND recieiver_id = @reciever;", conn);
                    cmd.Parameters.AddWithValue("@sender", SenderId);
                    cmd.Parameters.AddWithValue("@reciever", RecieverId);

                    SqlDataReader reader = cmd.ExecuteReader();


                    /// keep working on this section
            //        if (reader.Read())
            //        {
            //            user = MapRowToUser(reader);
            //        }
            //        reader.Close();
            //        SqlCommand composerCmd = new SqlCommand($"SELECT composer_name FROM composers WHERE user_id={user.Id}", conn);
            //        reader = composerCmd.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            user.ListOfComposers.Add(MapRowToComposer(reader));
            //        }
            //        reader.Close();
            //    }

            //    return user;
            //}
            //catch (SqlException ex)
            //{
            //    throw ex;
            //}
        }
    }
}
