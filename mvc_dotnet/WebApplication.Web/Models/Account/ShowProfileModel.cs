using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class ShowProfileModel
    {
        public List<Note> Notes { get; set; }
        public User User { get; set; }



    }
}
