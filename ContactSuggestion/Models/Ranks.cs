using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class Ranks
    {
        public Int64 Rank { get; set; }
        public string ContactName { get; set; }
        public int Point { get; set; }
        public string ContactNumber { get; set; }
    }
}