using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class Category
    {
        public int CatId { get; set; }
        public string Name { get; set; }

        public IList<SubCategory> SubCategories { get; set; }


    }
}