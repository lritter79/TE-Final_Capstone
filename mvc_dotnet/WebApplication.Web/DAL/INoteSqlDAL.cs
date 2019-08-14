using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface INoteSqlDAL
    {
        void AddNote(Note note);
        void DeleteNote(int noteId);
        List<Note> GetNotes(int currentUserId, int notePageId);
    }
}
