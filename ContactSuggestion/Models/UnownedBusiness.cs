using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class UnownedBusiness
    {

        public int BUID { get; set; }
        public string Category { get; set; }
        public string TTD { get; set; }
        public int SourceId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Mon { get; set; }
        public string Tue { get; set; }
        public string Wed { get; set; }
        public string Thu { get; set; }
        public string Fri { get; set; }
        public string Sat { get; set; }
        public string Sun { get; set; }
        public string BusinessType { get; set; }

        public bool Verified { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }

        public string RequestReservation { get; set; }
        public string UID { get; set; }
    
     
    }
}