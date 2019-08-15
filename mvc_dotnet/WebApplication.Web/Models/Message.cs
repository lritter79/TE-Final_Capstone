using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Message
    {

        public Message(string text, DateTime date, int sender, int receiver)
        {
            Text = text;
            DateSent = date;
            SenderId = sender;
            ReceiverId = receiver;
            
        }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public bool isRead {get; set;}
            
    }
}
