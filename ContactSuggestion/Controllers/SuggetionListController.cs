using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;
using BAL;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;


namespace ContactSuggestion.Controllers
{
    [CustomAuthorize("2", "1")]
    public class SuggetionListController : Controller
    {
        //public object ReturnData(string data)
        //{
        //    return string.IsNullOrEmpty(data)!=true?data:null;
        //}
        public static T ConvertTo<T>(object value)
        {
            return ConvertTo(value, default(T));
        }
        public static T ConvertTo<T>(object value, T defaultValue)
        {
            if (value == DBNull.Value)
            {
                return defaultValue;
            }
            return (T)ChangeType(value, typeof(T));
        }
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }

            // if it's not a nullable type, just pass through the parameters to Convert.ChangeType
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                // null input returns null output regardless of base type
                if (value == null)
                {
                    return null;
                }

                // it's a nullable type, and not null, which means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }
            else if (conversionType.IsEnum)
            {
                // strings require Parse method
                if (value is string)
                {
                    return Enum.Parse(conversionType, (string)value);
                }
                // primitive types can be instantiated using ToObject
                else if (value is int || value is uint || value is short || value is ushort ||
                     value is byte || value is sbyte || value is long || value is ulong)
                {
                    return Enum.ToObject(conversionType, value);
                }
                else
                {
                    throw new ArgumentException(String.Format("Value cannot be converted to {0} - current type is " +
                                          "not supported for enum conversions.", conversionType.FullName));
                }
            }

