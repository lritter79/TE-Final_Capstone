<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Place
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public Place(string CityName, string StateName, DateTime FromDate, DateTime ToDate)
        {
            this.CityName = CityName;
            this.StateName = StateName;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
        }
    }
}


=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Place
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public Place(string CityName, string StateName, DateTime FromDate, DateTime ToDate)
        {
            this.CityName = CityName;
            this.StateName = StateName;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
        }
    }
}
>>>>>>> 556773158a5b0361be99a3ccc184136586f700d3
