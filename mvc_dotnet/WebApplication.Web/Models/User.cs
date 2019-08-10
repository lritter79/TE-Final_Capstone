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
            this.ListOfComposers = new List<Composer>();
            this.SelfDescription = "";
            this.IsPublic = true;
        }

        /// <summary>
        /// The user's id.
        /// </summary>
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

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Age is required. Min age at least 18")]
        // still needs a way to aasert that the user registering is 18 or older
        [Display(Name = "Age")]
        public int Age { get {
                return 21;
            } }

        [Required(ErrorMessage = "Home city is required.")]
        [Display(Name = "Home City")]
        public string HomeCity { get; set; }

        [Required(ErrorMessage = "Home state is required.")]
        [Display(Name = "Home State")]
        public string HomeState { get; set; }

        [Display(Name = "Bio")]
        public string SelfDescription { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// The user's role.
        /// <summary>
        /// The user's salt.
        /// </summary>
        [Required]
        public string Salt { get; set; }

        public bool IsPublic { get; set; }

        [Display(Name = "Intrument(s) played. Choose up to three. If voice is entered please give range.")]

        public List<Instrument> ListOfInstruments { get; set; }


        [Display(Name = "Touring cities this year")]
        public List<Place> ListOfPlaces { get; set; }

        [Display(Name = "Favorite Composers")]
        public List<Composer> ListOfComposers { get; set; }

        /// <summary>
        /// </summary>
        public string Role { get; set; }
    }
}
