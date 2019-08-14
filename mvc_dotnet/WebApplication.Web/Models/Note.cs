using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Note
    {

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public int CurrentUserId { get; set; }

        public int ProfileId { get; set; }

        public int NoteId { get; set; }
            
    }
}
