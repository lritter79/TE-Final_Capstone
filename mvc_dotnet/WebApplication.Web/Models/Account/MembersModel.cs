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
    }
}
