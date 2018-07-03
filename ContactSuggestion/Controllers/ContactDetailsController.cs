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
    [CustomAuthorize("2", "1")]
    public class ContactDetailsController : Controller
    {
        private void FillCityDrodown()
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtContact = objUserDetails.GetCity();
            IList<CityModel> items = dtContact.AsEnumerable().Select(row =>
             new CityModel
             {
                 CityId = row.Field<int>("CityId"),
                 CityName = row.Field<string>("CityName")
             }).ToList();

            // var list = new SelectList(items, "CatId", "Name");
            var list = new SelectList(items, "CityId", "CityName");
            ViewData["City"] = list;
        }
        // GET: ContactDetails
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Details()
        {
            FillCityDrodown();
            return PartialView("ModalPopUp");
        }
        [HttpPost]
        public ActionResult SaveLocation(string Suburb, string LocationName, string City)
        {
            if (!string.IsNullOrEmpty(Suburb.Trim()) && !string.IsNullOrEmpty(LocationName.Trim()) && !string.IsNullOrEmpty(City))
            {
                UserDetails objUserDetail = new UserDetails();
                if (objUserDetail.SaveLocation(Suburb.Trim(), LocationName.Trim(),Convert.ToInt32(City)))
                {
                    return Json(new
                    {

                        isRedirect = true,
                        Message = ""
                    });
                }
                else
                {

                    return Json(new
                    {
                        Message = "Duplicate location!."
                    });
                }

            }

            return Json(new
            {
                Message = "Please enter location details."
            });
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


            ViewBag.CatId = list;

        }

        private void FillSubCategoryDrodown(string catId)
        {

            DataTable dSubCate = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dSubCate = objUserDetails.GetSubCategory(Convert.ToInt32(catId), null, null).Tables[0];
            IList<SubCategory> items = dSubCate.AsEnumerable().Select(row =>
             new SubCategory
             {
                 SubCatId = row.Field<int>("SubCatId"),
                 Name = row.Field<string>("Name")
             }).ToList();

            var list = new SelectList(items, "SubCatId", "Name");


            ViewBag.CatId = list;

        }
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


            var subCate = new SelectList(items, "SubCatId", "Name");

            return Json(new SelectList(subCate, "Value", "Text"));
        }

        public ContactSuggestions GetContact()
        {
            ContactSuggestions objContact = null;
            if (Session["UserDetails"] != null)
            {
                objContact = new ContactSuggestions();
                ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
                UserDetails objUserDetails = new UserDetails();
                DataTable dtContact = new DataTable();
                dtContact = objUserDetails.GetContacts(Convert.ToInt32(objSource.ContactId), null,"");
                if (dtContact.Rows.Count > 0)
                {
                    objContact.SourceId = Convert.ToInt32(dtContact.Rows[0]["SourceId"]);
                    objContact.Source = Convert.ToString(dtContact.Rows[0]["Name"]);
                    objContact.ContactId = Convert.ToInt32(dtContact.Rows[0]["ContactId"]);
                    objContact.Contact = Convert.ToString(dtContact.Rows[0]["ContactName"]);
                    objContact.Location4 = Convert.ToString(dtContact.Rows[0]["LocationId1"]);
                    objContact.Location5 = Convert.ToString(dtContact.Rows[0]["LocationId2"]);
                    objContact.Location6 = Convert.ToString(dtContact.Rows[0]["LocationId3"]);
                    objContact.ContactNumber = Convert.ToString(dtContact.Rows[0]["ContactNumber"]);
                    objContact.ContactComments = Convert.ToString(dtContact.Rows[0]["Comments"]);
                }
                ViewBag.ContactComments = objContact.ContactComments;
                ViewBag.Comments = objContact.Comments;
            }
            return objContact;
        }
        public IList<SubCategory> GetSubCate(int catId)
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetSubCategory(catId, null, null).Tables[0];
            IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                new SubCategory
                {
                    SubCatId = row.Field<int>("SubCatId"),
                    Name = row.Field<string>("Name"),
                    CatId = row.Field<int>("CatId"),
                    CommentsToolTip = row.Field<string>("CommentsToolTip"),
                    MicroCategoryToolTip = row.Field<string>("CommentsToolTip")

                }).ToList();
            return items;
        }
        public ActionResult Create()
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();
            if (TempData["Invalid"] == null)
            {
                ContactSuggestions objContact = GetContact();
                if (objContact != null)
                {
                    ViewBag.ContactComments = objContact.ContactComments;
                    ViewBag.Comments = objContact.Comments;
                    if (TempData["Success"] == null)
                    {
                        TempData["Success"] = null;
                    }
                    FillCategoryDrodown();


                    dtContact = objUserDetails.GetCategory(null, null).Tables[0];
                    IList<Category> items = dtContact.AsEnumerable().Select(row =>
                     new Category
                     {
                         CatId = row.Field<int>("CatId"),
                         Name = row.Field<string>("Name"),
                         SubCategories = GetSubCate(row.Field<int>("CatId"))


                     }).ToList();

                    // var list = new SelectList(items, "CatId", "Name");
                    objContact.Categories = items;

                    return View(objContact);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                ContactSuggestions objContact = (ContactSuggestions)TempData["Invalid"];
                dtContact = objUserDetails.GetCategory(null, null).Tables[0];
                IList<Category> items = dtContact.AsEnumerable().Select(row =>
                 new Category
                 {
                     CatId = row.Field<int>("CatId"),
                     Name = row.Field<string>("Name"),
                     SubCategories = GetSubCate(row.Field<int>("CatId"))


                 }).ToList();

                // var list = new SelectList(items, "CatId", "Name");
                objContact.Categories = items;               
                return View(objContact);
            }
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
                 Name = row.Field<string>("Name").Contains('©')? row.Field<string>("Name").Split('©')[0].ToString().Trim() : row.Field<string>("Name").Trim()

             }).ToList();
            var location = (from loc in items
                            select new
                            {
                                label = loc.Name.Trim()
                            }).Distinct().ToList();
            return Json(location);
        }

        [HttpPost]
        public JsonResult MicrocategoryBoxAutolookup(int subCatId, string firstSplitName,string prefix)
        {
            if (string.IsNullOrEmpty(firstSplitName))
            {
                return AutoCompleteMicroCate(prefix, Convert.ToString(subCatId));
            }
            else
            {
                DataTable dtLocation = new DataTable();
                UserDetails objUserDetails = new UserDetails();
                dtLocation = objUserDetails.GetMicroCategory(Convert.ToInt32(subCatId), null);
                IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
                 new MicroCategory
                 {
                     MicroId = row.Field<int>("MicroId"),
                     Name = row.Field<string>("Name").Contains('©') ? row.Field<string>("Name").Split('©')[0].ToString().Trim() : row.Field<string>("Name").Trim(),
                     SubCateName = row.Field<string>("Name").Contains('©') ? row.Field<string>("Name").Split('©')[1].ToString().Trim() : row.Field<string>("Name").Trim()
                 }).ToList().Where(a => a.Name == firstSplitName).ToList();

                var location = (from loc in items
                                where loc.SubCateName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                                select new
                                {
                                    label = loc.SubCateName.Trim(),
                                    val = loc.MicroId
                                }).ToList();
                return Json(location);
            }
        }
        // POST: ContactSug/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "SourceId,ContactId,Source,Contact,CatId,SubCategory,Microcategory,CitiLevelBusiness,BusinessName,BusinessContact,Location1,Location2,Location3,Comments,Location4,Location5,Location6,ContactComments,SubCategoryId,Details,IsAChain")] ContactSuggestions contactSugg, string mydrop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["Invalid"] = null;
                    ContactSuggestions objContact = GetContact();
                    if (objContact != null)
                    {
                        string location = contactSugg.Location1 == null ? "" : contactSugg.Location1.Trim();
                        bool isValidNumber = false;
                        UserDetails objUserDetails = new UserDetails();
                        if (!string.IsNullOrEmpty(contactSugg.BusinessContact))
                        {
                            contactSugg.BusinessContact = objUserDetails.MobileFormat(contactSugg.BusinessContact);
                            Int64 isNumber = 0;
                            Int64.TryParse(contactSugg.BusinessContact, out isNumber);
                            if((contactSugg.BusinessContact.Length == 10 && isNumber > 0))
                            {
                                isValidNumber = true;
                            }
                        }                      
                        if (string.IsNullOrEmpty(contactSugg.BusinessContact)|| isValidNumber ==true)
                        {
                            contactSugg.Microcategory = mydrop + " © " + contactSugg.Details;
                            if (objUserDetails.SaveContactSuggestions(contactSugg.SourceId, contactSugg.ContactId, contactSugg.CatId.ToString(), contactSugg.SubCategoryId.ToString(), contactSugg.Microcategory, contactSugg.BusinessName, contactSugg.CitiLevelBusiness, contactSugg.BusinessContact, location, contactSugg.Location2, contactSugg.Location3, contactSugg.Comments, "", contactSugg.Location4, contactSugg.Location5, contactSugg.Location6, contactSugg.ContactComments,contactSugg.IsAChain,"Web",1,1, false))
                            {
                                TempData["Success"] = "Added Successfully!";
                            }
                        }
                        else
                        {
                            TempData["Success"] = "Invalid Business contact number!";
                            TempData["Invalid"] = contactSugg;

                            return RedirectToAction("Create", contactSugg);

                        }
                        return RedirectToAction("Create", objContact);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                return View();
                // TODO: Add insert logic here

                // return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["Success"] = ex.Message;
                return RedirectToAction("Create", contactSugg);

            }
        }
        public ActionResult LogOff()
        {
            Globals.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty,1);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = row.Field<string>("LocationName")!=null ? row.Field<string>("LocationName"):"",
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
        [HttpPost]
        public JsonResult AutoCompleteMicroCate(string prefix, string subCateId)
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
                                label = loc.Name.Replace('©','~'),
                                val = loc.MicroId
                            }).ToList();
            return Json(location);
        }
        [HttpPost]
        public JsonResult AutoCompleteSuburb(string prefix, int city)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetSuburb(city);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 // LocationId = row.Field<int>("LocationId"),
                 Suburb = row.Field<string>("Suburb"),
                 LocationId = 1
             }).ToList();

            var location = (from loc in items
                            where loc.Suburb.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                            select new
                            {
                                label = loc.Suburb,
                                val = loc.LocationId
                            }).ToList();
            return Json(location);
        }
        [HttpPost]
        public JsonResult AutoCompleteLocationWithSuburb(string prefix, string suburb, int city)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, suburb, prefix, city);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = row.Field<string>("LocationName") == null ? "" : row.Field<string>("LocationName"),
                 Area = row.Field<string>("Area"),
                 City = row.Field<string>("City"),
                 Suburb = row.Field<string>("Suburb") == null ? "" : row.Field<string>("Suburb")
             }).ToList();

            var location = (from loc in items
                            where loc.LocationName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || loc.Suburb.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                            select new
                            {
                                label = loc.LocationName.Trim(),
                                val = loc.LocationId
                            }).ToList();
            return Json(location);
        }
    }
}