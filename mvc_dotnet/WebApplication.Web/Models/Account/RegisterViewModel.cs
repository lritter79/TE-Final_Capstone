using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Your username can only be 20 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date, ErrorMessage = " You must enter a valid date.")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender")]
        public Int32 Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Seeking")]
        public Int32 Seeking { get; set; }

        [Required]
        [Display(Name = "Home City")]
        public string HomeCity { get; set; }

        [Required]
        [Display(Name = "Home State")]
        public string HomeState { get; set; }

        [Display(Name = "Description of Self")]
        public string SelfDescription { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public static List<SelectListItem> StateList = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Alabama", Value = "AL" },
            new SelectListItem() { Text = "Alaska", Value = "AK" },
            new SelectListItem() { Text = "Arizona", Value = "AZ" },
            new SelectListItem() { Text = "Arkansas", Value = "AR" },
            new SelectListItem() { Text = "California", Value = "CA" },
            new SelectListItem() { Text = "Colorado", Value = "CO" },
            new SelectListItem() { Text = "Connecticut", Value = "CT" },
            new SelectListItem() { Text = "Delaware", Value = "DE" },
            new SelectListItem() { Text = "Florida", Value = "FL" },
            new SelectListItem() { Text = "Georgia", Value = "GA" },
            new SelectListItem() { Text = "Hawaii", Value = "HI" },
            new SelectListItem() { Text = "Idaho", Value = "ID" },
            new SelectListItem() { Text = "Illinois", Value = "IL" },
            new SelectListItem() { Text = "Indiana", Value = "IN" },
            new SelectListItem() { Text = "Iowa", Value = "IA" },
            new SelectListItem() { Text = "Kansas", Value = "KS" },
            new SelectListItem() { Text = "Kentucky", Value = "KY" },
            new SelectListItem() { Text = "Louisiana", Value = "LA" },
            new SelectListItem() { Text = "Maine", Value = "ME" },
            new SelectListItem() { Text = "Maryland", Value = "MD" },
            new SelectListItem() { Text = "Massachusetts", Value = "MA" },
            new SelectListItem() { Text = "Michigan", Value = "MI" },
            new SelectListItem() { Text = "Minnesota", Value = "MN" },
            new SelectListItem() { Text = "Mississippi", Value = "MS" },
            new SelectListItem() { Text = "Missouri", Value = "MO" },
            new SelectListItem() { Text = "Montana", Value = "MT" },
            new SelectListItem() { Text = "Nebraska", Value = "NE" },
            new SelectListItem() { Text = "Nevada", Value = "NV" },
            new SelectListItem() { Text = "New Hampshire", Value = "NH" },
            new SelectListItem() { Text = "New Jersey", Value = "NJ" },
            new SelectListItem() { Text = "New Mexico", Value = "NM" },
            new SelectListItem() { Text = "New York", Value = "NY" },
            new SelectListItem() { Text = "North Carolina", Value = "NC" },
            new SelectListItem() { Text = "North Dakota", Value = "ND" },
            new SelectListItem() { Text = "Ohio", Value = "OH" },
            new SelectListItem() { Text = "Oklahoma", Value = "OK" },
            new SelectListItem() { Text = "Oregon", Value = "OR" },
            new SelectListItem() { Text = "Pennsylvania", Value = "PA" },
            new SelectListItem() { Text = "Rhode Island", Value = "RI" },
            new SelectListItem() { Text = "South Carolina", Value = "SC" },
            new SelectListItem() { Text = "South Dakota", Value = "SD" },
            new SelectListItem() { Text = "Tennessee", Value = "TN" },
            new SelectListItem() { Text = "Texas", Value = "TX" },
            new SelectListItem() { Text = "Utah", Value = "UT" },
            new SelectListItem() { Text = "Vermont", Value = "VT" },
            new SelectListItem() { Text = "Virginia", Value = "VA" },
            new SelectListItem() { Text = "Washington", Value = "WA" },
            new SelectListItem() { Text = "West Virginia", Value = "WV" },
            new SelectListItem() { Text = "Wisconsin", Value = "WI" },
            new SelectListItem() { Text = "Wyoming", Value = "WY" }
        };

       
    }
}
