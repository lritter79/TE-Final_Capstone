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
                    SqlCommand cmd = new SqlCommand("INSERT INTO users (email, username, password, salt, role, age , home_city, home_state, self_description, is_public) VALUES (@email, @username, @password, @salt, @role, @age, @homeCity, @homeState, @selfDescription, '1');", conn);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@homeCity", user.HomeCity);
                    cmd.Parameters.AddWithValue("@homeState", user.HomeState);
                    cmd.Parameters.AddWithValue("@selfDescription", user.SelfDescription);
                    cmd.Parameters.AddWithValue("@age ", user.Age);

                    int i = cmd.ExecuteNonQuery();
                    //gets the id of the new user so we can add instruments and places with the users ID as the foreign key
                    string cmdQuery = $"SELECT ID FROM Users WHERE username = '{user.Username}' and email = '{user.Email}';";
                    cmd = new SqlCommand(cmdQuery, conn);
                    string userId = Convert.ToString(cmd.ExecuteScalar());

                    foreach (Instrument instrument in user.ListOfInstruments)
                    {
                        cmdQuery = $"INSERT INTO Instruments_Played VALUES ('{userId}',@Name);";
                        cmd = new SqlCommand(cmdQuery, conn);
                        cmd.Parameters.AddWithValue("@Name", instrument.Name);

                        cmd.ExecuteNonQuery();
                    }

                    foreach (Composer composer in user.ListOfComposers)
                    {
                        cmdQuery = $"INSERT INTO Composers VALUES ('{userId}',@Name);";
                        cmd = new SqlCommand(cmdQuery, conn);
                        cmd.Parameters.AddWithValue("@Name", composer.Name);

                        cmd.ExecuteNonQuery();
                    }

                    foreach (Place place in user.ListOfPlaces)
                    {
                        cmdQuery = $"INSERT INTO Places VALUES ('{userId}',@City,@State,@FromDate,@ToDate);";
                        cmd = new SqlCommand(cmdQuery, conn);
                        cmd.Parameters.AddWithValue("@City", place.CityName);
                        cmd.Parameters.AddWithValue("@State", place.StateName);
                        cmd.Parameters.AddWithValue("@FromDate", place.FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", place.ToDate);


                        cmd.ExecuteNonQuery();
                    }

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
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE email = @email;", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                        
                    }

                    reader.Close();

                    cmd = new SqlCommand("SELECT ID FROM users WHERE email = @email", conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    string userId = Convert.ToString(cmd.ExecuteScalar());


                    cmd = new SqlCommand($"SELECT instrument_name FROM Instruments_Played WHERE user_id = '{userId}';", conn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.ListOfInstruments.Add(MapRowToInstrument(reader));
                    }

                    reader.Close();

                    cmd = new SqlCommand($"SELECT composer_name FROM Composers WHERE user_id = '{userId}';", conn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.ListOfComposers.Add(MapRowToComposer(reader));
                    }

                    reader.Close();


                    cmd = new SqlCommand($"SELECT city, state_name,from_date,to_date FROM Places WHERE user_id = '{userId}';", conn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.ListOfPlaces.Add(MapRowToPlace(reader));
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

        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = Convert.ToString(reader["username"]),
                Password = Convert.ToString(reader["password"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role"])
            };
        }

        private Instrument MapRowToInstrument(SqlDataReader reader)
        {
            string Name = Convert.ToString(reader["instrument_name"]);
            return new Instrument(Name);
        }

        private Composer MapRowToComposer(SqlDataReader reader)
        {
            string Name = Convert.ToString(reader["composer_name"]);
            return new Composer(Name);
        }

        private Place MapRowToPlace(SqlDataReader reader)
        {
            string CityName = Convert.ToString(reader["city"]);
            string StateName = Convert.ToString(reader["state_name"]);
            DateTime FromDate = Convert.ToDateTime(reader["from_date"]);
            DateTime ToDate = Convert.ToDateTime(reader["to_date"]);
            return new Place(CityName, StateName, FromDate, ToDate);
        }
    }
}

