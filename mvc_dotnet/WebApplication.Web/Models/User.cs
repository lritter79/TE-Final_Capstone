using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class User
    {
        /// <summary>
        /// The user's id.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Birth Date is required.Min age at least 18, Max age < 110")]
        [Display(Name = "Date of Birth")]
        public string BirthDate { get; set; }

        [Required]
        [Display(Name = "Home City")]
        public string HomeCity { get; set; }

        [Required]
        [Display(Name = "Home State")]
        public string HomeState { get; set; }

        [Required]
        public string SelfDescription { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The user's salt.
        /// </summary>
        [Required]
        public string Salt { get; set; }

        [Required]
        

       
    }
}
