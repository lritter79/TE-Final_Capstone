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
                    SqlCommand cmd = new SqlCommand("INSERT INTO message_table VALUES (@senderId, @receiverId, @text, @date, 0);", conn);
                    cmd.Parameters.AddWithValue("@senderId", message.SenderId);
                    cmd.Parameters.AddWithValue("@receiverId", message.ReceiverId);
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

        public List<Message> GetCurrentUsersMessages(User user)
        {
            List<Message> CurrentUserMessages = new List<Message>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM message_table WHERE receiver_id = {user.Id}", conn);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CurrentUserMessages.Add(MapRowToMessage(reader));
                    }
                    reader.Close();

                    
                }

                return CurrentUserMessages;
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
                    SqlCommand cmd = new SqlCommand($"SELECT sender_id FROM message_table WHERE receiver_id = {user.Id} GROUP BY sender_id", conn);


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

        //            SqlCommand cmd = new SqlCommand($"SELECT sender_id, date_sent FROM message_table WHERE receiver_id = '{user.Id}' GROUP BY sender_id, date_sent ORDER BY date_sent DESC;", conn);


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

        //public Message GetRecentMessage(int SenderUsername, int receiverUsername)
        //{
        //    Message recent = null;
        //    int SenderId = -1;
        //    int receiverId = -1;

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

        //            cmd = new SqlCommand("SELECT id FROM users WHERE username = @receiver;", conn);
        //            cmd.Parameters.AddWithValue("@receiver", receiverUsername);

        //            reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                receiverId = Convert.ToInt32(reader["receiver_id"]);
        //            }
        //            reader.Close();

        //            cmd = new SqlCommand("SELECT * FROM message_table WHERE sender_id = @sender AND receiver_id = @receiver ORDER BY date_sent DESC;", conn);
        //            cmd.Parameters.AddWithValue("@sender", SenderId);
        //            cmd.Parameters.AddWithValue("@receiver", receiverId);

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

        public List<Message> GetConversation(string OtherUsername, string CurrentUsername)
        {
            List<Message> Conversation = new List<Message>();
            int OtherUserId = -1;
            int CurrentUserId = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id FROM users WHERE username = @sender;", conn);
                    cmd.Parameters.AddWithValue("@sender", OtherUsername);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        OtherUserId = Convert.ToInt32(reader["id"]);
                    }
                    reader.Close();

                    cmd = new SqlCommand("SELECT id FROM users WHERE username = @receiver;", conn);
                    cmd.Parameters.AddWithValue("@receiver", CurrentUsername);

                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        CurrentUserId = Convert.ToInt32(reader["id"]);
                    }
                    reader.Close();

                    cmd = new SqlCommand("SELECT * FROM message_table WHERE (sender_id = @sender AND receiver_id = @receiver_id) or (sender_id = @receiver_id AND receiver_id = @sender) order by date_sent asc;", conn);
                    cmd.Parameters.AddWithValue("@sender", OtherUserId);
                    cmd.Parameters.AddWithValue("@receiver_id", CurrentUserId);

                    reader = cmd.ExecuteReader();


                    /// keep working on this section


                    while (reader.Read())
                    {
                        Conversation.Add(MapRowToMessage(reader));
                    }
                    reader.Close();
                }

                foreach (Message message in Conversation)
                {
                    if (message.ReceiverId == CurrentUserId)
                    {
                        message.isRead = true;
                    }
                    

                }

                UpdateToRead(Conversation, CurrentUserId, OtherUserId);

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
            int receiverId = Convert.ToInt32(reader["receiver_id"]);
            bool isRead = Convert.ToBoolean(reader["is_read"]);
            Message message = new Message(Text, DateSent, SenderId, receiverId);
            message.isRead = isRead;
            

            return message;
        }

        public void UpdateToRead(List<Message> ListOfMessages, int CurrentUserId, int OtherUserId)
        {
            foreach (Message message in ListOfMessages)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE message_table set is_read = 1 WHERE receiver_id = @receiverId AND sender_id = @senderId;", conn);
                        
                        cmd.Parameters.AddWithValue("@receiverId", CurrentUserId);
                        cmd.Parameters.AddWithValue("@senderId", OtherUserId);

                        cmd.ExecuteNonQuery();


                        
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

        }
    }

}
