using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class MembersModel
    {
        public List<User> Members { get; set; }

        
        [Display(Name = "Note")]
        public string Note { get; set; }

        public User CurrentUser { get; set; }

        public List<string> Flirts { get
            {
                List<string> flirts = new List<string> {
                    "Meet me in Paris, we can make music...",
                    "What's your instrument?",
                    "You feel like compsing anything?",
                    "Hey there hautbois ;)",
                    "Je suis chaud",
                    "Modern instruments are so overrated, don't you agree",
                    "Oh baby, yeah, all night long ... is how long it takes to tune my lute",
                    "yerrr you tryna grab a drink"
                };
                return flirts;
                
            } }

    }
}