            return Convert.ChangeType(value, conversionType);
        }

        public static T ReturnData<T>(string data)
        {
            T result = default(T);
            string value = data;

            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    result = ConvertTo<T>(value);
                }
                catch
                {
                    //Could not convert.  Pass back default value...
                    result = default(T);
                }
            }

            return result;
        }
        // GET: SuggetionList
        public ActionResult Index(string Category, string StartDate, string EndDate, string SubCategory, string Microcategory, string BusinessName, string CityName, string Location1, string Source, string Contact, string Platform, string IsValid, string VendorIsVerified, string IsChain, string isLocal)
        {//string option, string search, int? pageNumber
            int? locId = null;
            if (!string.IsNullOrEmpty(Location1))
            {
                locId = Convert.ToInt32(Location1);
            }
            int cityId = 0;
            if (!string.IsNullOrEmpty(CityName))
            {
                cityId = Convert.ToInt32(CityName);
            }

            ContactSuggestions objContactSuggestion = new ContactSuggestions();
            // List<ContactSuggestions> objList = new List<ContactSuggestions>();
            // objContactSuggestion.ContactSuggestionsList = objList;
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            // int? cateID = !string.IsNullOrEmpty(Category)
            if (locId != null)
            {
                DataTable dtAllLocation = objUserDetails.GetLocation(locId, string.Empty, string.Empty, cityId);
                Location1 = string.IsNullOrEmpty(Convert.ToString(dtAllLocation.Rows[0]["LocationName"])) ? Convert.ToString(dtAllLocation.Rows[0]["Suburb"]).Trim() : Convert.ToString(dtAllLocation.Rows[0]["LocationName"]).Trim() + " - " + Convert.ToString(dtAllLocation.Rows[0]["Suburb"]).Trim();
            }

            dtLocation = objUserDetails.GetSuggestionList(ReturnData<int?>(Category), ReturnData<int?>(SubCategory), null, ReturnData<int?>(Contact), "", ReturnData<int?>(Source), ReturnData<string>(BusinessName), ReturnData<bool?>(isLocal), ReturnData<string>(Location1), ReturnData<int?>(Microcategory), "", ReturnData<int?>(CityName), ReturnData<DateTime?>(StartDate), ReturnData<DateTime?>(EndDate), ReturnData<bool?>(IsChain), ReturnData<bool?>(IsValid), ReturnData<bool?>(IsValid)).Tables[0];
            IList<ContactSuggestions> items = dtLocation.AsEnumerable().Select(row =>
             new ContactSuggestions
             {
                 UID = row.Field<int>("UID").ToString(),
                 BusinessName = row.Field<string>("BusinessName")==null?"": row.Field<string>("BusinessName"),
                 BusinessContact = row.Field<string>("BusinessContact")==null?"": row.Field<string>("BusinessContact"),
                 CityName = row.Field<string>("CityName")==null?"": row.Field<string>("CityName"),
                 Location1 = row.Field<string>("LocationId1")==null?"": row.Field<string>("LocationId1"),
                 Microcategory = row.Field<string>("Microcategory")==null?"": row.Field<string>("Microcategory"),
                 SubCategory = row.Field<string>("SubCategoryName")==null?"": row.Field<string>("SubCategoryName"),
                 Category = row.Field<string>("CategoryName")==null?"": row.Field<string>("CategoryName"),
                 Source = row.Field<string>("SourceName")==null?"" : row.Field<string>("SourceName"),
                 Contact = row.Field<string>("ContactName")==null?"": row.Field<string>("ContactName"),
                 AddedWhen = row.Field<string>("AddedWhen")==null? "": row.Field<string>("AddedWhen"),
                 Platform = row.Field<string>("Platform")==null?"": row.Field<string>("Platform"),
                 Chain = row.Field<string>("IsAChain")==null?"":row.Field<string>("IsAChain"),
                 Local = row.Field<string>("CitiLevelBusiness")==null?"": row.Field<string>("CitiLevelBusiness"),
                 IsVerified = row.Field<string>("VendorIsVerified")==null?"":row.Field<string>("VendorIsVerified"),
                 IsActive = row.Field<string>("IsValid")==null?"": row.Field<string>("IsValid"),
                 Maps = row.Field<string>("ShowMaps")==null?"": row.Field<string>("ShowMaps"),
                 DataActive = row.Field<string>("IsActive")==null?"": row.Field<string>("IsActive"),
                 GoogleMaps = "https://www.google.com/maps/search/" + (row.Field<string>("BusinessName")==null?"": row.Field<string>("BusinessName").Trim()) + "+" + (row.Field<string>("LocationId1")!=null? (row.Field<string>("LocationId1").Split('-').Length > 1 ? row.Field<string>("LocationId1").Split('-')[1] : row.Field<string>("LocationId1")):"").Trim()


             }).ToList();
            objContactSuggestion.ContactSuggestionsList = items;
            FillCategoryDrodown();
            FillSubCate(Category);
            FillMicroCate(SubCategory);
            City();
            BindSource();
            FillContact(Source);
            PlatformBind();
            FillLocation(CityName);
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
        private void FillSubCate(string id)
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

            var list = new SelectList(items, "SubCatId", "Name");
            ViewData["SubCategory"] = list;
        }
        private void FillMicroCate(string id)
        {
            int cid = 0;
            int.TryParse(id, out cid);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetMicroCategory(cid == 0 ? -1 : cid, null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name")

             }).ToList();
            var list = new SelectList(items, "MicroId", "Name");
            ViewData["Microcategory"] = list;
        }
        private void FillContact(string id)
        {
            int cid = 0;
            int.TryParse(id, out cid);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetContacts(null, cid, "");
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
            var list = new SelectList(items, "ContactId", "ContactName");
            ViewData["Contact"] = list;
        }
        private void FillLocation(string id)
        {
            int cid = 0;
            int.TryParse(id, out cid);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty, cid == 0 ? -1 : cid);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = string.IsNullOrEmpty(row.Field<string>("LocationName")) ? row.Field<string>("Suburb") : row.Field<string>("LocationName").Trim() + " - " + row.Field<string>("Suburb").Trim(),
                 Area = row.Field<string>("Area"),
                 City = row.Field<string>("City"),
                 Suburb = row.Field<string>("Suburb") != null ? row.Field<string>("Suburb") : "",

             }).ToList();
            var list = new SelectList(items, "LocationId", "LocationName");
            ViewData["Location1"] = list;
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
            dtLocation = objUserDetails.GetMicroCategory(subCatId == 0 ? -1 : subCatId, null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name")

             }).ToList();

            var location = (from loc in items
                            select new
                            {
                                label = loc.Name.Replace('©', '~'),
                                val = loc.MicroId
                            }).ToList();
            return Json(location);
        }

        public void BindSource()
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
        public JsonResult ViewMyContacts(int sourceId)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetContacts(null, sourceId, "");
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
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty, Convert.ToInt32(city));
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

                            select new
                            {
                                label = string.IsNullOrEmpty(loc.LocationName.Trim()) ? loc.Suburb.Trim() : loc.LocationName.Trim() + " - " + loc.Suburb.Trim(),
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
        public void PlatformBind()
        {
            List<SelectListItem> li = new List<SelectListItem>();

            li.Add(new SelectListItem { Text = "Web", Value = "Web" });

            ViewData["Platform"] = li;

        }
        public ActionResult Edit(int? id)
        {
            string sugID = Convert.ToString(id);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetSuggestionFilters(null, null, id, null, "", null, "", null, "", null).Tables[0];
            IList<ContactSuggestions> items = dtLocation.AsEnumerable().Select(row =>
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
                 SubCategoryId = row.Field<int>("SubCategory"),
                 CatId = row.Field<int>("Category"),
                 IsAChain = row.Field<bool>("IsAChain"),
                 UID = Convert.ToString(row.Field<int>("UID")),
                 locationId = Convert.ToInt32(row.Field<string>("LocationId2")),
                 cityid = row.Field<int?>("City")
             }).ToList();
            ContactSuggestions objContactSuggestions = items.Where(a => a.UID == sugID).FirstOrDefault();
            FillCategoryDrodown(Convert.ToInt32(objContactSuggestions.CatId));
            FillSubCateDropdown(Convert.ToInt32(objContactSuggestions.CatId), Convert.ToInt32(objContactSuggestions.SubCategoryId));
            FillMicroCateDropdown(Convert.ToInt32(objContactSuggestions.SubCategoryId), Convert.ToInt32(objContactSuggestions.microcateId));
            FillCityDropdown(objContactSuggestions.cityid);
            FillLocationDropdown(objContactSuggestions.locationId, Convert.ToInt32(objContactSuggestions.cityid));
            return View(objContactSuggestions);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "UID, SourceId,  ContactId,  Category,  SubCategory,  Microcategory,  BusinessName,  CitiLevelBusiness,  BusinessContact,  Location1,  Comments, IsAChain,  Platform,  CityName,ReasonForChange")] ContactSuggestions contactSugg)
        {
            int  locID = Convert.ToInt32(contactSugg.Location1);
            int? micorID = null;
            if(!string.IsNullOrEmpty(contactSugg.Microcategory))
            {
                micorID = Convert.ToInt32(contactSugg.Microcategory);
            }
            if (ModelState.IsValid)
            {
                string message = string.Empty;
                //  CustomResponseMessage custMessage = new CustomResponseMessage();
                ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
                UserDetails objUserDetails = new UserDetails();
                try
                {
                    DataTable dtDeviceDetails = objUserDetails.GetSourcesTokenByContactId(contactSugg.ContactId).Tables[0];

                   DataTable dtDevices = objUserDetails.GetDeviceDetails(contactSugg.ContactId);

                    //ContactSuggestions objContactSugg = GetContact(contactSugg.contactId);
                    contactSugg.BusinessContact = objUserDetails.MobileFormat(contactSugg.BusinessContact);                   

                   DataTable dtLocation = objUserDetails.GetLocation(Convert.ToInt32(contactSugg.Location1), string.Empty, string.Empty,Convert.ToInt32(contactSugg.CityName));
                    contactSugg.Location1 = string.IsNullOrEmpty(Convert.ToString(dtLocation.Rows[0]["LocationName"])) ? Convert.ToString(dtLocation.Rows[0]["Suburb"]).Trim() : Convert.ToString(dtLocation.Rows[0]["LocationName"]).Trim() + " - " + Convert.ToString(dtLocation.Rows[0]["Suburb"]).Trim();
                    contactSugg.Location2 = Convert.ToString(dtLocation.Rows[0]["LocationId"]);
                    contactSugg.Location3 = Convert.ToString(dtLocation.Rows[0]["AreaShortCode"]);
                    if (!string.IsNullOrEmpty(contactSugg.BusinessContact))
                    {
                        Int64 isNumber = 0;
                        Int64.TryParse(contactSugg.BusinessContact, out isNumber);
                        if (contactSugg.BusinessContact.Length == 10 && isNumber > 0)
                        {
                            if (objUserDetails.UpdateContactSuggestions(Convert.ToInt32(contactSugg.UID), contactSugg.SourceId, contactSugg.ContactId, contactSugg.Category, contactSugg.SubCategory, contactSugg.Microcategory, contactSugg.BusinessName, contactSugg.CitiLevelBusiness, contactSugg.BusinessContact, contactSugg.Location1, contactSugg.Location2, contactSugg.Location3, contactSugg.Comments, "", "", "", "", contactSugg.ContactComments, contactSugg.IsAChain, contactSugg.Platform,Convert.ToInt32(contactSugg.CityName)))
                            {
                                for (int i = 0; i < dtDevices.Rows.Count; i++)
                                {
                                    objUserDetails.SaveNotificationForWebSend(0, Convert.ToInt32(contactSugg.SubCategory), micorID, contactSugg.ContactId, Convert.ToInt32(dtDevices.Rows[i]["UID"]), contactSugg.ReasonForChange, "ModYourSug", false, "Suggestion Updated", locID, "Suggestion Updated", "ViewSugg",objSource.ContactId);
                                }
                                    // custMessage.action = "Success";
                                // custMessage.message = "Update Successfully!";
                                TempData["Success"] = "Update Successfully!";

                                PushAndroidNotification(Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]), Convert.ToInt32(contactSugg.Category), Convert.ToInt32(contactSugg.SubCategory), Convert.ToInt32(contactSugg.Microcategory), Convert.ToInt32(locID), contactSugg.ReasonForChange, "","", contactSugg.BusinessName, contactSugg.ContactId.ToString(), contactSugg.UID, contactSugg.ReasonForChange);
                            }
                            else
                            {
                                // custMessage.action = "Failure";
                                // custMessage.message = "Please choose different Business Name and Location!.";
                                // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                                TempData["Success"] = "Please choose different Business Name and Location!";
                            }
                        }
                        else
                        {
                            //// custMessage.action = "Failure";
                            // custMessage.message = "Invalid Business contact number!";
                            //return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                            TempData["Success"] = "Invalid Business contact number!";

                        }
                    }
                    else
                    {
                        if (objUserDetails.UpdateContactSuggestions(Convert.ToInt32(contactSugg.UID), contactSugg.SourceId, contactSugg.ContactId, contactSugg.Category, contactSugg.SubCategory, contactSugg.Microcategory, contactSugg.BusinessName, contactSugg.CitiLevelBusiness, contactSugg.BusinessContact, contactSugg.Location1, contactSugg.Location2, contactSugg.Location3, contactSugg.Comments, "", "", "", "", contactSugg.ContactComments, contactSugg.IsAChain, contactSugg.Platform, Convert.ToInt32(contactSugg.CityName)))
                        {
                            for (int i = 0; i < dtDevices.Rows.Count; i++)
                            {
                                objUserDetails.SaveNotificationForWebSend(0, Convert.ToInt32(contactSugg.SubCategory), micorID, contactSugg.ContactId, Convert.ToInt32(dtDevices.Rows[i]["UID"]), contactSugg.ReasonForChange, "ModYourSug", false, "Suggestion Updated", locID, "Suggestion Updated", "ViewSugg", objSource.ContactId);
                            }

                            // custMessage.action = "Success";
                            // custMessage.message = "Update Successfully!";
                            TempData["Success"] = "Update Successfully!";
                            PushAndroidNotification(Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]), Convert.ToInt32(contactSugg.Category), Convert.ToInt32(contactSugg.SubCategory), Convert.ToInt32(contactSugg.Microcategory), Convert.ToInt32(locID), contactSugg.ReasonForChange, "", "", contactSugg.BusinessName, contactSugg.ContactId.ToString(), contactSugg.UID, contactSugg.ReasonForChange);
                        }
                        else
                        {
                            // custMessage.action = "Failure";
                            // custMessage.message = "Please choose different Business Name and Location!.";
                            // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                            TempData["Success"] = "Please choose different Business Name and Location!";
                        }

                    }
                }
                catch (Exception ex)
                {
                    TempData["Success"] = ex.Message;

                    // custMessage.action = "Failure";
                    // custMessage.message = ex.Message;
                    //SendEmail(ex.Message, "suggestion PUT");
                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            FillCategoryDrodown(Convert.ToInt32(contactSugg.Category));
            FillSubCateDropdown(Convert.ToInt32(contactSugg.Category), Convert.ToInt32(contactSugg.SubCategory));
            FillMicroCateDropdown(Convert.ToInt32(contactSugg.SubCategory), Convert.ToInt32(contactSugg.Microcategory));
            FillCityDropdown(Convert.ToInt32(contactSugg.CityName));
            FillLocationDropdown(locID, Convert.ToInt32(contactSugg.CityName));
            return View();
        } 

        private void FillCategoryDrodown(int catId)
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

            // var list = new SelectList(items, "CatId", "Name");
            var list = new SelectList(items, "CatId", "Name", catId);
            ViewData["Category"] = list;
        }
        private void FillSubCateDropdown(int catid, int subcatId)
        {

            DataTable dSubCate = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dSubCate = objUserDetails.GetSubCategory(catid == 0 ? -1 : catid, null, null).Tables[0];
            IList<SubCategory> items = dSubCate.AsEnumerable().Select(row =>
             new SubCategory
             {
                 SubCatId = row.Field<int>("SubCatId"),
                 Name = row.Field<string>("Name")
             }).ToList();

            // var list = new SelectList(items, "SubCatId", "Name");
            var list = new SelectList(items, "SubCatId", "Name", subcatId);

            ViewData["SubCategory"] = list;
        }
        private void FillMicroCateDropdown(int subcatId, int ? mcid)
        {

            // int.TryParse(subcatId, out cid);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetMicroCategory(subcatId == 0 ? -1 : subcatId, null);
            IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
             new MicroCategory
             {
                 MicroId = row.Field<int>("MicroId"),
                 Name = row.Field<string>("Name")

             }).ToList();
            var list = new SelectList(items, "MicroId", "Name", mcid);           
            ViewData["Microcategory"] = list;
        }
        public void FillCityDropdown(int ? cityId)
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
            var list = new SelectList(items, "CityId", "CityName", cityId);
            ViewData["CityName"] = list;
        }
        private void FillLocationDropdown(int locid, int  cityid)
        {
           // int cid = 0;yy
          //  int.TryParse(cityid, out cid);
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty, cityid == 0 ? -1 : cityid);
            IList<Location> items = dtLocation.AsEnumerable().Select(row =>
             new Location
             {
                 LocationId = row.Field<int>("LocationId"),
                 LocationName = string.IsNullOrEmpty(row.Field<string>("LocationName")) ? row.Field<string>("Suburb") : row.Field<string>("LocationName").Trim() + " - " + row.Field<string>("Suburb").Trim(),

                 Area = row.Field<string>("Area"),
                 City = row.Field<string>("City"),
                 Suburb = row.Field<string>("Suburb") != null ? row.Field<string>("Suburb") : "",

             }).ToList();
            var list = new SelectList(items, "LocationId", "LocationName", locid);
            ViewData["Location1"] = list;
        }
        public void PushAndroidNotification(string token, int? CatId, int? SubCategoryId, int? MicrocategoryId, int? LocationId, string title, string text, string uIDList,string bussName, string contactId, string sugId, string body)
        {
            // Authorization: key = AAAAS7HcV0s:APA91bF436VQayZCb - O3blmqqovG - 8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7 - 0utoLGprkzIm06OYR2zn43m4qGkS5V - Jep
            string[] regIDs = token.Split('|');

            string androidApiUrl = "https://fcm.googleapis.com/fcm/send";
            PushNotification objPushnotification = new PushNotification();
            objPushnotification.registration_ids = regIDs;

            Notifications objNotifications = new Notifications();
            objNotifications.title = "Your suggestion updated has been updated";
            objNotifications.body = body; //"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objNotifications.sound = "default";
            objNotifications.vibrate = "default";
            objNotifications.priority = "high";
           // objNotifications.
            objPushnotification.notification = objNotifications;
            PushData objPushData = new PushData();
            objPushData.title = "Your suggestion updated has been updated";
            objPushData.body = body;// "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objPushData.date = DateTime.Now.ToString();
            objPushData.catId = CatId;
            objPushData.subCatId = SubCategoryId;
            objPushData.microId = MicrocategoryId;
            objPushData.locationId = LocationId;
            objPushData.bussinessName = bussName;
            objPushData.contactId = contactId;
            objPushData.suggestionId = sugId;
            objPushnotification.data = objPushData;
            // CallApi(objPushnotification, androidApiUrl);
            SendPushNotification(objPushnotification, androidApiUrl, uIDList);

        }
        public static void SendPushNotification(Object ApiRequestModel, string api, string uIDList)
        {
            try
            {
                string applicationID = "AAAAS7HcV0s:APA91bF436VQayZCb-O3blmqqovG-8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7-0utoLGprkzIm06OYR2zn43m4qGkS5V-Jep";
                HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(api);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(ApiRequestModel);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                // tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                                if (tResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    UserDetails objUserDetailsWeb = new UserDetails();
                                   // objUserDetailsWeb.UpdateNotificationTimeSent(uIDList);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public ActionResult Delete(int? id)
        {
            UserDetails objUserDetails = new UserDetails();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }           
            else
            {
                objUserDetails.DeleteSuggesion(Convert.ToInt32(id), "record deleted from web");
                TempData["Success"] = "Deleted Successfully!";
                return RedirectToAction("Index");
            }
        }
        public ActionResult IsValid(int? id)
        {
            UserDetails objUserDetails = new UserDetails();
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Guid objGuid = Guid.NewGuid();
                objUserDetails.VerifySuggesion(Convert.ToInt32(id), objGuid.ToString(), objSource.ContactId);
                TempData["Success"] = "Validated Successfully!";
                return RedirectToAction("Index");
            }
        }
    }

}