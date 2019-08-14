using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;


namespace WebApplication.Web.DAL
{
    public class MessageSqlDAL :IMessageSqlDAL
    {
        private readonly string connectionString;

        public MessageSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateMessage(Message message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO message_table VALUES (@senderId, @recieverId, @text, @date);", conn);
                    cmd.Parameters.AddWithValue("@senderId", message.SenderId);
                    cmd.Parameters.AddWithValue("@recieverId", message.RecieiverId);
                    cmd.Parameters.AddWithValue("@text", message.Text);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);

                    cmd.ExecuteNonQuery();


                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, Message> GetMessagesByUsername (User user)
        {
            
            List<int> ids = new List<int>();
            Dictionary<string, Message> messagesByUsername = new Dictionary<string, Message>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT sender_id FROM message_table WHERE reciever_id = {user.Id} GROUP BY sender_id", conn);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ids.Add(Convert.ToInt32(reader["sender_id"]));
                    }
                    reader.Close();

                    foreach (int id in ids)
                    {
                        string Username = "";
                        cmd = new SqlCommand($"SELECT username FROM users WHERE id = {id}", conn);

                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            Username = Convert.ToString(reader["username"]);
                        }
                        reader.Close();

                        cmd = new SqlCommand($"SELECT * FROM message_table WHERE sender_id = {id} ORDER BY date_sent DESC", conn);

                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            
                            messagesByUsername.Add(Username, MapRowToMessage(reader));
                        }
                        reader.Close();
                    }
                    

                    return messagesByUsername;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public List<string> GetSenderNames(User user)
        //{
        //    List<int> sendersIds = new List<int>();
        //    List<string> senders = new List<string>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand($"SELECT sender_id, date_sent FROM message_table WHERE reciever_id = '{user.Id}' GROUP BY sender_id, date_sent ORDER BY date_sent DESC;", conn);


        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                sendersIds.Add(Convert.ToInt32(reader["sender_id"]));
        //            }
        //            reader.Close();

        //            foreach (int id in sendersIds)
        //            {
        //                cmd = new SqlCommand($"SELECT username FROM users WHERE id = {id};", conn);

        //                reader = cmd.ExecuteReader();

        //                if (reader.Read())
        //                {
        //                    senders.Add(Convert.ToString(reader["username"]));
        //                }
        //                reader.Close();
        //            }

        //        }

        //        return senders;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public Message GetRecentMessage(int SenderUsername, int RecieverUsername)
        //{
        //    Message recent = null;
        //    int SenderId = -1;
        //    int RecieverId = -1;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand("SELECT id FROM users WHERE username = @sender;", conn);
        //            cmd.Parameters.AddWithValue("@sender", SenderUsername);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                SenderId = Convert.ToInt32(reader["sender_id"]);
        //            }
        //            reader.Close();

        //            cmd = new SqlCommand("SELECT id FROM users WHERE username = @reciever;", conn);
        //            cmd.Parameters.AddWithValue("@reciever", RecieverUsername);

        //            reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                RecieverId = Convert.ToInt32(reader["reciever_id"]);
        //            }
        //            reader.Close();

        //            cmd = new SqlCommand("SELECT * FROM message_table WHERE sender_id = @sender AND recieiver_id = @reciever ORDER BY date_sent DESC;", conn);
        //            cmd.Parameters.AddWithValue("@sender", SenderId);
        //            cmd.Parameters.AddWithValue("@reciever", RecieverId);

        //            reader = cmd.ExecuteReader();



        //            if (reader.Read())
        //            {
        //                recent = (MapRowToMessage(reader));
        //            }
        //            reader.Close();
        //        }

        //        return recent;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public List<Message> GetConversation(string SenderUsername, string RecieverUsername)
        {
            List<Message> Conversation = new List<Message>();
            int SenderId = -1;
            int RecieverId = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id FROM users WHERE username = @sender;", conn);
                    cmd.Parameters.AddWithValue("@sender", SenderUsername);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        SenderId = Convert.ToInt32(reader["id"]);
                    }
                    reader.Close();

                    cmd = new SqlCommand("SELECT id FROM users WHERE username = @reciever;", conn);
                    cmd.Parameters.AddWithValue("@reciever", RecieverUsername);

                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        RecieverId = Convert.ToInt32(reader["id"]);
                    }
                    reader.Close();

                    cmd = new SqlCommand("SELECT * FROM message_table WHERE (sender_id = @sender AND reciever_id = @reciever_id) or (sender_id = @reciever_id AND reciever_id = @sender) order by date_sent asc;", conn);
                    cmd.Parameters.AddWithValue("@sender", SenderId);
                    cmd.Parameters.AddWithValue("@reciever_id", RecieverId);

                    reader = cmd.ExecuteReader();


                    /// keep working on this section


                    while (reader.Read())
                    {
                        Conversation.Add(MapRowToMessage(reader));
                    }
                    reader.Close();
                }

                return Conversation;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Message MapRowToMessage(SqlDataReader reader)
        {
            string Text = Convert.ToString(reader["message_text"]);
            DateTime DateSent = Convert.ToDateTime(reader["date_sent"]);
            int SenderId = Convert.ToInt32(reader["sender_id"]);
            int RecieverId = Convert.ToInt32(reader["reciever_id"]);
            Message message = new Message(Text, DateSent, SenderId, RecieverId);

            return message;
        }
    }
}
