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
   
    public class SourceContactController : Controller
    {
        // GET: SourceContact
        [CustomAuthorize("2", "1")]
        public ActionResult Index(string ContactName)
        {
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetContacts(null, Convert.ToInt32(objSource.SourceId),"");
            IList<Contacts> items = dtLocation.AsEnumerable().Select(row =>
             new Contacts
             {
                 ContactName = row.Field<string>("ContactName"),
                 ContactNumber = row.Field<string>("ContactNumber")

             }).ToList();

            Contacts objContact = new Contacts();
            if (ContactName == null)
                objContact.ContactsList = items.ToList();
            else
                objContact.ContactsList = items.ToList().Where(a => a.ContactName.ToUpper().Contains(ContactName.ToUpper())).ToList();
            return View(objContact);
        }

        // GET: SourceContact/Details/5
        [CustomAuthorize("2", "1")]
        public ActionResult Details(int id)
        {
            return View();
        }
        [CustomAuthorize("2", "1")]
        public ActionResult MyDetails()
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
                return View(objContact);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [CustomAuthorize("2", "1")]
        // POST: ContactSug/Create
        [HttpPost]
        public ActionResult MyDetails([Bind(Include = "SourceId,ContactId,Location4,Location5,Location6,ContactComments,ContactLevelUnderstating,Notification,IsContactDetailsAdded")] ContactSuggestions contactSugg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactSuggestions objContact = GetContact();
                    if (objContact != null)
                    {
                        
                            if (!string.IsNullOrEmpty(contactSugg.Location1) && !string.IsNullOrEmpty(contactSugg.Location2) && !string.IsNullOrEmpty(contactSugg.Location3))
                            {
                                contactSugg.IsContactDetailsAdded = true;
                            }
                        
                        UserDetails objUserDetails = new UserDetails();
                        if (objUserDetails.UpdateContact(contactSugg.ContactId, contactSugg.Location4, contactSugg.Location5, contactSugg.Location6, contactSugg.ContactComments, contactSugg.ContactLevelUnderstating, contactSugg.Notification, contactSugg.IsContactDetailsAdded))
                        {
                            TempData["Success"] = "Updated Successfully!";
                        }
                        return RedirectToAction("MyDetails", objContact);
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
            catch
            {
                return View();
            }
        }
        [CustomAuthorize("2", "1")]
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
                    if (dtContact.Rows[0]["ContactLevelUnderstanding"] != null)
                        objContact.ContactLevelUnderstating = Convert.ToInt32(dtContact.Rows[0]["ContactLevelUnderstanding"]);
                    if (dtContact.Rows[0]["Notification"] != null)
                    objContact.Notification= Convert.ToInt32(dtContact.Rows[0]["Notification"]);
                }
                ViewBag.ContactComments = objContact.ContactComments;
                ViewBag.Comments = objContact.Comments;
            }
            return objContact;
        }

        [CustomAuthorize("2", "1")]
        // GET: SourceContact/Create
        public ActionResult Create()
        {
            Contacts objContact = new Contacts();
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            if (!objSource.IsInterns)
                objContact.ContactSourceId = objSource.SourceId;
            ViewBag.Comments = objContact.Comments;
            if (TempData["Success"] == null)
            {
                TempData["Success"] = null;
            }
            return View(objContact);
        }
        // POST: SourceContact/Create
        [CustomAuthorize("2", "1")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "ContactSourceId,ContactName,ContactNumber,Location1,Location2,Location3,Comments,ContactLevelUnderstating,Notification,IsContactDetailsAdded")] Contacts contact)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserDetails"] != null)
                {
                    UserDetails objUserDetails = new UserDetails();
                    contact.ContactNumber = objUserDetails.MobileFormat(contact.ContactNumber);
                    Int64 isNumber = 0;
                    Int64.TryParse(contact.ContactNumber, out isNumber);
                    if (contact.ContactNumber.Length == 10 && isNumber > 0)
                    {
                       
                            if (!string.IsNullOrEmpty(contact.Location1) && !string.IsNullOrEmpty(contact.Location2) && !string.IsNullOrEmpty(contact.Location3))
                            {
                                contact.IsContactDetailsAdded = true;
                            }
                        
                        if (objUserDetails.SaveContact(contact.ContactSourceId, contact.ContactName, contact.ContactNumber, contact.Location1, contact.Location2, contact.Location3, contact.Comments, contact.ContactLevelUnderstating, contact.Notification, contact.IsContactDetailsAdded))
                        {
                            TempData["Success"] = "Added Successfully!";
                        }
                        else
                        {
                            TempData["Success"] = "Duplicate contact!";
                        }
                    }
                    else
                    {
                        TempData["Success"] = "Invalid  contact number!";
                        return RedirectToAction("Create", contact);
                    }
                    Contacts objContact = new Contacts();
                    ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
                    objContact.SourceId = objSource.SourceId;
                    return RedirectToAction("Create", objContact);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }
        [CustomAuthorize("2", "1")]
        public ActionResult Vedio()
        {
            return View();
        }

        [CustomAuthorize("2", "1")]
        // GET: SourceContact/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        [CustomAuthorize("2", "1")]
        // POST: SourceContact/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [CustomAuthorize("2", "1")]
        public ActionResult LogOff()
        {
            Globals.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        // GET: SourceContact/Delete/5
        [CustomAuthorize("2", "1")]
        public ActionResult Delete(int id)
        {
            return View();
        }
        [CustomAuthorize("2", "1")]
        // POST: SourceContact/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [CustomAuthorize("2", "1")]
        public ActionResult AddSource()
        {
            return View();
        }
        [CustomAuthorize("2", "1")]
        [HttpPost]
        public ActionResult AddSource([Bind(Include = "ContactName,Mobile,MyPassword,FriendsPassword,IsInterns")] Source source)
        {
            Source objSoruce = new Source();
            if (ModelState.IsValid)
            {
                UserDetails objUserDetails = new UserDetails();

                source.Mobile = objUserDetails.MobileFormat(source.Mobile);
                Int64 isNumber = 0;
                Int64.TryParse(source.Mobile, out isNumber);
                if (source.Mobile.Length == 10 && isNumber > 0)
                {
                    if (objUserDetails.SaveSource(source.ContactName, source.Mobile, source.MyPassword, source.FriendsPassword, source.IsInterns))
                    {
                        TempData["Success"] = "Source details added! Please find details " + "Source Name:" + source.ContactName + ",ContactNumber:" + source.Mobile + ",MyPassword:" + source.MyPassword + ",FriendsPassword:" + source.FriendsPassword;
                    }
                    else
                    {
                        TempData["Success"] = "Duplicate source";
                    }
                }
                else
                {
                    TempData["Success"] = "Invalid  contact number!";
                    return RedirectToAction("AddSource", source);
                }
                return RedirectToAction("AddSource", objSoruce);

            }
            return RedirectToAction("AddSource", source);
        }
        [CustomAuthorize("2", "1")]
        [HttpPost]
        public JsonResult AutoCompleteSource(string prefix)
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

            var location = (from loc in items
                            where loc.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || loc.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                            select new
                            {
                                label = loc.Name.Trim(),
                                val = loc.SourceId
                            }).ToList();
            return Json(location);
        }
      

        public ActionResult ViewMyContacts(string srcMobile)
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
                 Location3 = row.Field<string>("LocationId3")
             }).ToList();

            return View(items);
        }
    }
}
