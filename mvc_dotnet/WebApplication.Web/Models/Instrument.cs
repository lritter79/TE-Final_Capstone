using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Instrument
    {
        public string Name { get; set; }
        public Instrument(string Name)
        {
            this.Name = Name;
        }
    }
}



