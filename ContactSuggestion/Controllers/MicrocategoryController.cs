using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;
using BAL;
using System.Data;
using System.Net;

namespace ContactSuggestion.Controllers
{
    public class MicrocategoryController : Controller
    {
        public ActionResult ReloadIndex()
        {
            return Json(new
            {
                Message = string.Empty
            });
        }
        // GET: Microcategory
        public ActionResult Index(int? subCatId)
        {
            if (subCatId != null)
                Session["subCatId"] = subCatId;
            else
                Session["subCatId"] = null;

            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetMicroCategory(subCatId, null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name"),
                 SubCateName = row.Field<string>("SubCateName"),
                 SubCateId = row.Field<int>("SubCateId")

             }).ToList();
            ViewBag.MicroCategory = new MicroCategory();
            ContactSuggestions objContactSug = new ContactSuggestions();
            objContactSug = GetCate();
            if (subCatId != null)
            {
                // objContactSug.Category = Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["CategoryName"]);
                ViewBag.Category = Convert.ToString(objUserDetails.GetSubCategorName(subCatId).Rows[0]["CategoryName"]);
                // objContactSug.SubCategory ="->"+ Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["SubCateName"]);
                ViewBag.SubCategory = "->" + Convert.ToString(objUserDetails.GetSubCategorName(subCatId).Rows[0]["SubCateName"]);
            }
            ViewBag.CategoryTab = GetCate();


            return View(items.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MicroId,Name,SubCateId")] MicroCategory microCategory)
        {
            if (ModelState.IsValid)
            {
                Session["subCatId"] = null;
                ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
                UserDetails objUserDetails = new UserDetails();
                string[] myStrName = microCategory.Name.Split('-');
                microCategory.Name = myStrName[0] + " © ";
                if (myStrName.Length > 1)
                {
                    microCategory.Name = microCategory.Name + myStrName[1];
                }

                if (objUserDetails.SaveMicroCategory(microCategory.SubCateId, microCategory.Name.Trim(), objSource.SourceId, microCategory.MicroId))
                {
                    TempData["Success"] = "Micro category saved successfully.";
                }
                else
                {
                    TempData["Success"] = "Duplicate micro category found.";
                }
                ViewBag.MicroCategory = new MicroCategory();
                ViewBag.CategoryTab = GetCate();
                return RedirectToAction("Index");
            }
            return View(microCategory);
        }
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MicroCategory objMicroCategory = new MicroCategory();
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();

            DataTable microcat = new DataTable();
            microcat = objUserDetails.GetMicroCategory(null, id);
            if (microcat.Rows.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            objMicroCategory.MicroId = Convert.ToInt32(microcat.Rows[0]["MicroId"]);
            objMicroCategory.SubCateId = Convert.ToInt32(microcat.Rows[0]["SubCateId"]);
            objMicroCategory.Name = Convert.ToString(microcat.Rows[0]["Name"]);
            dtLocation = objUserDetails.GetMicroCategory(null, null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name"),
                 SubCateName = row.Field<string>("SubCateName"),
                 SubCateId = row.Field<int>("SubCateId")

             }).ToList();
            ViewBag.MicroCategory = objMicroCategory;

            ViewBag.CategoryTab = GetCate();
            if (Session["subCatId"] != null)
            {
                int? subCategoryId = Convert.ToInt32(Session["subCatId"]);
                if (subCategoryId != null)
                {
                    // objContactSug.Category = Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["CategoryName"]);
                    ViewBag.Category = Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["CategoryName"]);
                    // objContactSug.SubCategory ="->"+ Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["SubCateName"]);
                    ViewBag.SubCategory = "->" + Convert.ToString(objUserDetails.GetSubCategorName(subCategoryId).Rows[0]["SubCateName"]);
                }
                return View("Index", items.ToList().Where(a => a.SubCateId == subCategoryId).ToList());
            }
            else
            {
                return View("Index", items.ToList());
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MicroId,Name,SubCateId")] MicroCategory microCategory)
        {
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            UserDetails objUserDetails = new UserDetails();
            if (ModelState.IsValid)
            {
                Session["subCatId"] = null;
                if (objUserDetails.SaveMicroCategory(microCategory.SubCateId, microCategory.Name, objSource.SourceId, microCategory.MicroId))
                {
                    TempData["Success"] = "Micro category updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Success"] = "Duplicate micro category found.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.MicroCategory = new MicroCategory();
            ViewBag.CategoryTab = GetCate();
            return View(microCategory);
        }
        public IList<SubCategory> GetSubCate(int catId, int? contactId)
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
                    CatId = row.Field<int>("CatId")


                }).ToList();
            return items;
        }
        public ContactSuggestions GetCate()
        {
            ContactSuggestions objContact = new ContactSuggestions();
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetCategory(null, null).Tables[0];
            IList<Category> items = dtContact.AsEnumerable().Select(row =>
             new Category
             {
                 CatId = row.Field<int>("CatId"),
                 Name = row.Field<string>("Name"),
                 SubCategories = GetSubCate(row.Field<int>("CatId"), null)


             }).ToList();
            objContact.Categories = items;
            return objContact;
        }
        public ActionResult BindCategory()
        {
            ContactSuggestions objContact = new ContactSuggestions();
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetCategory(null, null).Tables[1];
            IList<Category> items = dtContact.AsEnumerable().Select(row =>
             new Category
             {
                 CatId = row.Field<int>("CatId"),
                 Name = row.Field<string>("Name"),
                 SubCategories = GetSubCate(row.Field<int>("CatId"), null)


             }).ToList();
            objContact.Categories = items;
            return View(objContact);
        }

    }

}