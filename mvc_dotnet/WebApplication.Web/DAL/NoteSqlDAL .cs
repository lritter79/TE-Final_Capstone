using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;


namespace WebApplication.Web.DAL
{
    public class NoteSqlDAL : INoteSqlDAL
    {
        private readonly string connectionString;

        public NoteSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddNote(Note note)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO notes (current_user_id, other_user_id, note, date_written) VALUES (@currentUserId, @notePageId, @text, @date);", conn);
                    cmd.Parameters.AddWithValue("@currentUserId", note.CurrentUserId);
                    cmd.Parameters.AddWithValue("@notePageId", note.ProfileId);
                    cmd.Parameters.AddWithValue("@text", note.Text);
                    cmd.Parameters.AddWithValue("@date", note.DateSent);

                    cmd.ExecuteNonQuery();
                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void DeleteNote(int noteId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE from NOTES where id = @noteId;", conn);
                    cmd.Parameters.AddWithValue("@noteId", noteId);

                    cmd.ExecuteNonQuery();
                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Note> GetNotes(int currentUserId, int notePageId)
        {
            List<Note> Notes = new List<Note>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM notes WHERE current_user_id = @currentUserId and other_user_id = @notePageId order by date_written desc;", conn);
                    cmd.Parameters.AddWithValue("@currentUserId", currentUserId);
                    cmd.Parameters.AddWithValue("@notePageId", notePageId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Note note = new Note();
                        note.NoteId = Convert.ToInt32(reader["id"]);
                        note.CurrentUserId = Convert.ToInt32(reader["current_user_id"]);
                        note.ProfileId = Convert.ToInt32(reader["other_user_id"]);
                        note.Text = Convert.ToString(reader["note"]);
                        note.DateSent = Convert.ToDateTime(reader["date_written"]);

                        Notes.Add(note);
                    }
                    reader.Close();
                }

                return Notes;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
