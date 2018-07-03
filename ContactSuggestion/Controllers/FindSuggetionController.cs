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
    public class FindSuggetionController : Controller
    {
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
        public IList<SubCategory> GetSubCate(int catId, string contact)
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetMySuggestionSubCategoryWise(catId, contact);
            IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                new SubCategory
                {
                    SubCatId = row.Field<int>("SubCatId"),
                    Name = row.Field<string>("Name"),
                    CatId = row.Field<int>("CatId")


                }).ToList();
            return items;
        }
        public IList<SubCategory> GetAllSubCate(int catId)
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetAllSuggestionSubCategoryWise(catId);
            IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                new SubCategory
                {
                    SubCatId = row.Field<int>("SubCatId"),
                    Name = row.Field<string>("Name"),
                    CatId = row.Field<int>("CatId")


                }).ToList();
            return items;
        }
        public ActionResult Suggestion(string catId, string SubCatId, string bussiness, string location , int ? microCat, bool ? isLocal)
        {
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            ContactSuggestions objContactSuggestions = new ContactSuggestions();
           // ViewBag.MySuggesstion = GetMySuggesstion();
            dtContact = objUserDetails.GetSuggestionFilters(Convert.ToInt32(catId), Convert.ToInt32(SubCatId),null, objSource.ContactId,"",null, bussiness, isLocal, location, microCat).Tables[0];

            IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                new ContactSuggestions
                {
                    ContactNumber = row.Field<string>("ContactNumber"),
                    BusinessName = row.Field<string>("BusinessName"),
                    BusinessContact = row.Field<string>("BusinessContact"),
                    Category = row.Field<string>("CategoryName"),
                    SubCategory = row.Field<string>("SubCategoryName"),
                    Microcategory = row.Field<string>("Microcategory"),
                    SourceName = row.Field<string>("SourceName"),
                    Location1 = row.Field<string>("LocationId1"),
                    CitiLevelBusiness = row.Field<bool>("CitiLevelBusiness"),
                    Comments = row.Field<string>("Comments"),
                    ContactName = row.Field<string>("ContactName"),
                    microcateId=  row.Field<int?>("MicrocategoryId"),
                    SubCategoryId = row.Field<int>("SubCategory")

                }).ToList();
            

            // return View(items.ToList());

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllSuggestion(string catId, string SubCatId, string bussiness, string location, int? microCat, bool? isLocal)
        {
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            ContactSuggestions objContactSuggestions = new ContactSuggestions();
            // ViewBag.MySuggesstion = GetMySuggesstion();
            dtContact = objUserDetails.GetSuggestionFilters(Convert.ToInt32(catId), Convert.ToInt32(SubCatId), null, null, "", null, bussiness, isLocal, location, microCat).Tables[0];

            IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                new ContactSuggestions
                {
                    ContactNumber = row.Field<string>("ContactNumber"),
                    BusinessName = row.Field<string>("BusinessName"),
                    BusinessContact = row.Field<string>("BusinessContact"),
                    Category = row.Field<string>("CategoryName"),
                    SubCategory = row.Field<string>("SubCategoryName"),
                    Microcategory = row.Field<string>("Microcategory"),
                    SourceName = row.Field<string>("SourceName"),
                    Location1 = row.Field<string>("LocationId1"),
                    CitiLevelBusiness = row.Field<bool>("CitiLevelBusiness"),
                    Comments = row.Field<string>("Comments"),
                    ContactName = row.Field<string>("ContactName"),
                    microcateId = row.Field<int?>("MicrocategoryId"),
                    SubCategoryId  = row.Field<int>("SubCategory")
                    
                }).ToList();

            // return View(items.ToList());

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ContactSuggestions GetMySuggesstion()
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
                DataTable dtContact = new DataTable();
                UserDetails objUserDetails = new UserDetails();
                Category objCategory = new Category();

                dtContact = objUserDetails.GetMySuggestionCategoryWise(objContact.ContactNumber);
                IList<Category> items = dtContact.AsEnumerable().Select(row =>
                 new Category
                 {
                     CatId = row.Field<int>("CatId"),
                     Name = row.Field<string>("Name"),
                     SubCategories = GetSubCate(row.Field<int>("CatId"), objContact.ContactNumber)


                 }).ToList();

                // var list = new SelectList(items, "CatId", "Name");
                objContact.Categories = items;
            }
            return objContact;
        }
        public ActionResult MySuggestion()
        {
            ContactSuggestions objContact = GetMySuggesstion();
            if(objContact!=null)
                {

                return View(objContact);
            }
            
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        // GET: FindSuggetion
        public ActionResult Index()
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
                DataTable dtContact = new DataTable();
                UserDetails objUserDetails = new UserDetails();
                Category objCategory = new Category();

                dtContact = objUserDetails.GetAllSuggestionCategoryWise();
                IList<Category> items = dtContact.AsEnumerable().Select(row =>
                 new Category
                 {
                     CatId = row.Field<int>("CatId"),
                     Name = row.Field<string>("Name"),
                     SubCategories = GetAllSubCate(row.Field<int>("CatId"))


                 }).ToList();

                // var list = new SelectList(items, "CatId", "Name");
                objContact.Categories = items;
            }          
            if (objContact != null)
            {

                return View(objContact);
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }

        }


    }
}