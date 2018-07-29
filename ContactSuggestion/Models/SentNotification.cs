using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class SentNotification
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Source { get; set; }
        public string SourceNo { get; set; }
        public string HomeLocation { get; set; }
        public string WorkLocation { get; set; }
        public string CollegeSchoolLocation  { get; set; }
        public string ProfeDetails { get; set; }
        public bool AllowedRequestSuggestion { get; set; }
        public  int SourceId { get; set; }




    }
}