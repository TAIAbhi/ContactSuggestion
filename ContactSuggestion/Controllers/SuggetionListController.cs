using BAL;
using ContactSuggestion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactSuggestion.Controllers
{
    public class SuggetionListController : Controller
    {
        // GET: SuggetionList
        public ActionResult Index(string Category)
        {//string option, string search, int? pageNumber
            ContactSuggestions objContactSuggestion = new ContactSuggestions();
           // List<ContactSuggestions> objList = new List<ContactSuggestions>();
           // objContactSuggestion.ContactSuggestionsList = objList;
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetSuggestionList(null, null, null, null, "", null, "", null, "", null, "", null, null, null, null, null, null).Tables[0];
            IList<ContactSuggestions> items = dtLocation.AsEnumerable().Select(row =>
             new ContactSuggestions
             {
                 UID = row.Field<int>("UID").ToString(),
                 BusinessName= row.Field<string>("BusinessName"),
                 BusinessContact = row.Field<string>("BusinessContact"),
                 CityName = row.Field<string>("CityName"),
                 Location1 = row.Field<string>("LocationId1"),
                 Microcategory = row.Field<string>("Microcategory"),
                 SubCategory = row.Field<string>("SubCategoryName"),
                 Category = row.Field<string>("CategoryName"),
                 Source = row.Field<string>("SourceName"),
                 Contact = row.Field<string>("ContactName"),
                 AddedWhen = row.Field<string>("AddedWhen"),
                 Platform = row.Field<string>("Platform"),
                  Chain = row.Field<string>("IsAChain"),
                 Local = row.Field<string>("CitiLevelBusiness"),
                 IsVerified = row.Field<string>("VendorIsVerified"),
                 IsActive= row.Field<string>("IsValid"),
                 Maps= row.Field<string>("ShowMaps"),
                 DataActive= row.Field<string>("IsActive"),
                 GoogleMaps = "https://www.google.com/maps/search/"+ row.Field<string>("BusinessName").Trim()+"+" +( row.Field<string>("LocationId1").Split('-').Length> 1? row.Field<string>("LocationId1").Split('-')[1] : row.Field<string>("LocationId1")).Trim()


             }).ToList();
            objContactSuggestion.ContactSuggestionsList = items;
            FillCategoryDrodown();
            City();
            BindSource();
            Platform();
            return View(objContactSuggestion);
        }
        private void FillCategoryDrodown()
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtContact = objUserDetails.GetCategory(null, null).Tables[0];
            IList<Category> items = dtContact.AsEnumerable().Select(row =>
             new Category
             {
                 CatId = row.Field<int>("CatId"),
                 Name = row.Field<string>("Name")
             }).ToList();

            var list = new SelectList(items, "CatId", "Name");
            ViewData["Category"] = list;
        }

        [HttpPost]
        public JsonResult GetSubCate(string id)
        {
            int cid = 0;
            int.TryParse(id, out cid);
            DataTable dSubCate = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dSubCate = objUserDetails.GetSubCategory(Convert.ToInt32(cid), null, null).Tables[0];
            IList<SubCategory> items = dSubCate.AsEnumerable().Select(row =>
             new SubCategory
             {
                 SubCatId = row.Field<int>("SubCatId"),
                 Name = row.Field<string>("Name")
             }).ToList();
            var location = (from loc in items
                            select new
                            {
                                label = loc.Name.Trim(),
                                val = loc.SubCatId
                            }).ToList();
            return Json(location);


        }
        [HttpPost]
        public JsonResult BindDropdown(int subCatId)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetMicroCategory(Convert.ToInt32(subCatId), null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name").Contains('©') ? row.Field<string>("Name").Split('©')[0].ToString().Trim() : row.Field<string>("Name").Trim()

             }).ToList();
            var location = (from loc in items
                            select new
                            {
                                label = loc.Name.Trim(),
                                val = loc.MicroId
                            }).ToList();
            return Json(location);
        }
       
        public void  BindSource()
        {
            DataTable dtSoruce = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtSoruce = objUserDetails.GetSoruce(null, false);
            IList<Source> items = dtSoruce.AsEnumerable().Select(row =>
             new Source
             {

                 SourceId = row.Field<int>("SourceId"),
                 Name = row.Field<string>("Name") + "(" + row.Field<string>("Mobile") + ")"


             }).ToList();
          
            var list = new SelectList(items, "SourceId", "Name");
            ViewData["Source"] = list;
        }

        [HttpPost]
        public JsonResult ViewMyContacts(string srcMobile)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetContactBySourceContact(srcMobile);
            IList<Contacts> items = dtLocation.AsEnumerable().Select(row =>
             new Contacts
             {
                 ContactName = row.Field<string>("ContactName"),
                 ContactNumber = row.Field<string>("ContactNumber"),
                 SoruceName = row.Field<string>("Name"),
                 Location1 = row.Field<string>("LocationId1"),
                 Location2 = row.Field<string>("LocationId2"),
                 Location3 = row.Field<string>("LocationId3"),
                 ContactId = row.Field<int>("ContactId")
             }).ToList();

            var location = (from loc in items
                            select new
                            {
                                label = loc.ContactName.Trim(),
                                val = loc.ContactId
                            }).ToList();
            return Json(location);
        }
        [HttpPost]
        public JsonResult AutoCompleteLocationWithSuburb(int city)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, "", "", city);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = row.Field<string>("LocationName"),
                 Area = row.Field<string>("Area"),
                 City = row.Field<string>("City"),
                 Suburb = row.Field<string>("Suburb")

             }).ToList();

            var location = (from loc in items                          
                            select new
                            {
                                label = loc.LocationName.Trim(),
                                val = loc.LocationId
                            }).ToList();
            return Json(location);
        }      
        public void City()
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetCity();
            IList<CityModel> items = dtLocation.AsEnumerable().Select(row =>
             new CityModel
             {
                 CityId = row.Field<int>("CityId"),
                 CityName = row.Field<string>("CityName")                

             }).ToList();
            var list = new SelectList(items, "CityId", "CityName");         
            ViewData["CityName"] = list;
        }
        public void Platform()
        {
            List<SelectListItem> li = new List<SelectListItem>();
           
            li.Add(new SelectListItem { Text = "Web", Value = "Web" });          
            
            ViewData["Platform"] = li;
            
        }
    }

}