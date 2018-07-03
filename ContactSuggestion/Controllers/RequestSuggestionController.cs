using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;
using BAL;
using System.Data;

namespace ContactSuggestion.Controllers
{
    public class RequestSuggestionController : Controller
    {
        // GET: RequestSuggestion

        [HttpPost]
        public JsonResult MicroCateLookup(string prefix, string subCateId)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetMicroCategory(Convert.ToInt32(subCateId), null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name")

             }).ToList();

            var location = (from loc in items
                            where loc.Name.ToUpper().Contains(prefix.ToUpper())
                            select new
                            {
                                label = loc.Name.Replace('©', '~'),
                                val = loc.MicroId
                            }).ToList();
            return Json(location);
        }
        [HttpPost]
        public JsonResult locationLookup(string prefix, string cityId)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty,Convert.ToInt32(cityId));
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = row.Field<string>("LocationName") != null ? row.Field<string>("LocationName") : "",
                 Area = row.Field<string>("Area"),
                 City = row.Field<string>("City"),
                 Suburb = row.Field<string>("Suburb") != null ? row.Field<string>("Suburb") : "",

             }).ToList();

            var location = (from loc in items
                            where loc.LocationName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || loc.Suburb.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                            select new
                            {
                                label = string.IsNullOrEmpty(loc.LocationName.Trim()) ? loc.Suburb.Trim() : loc.LocationName.Trim() + " - " + loc.Suburb.Trim(),
                                val = loc.LocationId
                            }).ToList();
            return Json(location);
        }
        public  ActionResult SendRequest(int  cityId, int platform, int subcatid, string contactnumber)
        {
            UserDetails objUserDetails = new UserDetails();
            DataTable dtRequestSuggestion = new DataTable();
            DataTable dtCategory = new DataTable();
            DataTable dtContactId = new DataTable();
            dtCategory = objUserDetails.GetSubCategory(null, subcatid, null).Tables[0];
            if(dtCategory.Rows.Count==0)
            {
                return View();
            }
            int CatId = Convert.ToInt32(dtCategory.Rows[0]["CatId"]);
            dtContactId = objUserDetails.GetContacts(null, null, contactnumber);
            if (dtContactId.Rows.Count == 0)
            {
                return View();
            }
            int  ContactId = Convert.ToInt32(dtContactId.Rows[0]["ContactId"]);
            RequestSuggetions objRequestSugg = new RequestSuggetions();
            objRequestSugg.cityId = cityId;
            objRequestSugg.platform = platform;
            objRequestSugg.categoryId = CatId;
            objRequestSugg.subCategoryId = subcatid;
            objRequestSugg.contactId = ContactId;        
            return View(objRequestSugg);
        }
        [HttpPost]
        public  ActionResult SendRequest([Bind(Include = "categoryId,subCategoryId,microcategoryId,locationName,cityId,platform,comments,contactId")] RequestSuggetions reqSug)
        {
            if (ModelState.IsValid)
            {
                UserDetails objUserDetails = new UserDetails();
                if (objUserDetails.SaveRequestSuggestion(reqSug.uID, reqSug.categoryId, reqSug.subCategoryId, reqSug.microcategoryId, reqSug.locationName, reqSug.cityId, reqSug.comments, reqSug.contactId, reqSug.platform))
                {
                    TempData["Success"] = "Added Successfully!";
                }
                else
                {
                    TempData["Success"] = "Some issue found!";
                }
            }
            return View();
        }

    }
}