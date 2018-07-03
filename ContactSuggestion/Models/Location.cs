using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Suburb { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }

    }
}