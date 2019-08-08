﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class User
    {

        public User()
        {
            this.ListOfInstruments = new List<Instrument>();
            this.ListOfPlaces = new List<Place>();
        }
        /// <summary>
        /// The user's id.
        /// </summary>
        /// 
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        [Required(ErrorMessage = "*Required Field")]
        [DataType(DataType.EmailAddress, ErrorMessage = "You must enter a valid email address")]
        public string Email { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Birth Date is required.Min age at least 18, Max age < 110")]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Home City")]
        public string HomeCity { get; set; }

        [Required]
        [Display(Name = "Home State")]
        public string HomeState { get; set; }

        [Required]
        public string SelfDescription { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's salt.
        /// </summary>
        [Required]
        public string Salt { get; set; }

        [Required]
        [Display(Name = "Intrument(s) played. Choose up to three. If voice is entered please give range.")]

        public List<Instrument> ListOfInstruments { get; set; }
        

        [Display(Name = "Touring cities this year")]
        public List<Place> ListOfPlaces { get; set; }
    }
}
