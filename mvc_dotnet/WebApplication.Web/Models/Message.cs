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
<<<<<<< HEAD
            receiverId = receiver;
=======
            ReceiverId = receiver;
>>>>>>> bce154abb5b3aed8e9db83f2e120b1ff61db8d00
        }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public int SenderId { get; set; }

<<<<<<< HEAD
        public int receiverId { get; set; }
=======
        public int ReceiverId { get; set; }
>>>>>>> bce154abb5b3aed8e9db83f2e120b1ff61db8d00
            
    }
}
