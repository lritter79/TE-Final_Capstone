using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class UserSqlDAL : IUserDAL
    {
        private readonly string connectionString;

        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Saves the user to the database.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO users (email, username, birthdate, home_city, home_state, password, salt, role, is_public, gender, seeking) VALUES (@email, @username, @birthdate, @home_city, @home_state, @password, @salt, @role, '1', @gender, @seeking);", conn);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    cmd.Parameters.AddWithValue("@home_city", user.HomeCity);
                    cmd.Parameters.AddWithValue("@home_state", user.HomeState);
                    cmd.Parameters.AddWithValue("@gender", user.Gender);
                    cmd.Parameters.AddWithValue("@seeking", user.Seeking);
                    //cmd.Parameters.AddWithValue("@self_description", user.SelfDescription);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);

                    cmd.ExecuteNonQuery();


                    return;
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", user.Id);                    

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void BlockUser(int userId, int blockedUserId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO blocked (current_user_id, blocked_user_id) VALUES (@userId, blockedUserId);", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@blockedUserId", blockedUserId);

                    cmd.ExecuteNonQuery();


                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



/// <summary>
/// Gets the user from the database.
/// </summary>
/// <param name="username"></param>
/// <returns></returns>
        public User GetUser(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE username = @username;", conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                    reader.Close();
                    SqlCommand composerCmd = new SqlCommand($"SELECT composer_name FROM composers WHERE user_id={user.Id}", conn);
                    reader = composerCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.ListOfComposers.Add(MapRowToComposer(reader));
                    }
                    reader.Close();
                    //SqlCommand blockCommand = new SqlCommand($"SELECT * from USERS WHERE id in(SELECT blocked_user_id FROM blocked WHERE current_user_id={user.Id})", conn);
                    //reader = blockCommand.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    User blockedUser = MapRowToUser(reader);
                    //    user.BlockedUsers.Add(blockedUser);
                    //}
                    //reader.Close();
                }

                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }            
        }

        private User MapRowToUser(SqlDataReader reader)
        {
            var test = reader["self_description"];

            User user = new User()
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = Convert.ToString(reader["username"]),
                Password = Convert.ToString(reader["password"]),
                SelfDescription = Convert.ToString(reader["self_description"]),
                IsPublic = Convert.ToBoolean(reader["is_public"]),
                ProfilePic = Convert.ToString(reader["profile_pic"]),
                BirthDate = Convert.ToDateTime(reader["birthdate"]),
                Gender = Convert.ToInt32(reader["gender"]),
                Seeking = Convert.ToInt32(reader["seeking"]),
                HomeCity = Convert.ToString(reader["home_city"]),
                HomeState = Convert.ToString(reader["home_State"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role"])
            };
            return user;
        }
        private Composer MapRowToComposer(SqlDataReader reader)
        {
            string Name = Convert.ToString(reader["composer_name"]);
            return new Composer(Name);
        }

        private Composer MapRowToBlocked(SqlDataReader reader)
        {
            string Name = Convert.ToString(reader["composer_name"]);
            return new Composer(Name);
        }

        public List<int> GetBlockedIds(int currentUserId)
        {
            List<int> blockedUsers = new List<int>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT blocked_user_id FROM blocked where current_user_id = @currentUserId", conn);
                    cmd.Parameters.AddWithValue("@currentUserId", currentUserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        blockedUsers.Add(Convert.ToInt32(reader["blocked_user_id"]));
                    }
                    reader.Close();
                }

                return blockedUsers;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public List<User> GetUsers(int currentUserId)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS where id <> @currentUserId and id not in (SELECT current_user_id FROM blocked where blocked_user_id = @currentUserId) and id not in (SELECT current_user_id FROM blocked where blocked_user_id = @currentUserId) and id not in (SELECT blocked_user_id FROM blocked where current_user_id = @currentUserId);", conn);
                    cmd.Parameters.AddWithValue("@currentUserId", currentUserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(MapRowToUser(reader));
                    }
                    reader.Close();
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET password = @password, salt = @salt, role = @role WHERE id = @id;", conn);                    
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateDescription(User user, string description)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET self_description = @description WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdatePic(User user, string filename)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET profile_pic = @filename WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@filename", filename);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdatePrivacy(User user, bool isPublic)
        {
            int privacyVal;
            if (!isPublic)
            {
                privacyVal = 0;
            }
            else
            {
                privacyVal = 1;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET is_public = @ispublic WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@ispublic", privacyVal);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void AddComposer(User user, string composer)
        {
            int userId = user.Id;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string cmdStr = $"INSERT INTO Composers VALUES ('{user.Id}',@Name);";
                    SqlCommand cmd = new SqlCommand(cmdStr, conn);
                    cmd.Parameters.AddWithValue("@Name", composer);

                    cmd.ExecuteNonQuery();
                    

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }
    }
}

