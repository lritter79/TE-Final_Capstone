using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class BioUpdateModel
    {

        [Display(Name = "Self Description")]
        public string SelfDescription { get; set; }

        
       
    }
}
