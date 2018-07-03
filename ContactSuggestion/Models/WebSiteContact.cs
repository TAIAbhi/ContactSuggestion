using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace ContactSuggestion.Models
{
    public class WebSiteContact
    {

        public int ContactId { get; set; }
        public int SourceId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Location3 { get; set; }
        [Display(Name = "Professional Details")]
        public string Comments { get; set; }      

        [Display(Name = "Contact Level Understating")]
        public int ContactLevelUnderstating { get; set; }
        public int Notification { get; set; }
        [Display(Name = "Contact Details Added")]
        public bool? IsContactDetailsAdded { get; set; }
    }
}