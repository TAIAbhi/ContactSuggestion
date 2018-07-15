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
    public class NotificationController : Controller
    {
        [HttpPost]
        public JsonResult ContactLookup(string prefix)
        {
            DataTable dtLocation = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtLocation = objUserDetails.GetContacts(null, null, "");
            IList<Contacts> items = dtLocation.AsEnumerable().Select(row =>
             new Contacts
             {
                 ContactId = row.Field<int>("ContactId"),
                 ContactName = row.Field<string>("ContactName")

             }).ToList();

            var location = (from loc in items
                            where loc.ContactName.ToUpper().Contains(prefix.ToUpper())
                            select new
                            {
                                label = loc.ContactName,
                                val = loc.ContactId
                            }).ToList();
            return Json(location);
        }

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
            dtLocation = objUserDetails.GetLocation(null, string.Empty, string.Empty, Convert.ToInt32(cityId));
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
        // GET: Notification
        public ActionResult Index()
        {
            return View();

        }
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

            var list = new SelectList(items, "CityId", "CityName");

            ViewBag.CityId = list;

        }
        public ActionResult Create()
        {
            RequestNotificationViewModel objRequestNotificationViewModel = new RequestNotificationViewModel();
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            Category objCategory = new Category();
            dtContact = objUserDetails.GetCategory(null, null).Tables[0];
            IList<Category> items = dtContact.AsEnumerable().Select(row =>
             new Category
             {
                 CatId = row.Field<int>("CatId"),
                 Name = row.Field<string>("Name"),
                 SubCategories = GetSubCate(row.Field<int>("CatId"))


             }).ToList();
            FillCityDrodown();
            objRequestNotificationViewModel.Categories = items;

            return View(objRequestNotificationViewModel);
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "CatId,SubCategoryId,MicrocategoryId,ContactId,Text,NotificationType,NotificationTitle,LocationId,NotificationPhoto,ProvdReqdsuggData")] RequestNotificationViewModel reqSug)
        {
            if (ModelState.IsValid)
            {
                ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
              //  UserDetails objUserDetails = new UserDetails();
                string deviceID = string.Empty;
                string token = string.Empty;
                string type = string.Empty;
                UserDetails objUserDetails = new UserDetails();
                DataTable dtDeviceDetails = new DataTable();
                DataTable dtlocation = new DataTable();
                DataTable dtDeviceUIDList = new DataTable();
                dtDeviceDetails = objUserDetails.GetDeviceDetails(reqSug.ContactId);
                for (int i = 0; i < dtDeviceDetails.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(reqSug.NotificationPhoto))
                    {
                        reqSug.NotificationPhoto = "http://tagaboutit.com/Images/phone-display-round-icon.png";
                    }
                    if (reqSug.NotificationType == "Ranking")
                    {
                        reqSug.RedirectTo = "http://tagaboutit.com/login/Ranking";
                    }
                    if (reqSug.NotificationType == "ReqAddSug")
                    {
                        reqSug.RedirectTo = "AddSugg";
                    }
                    if (reqSug.NotificationType == "ProvdReqdsugg")
                    {
                        reqSug.RedirectTo = reqSug.ProvdReqdsuggData;
                    }
                    if (reqSug.NotificationType == "Reqdsuggprovd")
                    {
                        reqSug.RedirectTo = "ViewSugg";
                    }
                    if (reqSug.NotificationType == "MyDetail")
                    {
                        reqSug.RedirectTo = "MyDetail";
                    }
                    if (reqSug.NotificationType == "ModYourSug")
                    {
                        reqSug.RedirectTo = "ViewSugg";
                    }
                    if (objUserDetails.SaveNotificationForWebSend(reqSug.UID, reqSug.SubCategoryId, reqSug.MicrocategoryId, reqSug.ContactId, Convert.ToInt32(dtDeviceDetails.Rows[i]["UID"]), reqSug.Text, reqSug.NotificationType, false, reqSug.NotificationTitle, reqSug.LocationId, reqSug.NotificationPhoto, reqSug.RedirectTo, objSource.ContactId))
                    {
                        TempData["Success"] = "Added Successfully!";
                        deviceID = Convert.ToString(dtDeviceDetails.Rows[i]["DeviceID"]);
                        type = Convert.ToString(dtDeviceDetails.Rows[i]["Type"]);
                        token = Convert.ToString(dtDeviceDetails.Rows[i]["Token"]);
                        dtDeviceDetails = objUserDetails.GetSourcesToken().Tables[0];
                        dtDeviceUIDList = objUserDetails.GetSourcesToken().Tables[1];
                       // string[] strSplit = Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]).Length > 0 ? Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]).Split('|') : null;
                        PushAndroidNotification(Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]), reqSug.CatId, reqSug.SubCategoryId, reqSug.MicrocategoryId, reqSug.LocationId, reqSug.NotificationTitle, reqSug.Text, Convert.ToString(dtDeviceUIDList.Rows[0]["UIDList"]));

                    }
                    else
                    {
                        TempData["Success"] = "Some issue found!";
                    }
                }
                if (dtDeviceDetails.Rows.Count == 0)
                {
                    TempData["Success"] = "No device registered for this contact.";
                }

            }
            RequestNotificationViewModel objRequestNotificationViewModel = new RequestNotificationViewModel();
            DataTable dtContact = new DataTable();
            UserDetails objUserDetailsForView = new UserDetails();
            Category objCategory = new Category();
            dtContact = objUserDetailsForView.GetCategory(null, null).Tables[0];
            IList<Category> items = dtContact.AsEnumerable().Select(row =>
             new Category
             {
                 CatId = row.Field<int>("CatId"),
                 Name = row.Field<string>("Name"),
                 SubCategories = GetSubCate(row.Field<int>("CatId"))


             }).ToList();
            FillCityDrodown();
            objRequestNotificationViewModel.Categories = items;

            return View(objRequestNotificationViewModel);
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
        public void PushAndroidNotification(string token, int? CatId, int? SubCategoryId, int? MicrocategoryId, int? LocationId, string title, string text, string uIDList)
        {
            // Authorization: key = AAAAS7HcV0s:APA91bF436VQayZCb - O3blmqqovG - 8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7 - 0utoLGprkzIm06OYR2zn43m4qGkS5V - Jep
            string[] regIDs = token.Split('|');

            string androidApiUrl = "https://fcm.googleapis.com/fcm/send";
            PushNotification objPushnotification = new PushNotification();
            objPushnotification.registration_ids = regIDs;

            Notifications objNotifications = new Notifications();
            objNotifications.title = title;
            objNotifications.body = text; //"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objNotifications.sound = "default";
            objNotifications.vibrate = "default";
            objNotifications.priority = "high";
            objPushnotification.notification = objNotifications;
            PushData objPushData = new PushData();
            objPushData.title = title;
            objPushData.body = text;// "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objPushData.date = DateTime.Now.ToString();
            objPushData.catId = CatId;
            objPushData.subCatId = SubCategoryId;
            objPushData.microId = MicrocategoryId;
            objPushData.locationId = LocationId;
            objPushnotification.data = objPushData;
            // CallApi(objPushnotification, androidApiUrl);
            SendPushNotification(objPushnotification, androidApiUrl, uIDList);

        }
        public static void SendPushNotification(Object ApiRequestModel, string api, string uIDList)
        {
            try
            {
                string applicationID = System.Configuration.ConfigurationManager.AppSettings["RegKey"]; //"AAAAS7HcV0s:APA91bF436VQayZCb-O3blmqqovG-8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7-0utoLGprkzIm06OYR2zn43m4qGkS5V-Jep";
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
                                    objUserDetailsWeb.UpdateNotificationTimeSent(uIDList);
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
        public void CallApi(Object ApiRequestModel, string api)
        {

            string returnData = string.Empty;


            StringContent content = new StringContent(JsonConvert.SerializeObject(ApiRequestModel), Encoding.UTF8, "application/json");
            var stringContent = JsonConvert.SerializeObject(ApiRequestModel);
            HttpClientHandler handler = new HttpClientHandler()
            {
                PreAuthenticate = true,
                UseDefaultCredentials = true,
                UseProxy = false
            };

            using (var client = new HttpClient(handler))
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "AAAAS7HcV0s:APA91bF436VQayZCb - O3blmqqovG - 8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7 - 0utoLGprkzIm06OYR2zn43m4qGkS5V - Jep");
                var responseUpload = client.PostAsync(String.Format(api), content).Result;
                if (responseUpload.IsSuccessStatusCode)
                {
                    returnData = responseUpload.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw (new System.Exception("Api Not available"));

                }

            }
        }

    }

}