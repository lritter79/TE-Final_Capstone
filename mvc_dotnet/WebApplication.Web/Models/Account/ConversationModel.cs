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

        public string OtherUsername { get; set; }
        public string Receiver { get; set; }
        public User CurrentUser { get; set; }
        public int OtherUserId { get
            {
                int SenderId = -1;
                foreach (Message message in Messages)
                {
                    if (CurrentUser.Username == Receiver)
                    {
                        SenderId = message.SenderId;
                        break;
                    }
                }

                return SenderId;
            }
        }

        [Required]
        [Display(Name = "Reply")]
        public string Reply{ get; set; }

    }
}
