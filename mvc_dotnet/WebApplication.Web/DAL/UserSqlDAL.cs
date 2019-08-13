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
<<<<<<< HEAD
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


=======
                    string cmdStr = "INSERT INTO Users (email, username, birthdate, home_city, home_state, self_description, password_hash, salt) VALUES (@Email, @Username, @Birthdate, @HomeCity, @HomeState, @SelfDescription, @Password, @salt);";
                    SqlCommand cmd = new SqlCommand(cmdStr, conn);
                    
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    cmd.Parameters.AddWithValue("@HomeCity", user.HomeCity);
                    cmd.Parameters.AddWithValue("@HomeState", user.HomeState);
                    cmd.Parameters.AddWithValue("@SelfDescription", user.SelfDescription);
                    cmd.Parameters.AddWithValue("@password", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                   
                    int i = cmd.ExecuteNonQuery();
                    //gets the id of the new user so we can add instruments and places with the users ID as the foreign key
                    cmdStr = $"SELECT ID FROM Users WHERE username = '{user.Username}' and email = '{user.Email}';";
                    cmd = new SqlCommand(cmdStr, conn);
                    string userId = Convert.ToString(cmd.ExecuteScalar());

                    foreach (Instrument instrument in user.ListOfInstruments)
                    {
                        cmdStr = $"INSERT INTO Instruments_Played VALUES ('{userId}',@Name);";
                        cmd = new SqlCommand(cmdStr, conn);
                        cmd.Parameters.AddWithValue("@Name", instrument.Name);

                        cmd.ExecuteNonQuery();
                    }

                    foreach (Place place in user.ListOfPlaces)
                    {
                        cmdStr = $"INSERT INTO Places VALUES ('{userId}',@City,@State,@FromDate,@ToDate);";
                        cmd = new SqlCommand(cmdStr, conn);
                        cmd.Parameters.AddWithValue("@City", place.CityName);
                        cmd.Parameters.AddWithValue("@State", place.StateName);
                        cmd.Parameters.AddWithValue("@FromDate", place.FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", place.ToDate);


                        cmd.ExecuteNonQuery();
                    }

>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
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

<<<<<<< HEAD


       
/// <summary>
/// Gets the user from the database.
/// </summary>
/// <param name="username"></param>
/// <returns></returns>
    public User GetUser(string username)
=======
        /// <summary>
        /// Gets the user from the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
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
<<<<<<< HEAD
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

        public List<User> GetUsers(int excludeCurrentUserId)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS where id <> @excludeCurrentUserId;", conn);
                    cmd.Parameters.AddWithValue("@excludeCurrentUserId", excludeCurrentUserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(MapRowToUser(reader));
                    }
                    reader.Close();
                }

                return users;
=======

                    reader.Close();

                    cmd = new SqlCommand("SELECT ID FROM users WHERE username = '@username'", conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    string userId = Convert.ToString(cmd.ExecuteScalar());


                    cmd = new SqlCommand($"SELECT instrument_name FROM Instruments_Played WHERE user_id = '{userId}';", conn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.ListOfInstruments.Add(MapRowToInstrument(reader));
                    }

                    //cmd = new SqlCommand($"SELECT ine FROM Instruments_Played WHERE user_id = {userId};", conn);
                    //reader = cmd.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    user.ListOfInstruments.Add(MapRowToInstrument(reader));
                    //}
                }

                return user;
>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
            }
            catch (SqlException ex)
            {
                throw ex;
<<<<<<< HEAD
            }
=======
            }            
>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
        }

        /// <summary>
        /// Updates the user in the database.
        /// </summary>
        /// <param name="user"></param>
<<<<<<< HEAD
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

=======
        //public void UpdateUser(User user)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand("UPDATE users SET password = @password, salt = @salt, 
        //                = @role WHERE id = @id;", conn);                    
        //            cmd.Parameters.AddWithValue("@password", user.Password);
        //            cmd.Parameters.AddWithValue("@salt", user.Salt);
                    
        //            cmd.Parameters.AddWithValue("@id", user.Id);

        //            cmd.ExecuteNonQuery();

        //            return;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = Convert.ToInt32(reader["ID"]),
                Username = Convert.ToString(reader["username"]),
                BirthDate = Convert.ToDateTime(reader["birthdate"]),
                PasswordHash = Convert.ToString(reader["password_hash"]),
                Email = Convert.ToString(reader["email"]),
                HomeCity = Convert.ToString(reader["home_city"]),
                HomeState = Convert.ToString(reader["home_state"]),
                SelfDescription = Convert.ToString(reader["self_description"]),
                Salt = Convert.ToString(reader["salt"]),
                
            };
        }

        private Instrument MapRowToInstrument(SqlDataReader reader)
        {
            string Name = Convert.ToString(reader["instrument_name"]);
            return new Instrument(Name);
        }
    }
}
>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
