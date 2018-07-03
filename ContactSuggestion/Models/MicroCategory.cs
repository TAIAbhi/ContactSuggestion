using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class MicroCategory
    {
        public int MicroId { get; set; }
        public int SubCateId { get; set; }
        [Display(Name = "Micro Category")]
        public string Name { get; set; }
        [Display(Name = "Sub Category")]
        public string SubCateName { get; set; }
        
        public int AddedBy { get; set; }
        public IList<Category> Categories { get; set; }

        public Category MainCategory { get; set; }

    }
}