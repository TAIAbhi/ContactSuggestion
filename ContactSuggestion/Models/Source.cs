using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ContactSuggestion.Models
{
    public class Source
    {
        public int SourceId { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; }

        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string MyPassword { get; set; }
        [DataType(DataType.Password)]
        public string FriendsPassword { get; set; }
        public string ContactName { get; set; }
        public int Role { get; set; }
        public string LocationId1 { get; set; }
        public string LocationId2 { get; set; }
        public string LocationId3 { get; set; }
        public bool IsInterns { get; set; }
        public bool SkipVideo { get; set; }
        public bool EveryVideoCheck { get; set; }
        public int SourceType { get; set; }

    }
}