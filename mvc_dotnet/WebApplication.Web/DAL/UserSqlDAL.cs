﻿using System;
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO users (email, username, birthdate, home_city, home_state, password, salt, role, is_public) VALUES (@email, @username, @birthdate, @home_city, @home_state, @password, @salt, @role, '1');", conn);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    cmd.Parameters.AddWithValue("@home_city", user.HomeCity);
                    cmd.Parameters.AddWithValue("@home_state", user.HomeState);
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

        /// <summary>
        /// get's all users in our database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> UserList = new List<User>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    cmd = new SqlCommand($"SELECT * FROM Users WHERE is_public = 1;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserList.Add(MapRowToUser(reader));
                    }
                    reader.Close();

                    foreach (User user in UserList)
                    {
                        //cmd = new SqlCommand($"SELECT city, state_name,from_date,to_date FROM Places WHERE user_id = '{user.Id}';", conn);
                        //reader = cmd.ExecuteReader();

                        //while (reader.Read())
                        //{
                        //    user.ListOfPlaces.Add(MapRowToPlace(reader));
                        //}

                        //reader.Close();

                        cmd = new SqlCommand($"SELECT composer_name FROM Composers WHERE user_id = '{user.Id}';", conn);
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            user.ListOfComposers.Add(MapRowToComposer(reader));
                        }

                        reader.Close();
                    }

                }

                return UserList;
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

