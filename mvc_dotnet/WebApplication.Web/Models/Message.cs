using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Message
    {

        public Message(string text, DateTime date, int sender, int reciever)
        {
            Text = text;
            DateSent = date;
            SenderId = sender;
            RecieiverId = reciever;
        }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public int SenderId { get; set; }

        public int RecieiverId { get; set; }
            
    }
}
