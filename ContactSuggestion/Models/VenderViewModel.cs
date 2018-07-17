using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace ContactSuggestion.Models
{
    public class VenderViewModel
    {
      public int VendorId { get; set; }
      public string LoginToken  { get; set; }
      public string Password  { get; set; }
      public string VendorName  { get; set; }
      public int CatId  { get; set; }
      public int SubCatId  { get; set; }
      public int ? MicroCatId  { get; set; }
      public string Address  { get; set; }
      public int ? LocationId  { get; set; }
      public string ContactNumber  { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
      public string Email  { get; set; }     
      public int ContactId  { get; set; }
      public int DocumentId { get; set; }
      public int [] DocumentIds { get; set; }
        public string DocDetail { get; set; }
        public string DocURL { get; set; }
        

    }
}