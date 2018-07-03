using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class Feedbacks
    {
        public int UID { get; set; }
        public string MobileNumber { get; set; }
        [Display(Name = "Please select the aspect of the app for feedback")]
        public string Aspect { get; set; }
        [Display(Name = "1. Please select the aspect of the app for feedback")]
        public string Aspect1 { get; set; }
        [Display(Name = "2. Please select the aspect of the app for feedback")]
        public string Aspect2 { get; set; }
        [Display(Name = "3. Please select the aspect of the app for feedback")]
        public string Aspect3 { get; set; }
        [Display(Name = "4. Please select the aspect of the app for feedback")]
        public string Aspect4 { get; set; }
        [Display(Name = "5. Please select the aspect of the app for feedback")]
        public string Aspect5 { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        public int Rating1 { get; set; }


        public int Rating2 { get; set; }

        public int Rating3 { get; set; }

        public int Rating4 { get; set; }
        public int Rating5 { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments1 { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments2 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments3 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments4 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments5 { get; set; }
      
    }
}