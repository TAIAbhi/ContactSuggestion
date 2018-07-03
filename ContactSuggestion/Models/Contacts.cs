using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class Contacts
    {
        public int ContactId { get; set; }
        public int SourceId { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }


        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Location3 { get; set; }
        [Display(Name = "Professional Details")]
        public string Comments { get; set; }
        public List<Contacts> ContactsList { get; set; }

        public string SoruceName { get; set; }
        public int ContactSourceId { get; set; }
        [Display(Name = "Contact Level Understating")]
        public int ContactLevelUnderstating { get; set; }
        public int Notification { get; set; }
        [Display(Name = "Contact Details Added")]
        public bool ? IsContactDetailsAdded { get; set; }

    }
}