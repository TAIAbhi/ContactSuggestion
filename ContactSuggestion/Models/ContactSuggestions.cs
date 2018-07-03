using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class ContactSuggestions
    {
        public string UID { get; set; }
        public string Source { get; set; }
        public string Contact { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Category")]
        public int CatId { get; set; }

        public int SourceId { get; set; }
        public int ContactId { get; set; }
        public string Category { get; set; }
        [Display(Name = "Sub Category")]
        public string SubCategory { get; set; }

        [Display(Name = "Sub Category")]
        public string Microcategory { get; set; }
        [Display(Name = "Local")]
        public bool CitiLevelBusiness { get; set; }
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [Display(Name = "Business Contact")]
        public string BusinessContact { get; set; }
        [Display(Name = "Location")]
        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Location3 { get; set; }
        [Display(Name = "Business Details")]
        public string Comments { get; set; }

        [Display(Name = "Location1")]
        public string Location4 { get; set; }
        [Display(Name = "Location2")]
        public string Location5 { get; set; }
        [Display(Name = "Location3")]
        public string Location6 { get; set; }
        [Display(Name = "Professional Details")]
        public string ContactComments { get; set; }
        public IList<Category> Categories { get; set; }

        public Category MainCategory { get; set; }
        public int SubCategoryId { get; set; }
        public string SourceName { get; set; }
        public string ContactName { get; set; }
        [Display(Name = "Contact Level Understating")]
        public int ContactLevelUnderstating { get; set; }
        public int Notification { get; set; }
        [Display(Name = "Contact Details Added")]
        public bool? IsContactDetailsAdded { get; set; }
        [Display(Name = "Contact Details Added", Prompt="Details")]
        public string Details { get; set; }
        [Display(Name = "Is A Chain")]
        public bool  IsAChain { get; set; }

        public int locationId { get; set; }
        public int  ? microcateId { get; set; }
        public string AddedWhen { get; set; }
        public string CityName { get; set; }

        public bool ? VendorIsVerified { get; set; }
        public bool ? IsValid { get; set; }
        public bool  ? ShowMaps { get; set; }
        public bool ? IsChain { get; set; }
        public bool? isLocal { get; set; }
        public string Platform { get; set; }
        public DateTime ? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<ContactSuggestions> ContactSuggestionsList { get; set; }

        public string IsVerified { get; set; }
        public string IsActive { get; set; }       
        public string Chain { get; set; }
        public string Local { get; set; }

        public string Maps { get; set; }
        public string DataActive { get; set; }
        public string GoogleMaps { get; set; }
        
    }
}