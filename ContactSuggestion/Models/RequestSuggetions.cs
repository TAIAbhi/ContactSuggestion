using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class RequestSuggetions
    {
        public int uID { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int microcategoryId { get; set; }
        [Display(Name = "Location")]
        public string locationName { get; set; }
        public int cityId { get; set; }
        public int platform { get; set; }
        public string comments { get; set; }
        public int contactId { get; set; }
        public DateTime addedWhen { get; set; }

        public string categoryName { get; set; }
        public string subcategoryName { get; set; }

        [Display(Name = "Sub Category")]
        public string microcategoryName { get; set; }

        public string cityName { get; set; }
        public string contactName { get; set; }

        public string sourceName { get; set; }
        public int locationid { get; set; }
        public IList<RequestSuggetions> RequestSuggetionsList { get; set; }
    }
}