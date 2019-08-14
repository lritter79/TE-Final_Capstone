using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class ConversationModel
    {
        public List<Message> Messages { get; set; }

        public string Sender { get; set; }
        public string Receiver { get; set; }
        public User CurrentUser { get; set; }

        [Required]
        [Display(Name = "Reply")]
        public string Reply{ get; set; }

    }
}
