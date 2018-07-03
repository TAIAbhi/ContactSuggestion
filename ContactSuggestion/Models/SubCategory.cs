using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class SubCategory
    {
        public int SubCatId { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public string MicroCategoryToolTip { get; set; }
        public string CommentsToolTip { get; set; }
    }
}