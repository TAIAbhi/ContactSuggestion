using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;
using BAL;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace ContactSuggestion.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session["Feedback"] = null;
            Session["Other"] = null;
            return View();
        }
        public ActionResult Feedback(string mobile)
        {
            if (mobile != null)
            {
                Session["Feedback"] = "Yes";
                Session["Other"] = null;
                Feedbacks objFeedback = new Feedbacks();
                objFeedback.MobileNumber = mobile;
                ViewBag.Aspect1 = GetAspectList(string.Empty);
                ViewBag.Aspect2 = GetAspectList(string.Empty);
                ViewBag.Aspect3 = GetAspectList(string.Empty);
                ViewBag.Aspect4 = GetAspectList(string.Empty);
                ViewBag.Aspect5 = GetAspectList(string.Empty);
                return View(objFeedback);
            }
            else
            {
                return RedirectToAction("Login", "Index");
            }
        }
        public ActionResult Ranking(string mobile)
        {
            if (mobile != null)
            {
                DataTable dtContact = new DataTable();
                UserDetails objUserDetails = new UserDetails();
                Ranks objRanks = new Ranks();
                // ViewBag.MySuggesstion = GetMySuggesstion();
                dtContact = objUserDetails.GetRanking();

                IList<Ranks> items = dtContact.AsEnumerable().Select(row =>
                    new Ranks
                    {
                        Point = row.Field<int>("Points"),
                        ContactName = row.Field<string>("ContactName"),
                        Rank = row.Field<Int64>("Rank")                      

                    }).ToList();

                Session["Feedback"] = "Yes";
                Session["Other"] = "Yes";
                return View(items);
            }
            else
            {
                return RedirectToAction("Login", "Index");
            }
        }
        public ActionResult Others(string mobile)
        {
            if (mobile != null)
            {
                Session["Feedback"] = "Yes";
                Session["Other"] = "Yes";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Index");
            }
        }
        public ActionResult EULA()
        {
            Session["Feedback"] = "Yes";
            Session["Other"] = "Yes";
            return View();
        }

        public SelectList GetAspectList(string selectedValue)
        {
            List<AspectList> aspectList = new List<AspectList>();

            AspectList Aspect = new AspectList();
            Aspect.AspectCode = "Video Rating";
            Aspect.AspectValue = "Video Rating";
            aspectList.Add(Aspect);

            AspectList Aspect1 = new AspectList();

            Aspect1.AspectCode = "Colour Scheming";
            Aspect1.AspectValue = "Colour Scheming";
            aspectList.Add(Aspect1);

            AspectList Aspect2 = new AspectList();

            Aspect2.AspectCode = "Categorisation";
            Aspect2.AspectValue = "Categorisation";
            aspectList.Add(Aspect2);

            AspectList Aspect3 = new AspectList();

            Aspect3.AspectCode = "Categorisation Selection";
            Aspect3.AspectValue = "Categorisation Selection";
            aspectList.Add(Aspect3);
            AspectList Aspect4 = new AspectList();

            Aspect4.AspectCode = "Location Selection/Addition";
            Aspect4.AspectValue = "Location Selection/Addition";
            aspectList.Add(Aspect4);

            AspectList Aspect5 = new AspectList();
            Aspect5.AspectCode = "Ease of Understanding";
            Aspect5.AspectValue = "Ease of Understanding";
            aspectList.Add(Aspect5);

            AspectList Aspect6 = new AspectList();
            Aspect6.AspectCode = "Ease of Use";
            Aspect6.AspectValue = "Ease of Use";
            aspectList.Add(Aspect6);

            AspectList Aspect7 = new AspectList();
            Aspect7.AspectCode = "Concept Rating";
            Aspect7.AspectValue = "Concept Rating";
            aspectList.Add(Aspect7);

            AspectList Aspect8 = new AspectList();
            Aspect8.AspectCode = "Tutorials";
            Aspect8.AspectValue = "Tutorials";
            aspectList.Add(Aspect8);

            
            if (!string.IsNullOrEmpty(selectedValue))
            {
                return new SelectList(aspectList, "AspectCode", "AspectValue", selectedValue);
            }
            else
            {
                return new SelectList(aspectList, "AspectCode", "AspectValue");

            }


        }
        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
        [HttpPost]
        public ActionResult Feedback([Bind(Include = "MobileNumber,Aspect1,Aspect2,Aspect3,Aspect4,Aspect5,Rating1,Rating2,Rating3,Rating4,Rating5,Comments1,Comments2,Comments3,Comments4,Comments5")] Feedbacks Fb)
        {
            if (ModelState.IsValid)
            {

                UserDetails objUserDetails = new UserDetails();
                Fb.MobileNumber = objUserDetails.MobileFormat(Fb.MobileNumber);
                Int64 isNumber = 0;
                Int64.TryParse(Fb.MobileNumber, out isNumber);
                if (Fb.MobileNumber.Length == 10 && isNumber > 0)
                {
                  
                    DataTable dtXml = new DataTable();
                    dtXml.TableName = "Details";
                    dtXml.Columns.Add("Aspect", typeof(string));
                    dtXml.Columns.Add("Rating", typeof(int));
                    dtXml.Columns.Add("Comments", typeof(string));

                    DataRow dr1 = dtXml.NewRow();
                    dr1["Aspect"] = Fb.Aspect1;
                    dr1["Rating"] = Fb.Rating1;
                    dr1["Comments"] = string.IsNullOrEmpty(Fb.Comments1)==true?"": Fb.Comments1.Trim(); 
                    dtXml.Rows.Add(dr1);

                    DataRow dr2 = dtXml.NewRow();
                    dr2["Aspect"] = Fb.Aspect2;
                    dr2["Rating"] = Fb.Rating2;
                    dr2["Comments"] = string.IsNullOrEmpty(Fb.Comments2) == true ? "" : Fb.Comments2.Trim();
                    dtXml.Rows.Add(dr2);

                    DataRow dr3 = dtXml.NewRow();
                    dr3["Aspect"] = Fb.Aspect3;
                    dr3["Rating"] = Fb.Rating3;
                    dr3["Comments"] = string.IsNullOrEmpty(Fb.Comments3) == true ? "" : Fb.Comments3.Trim();
                    dtXml.Rows.Add(dr3);

                    DataRow dr4 = dtXml.NewRow();
                    dr4["Aspect"] = Fb.Aspect4;
                    dr4["Rating"] = Fb.Rating4;
                    dr4["Comments"] = string.IsNullOrEmpty(Fb.Comments4) == true ? "" : Fb.Comments4.Trim();
                    dtXml.Rows.Add(dr4);

                    DataRow dr5 = dtXml.NewRow();
                    dr5["Aspect"] = Fb.Aspect5;
                    dr5["Rating"] = Fb.Rating5;
                    dr5["Comments"] = string.IsNullOrEmpty(Fb.Comments5) == true ? "" : Fb.Comments5.Trim();
                    dtXml.Rows.Add(dr5);
                    StringWriter swSQL2;
                    StringBuilder sbSQL2 = new StringBuilder();
                    swSQL2 = new System.IO.StringWriter(sbSQL2);
                    dtXml.WriteXml(swSQL2);
                    swSQL2.Dispose();
                    string feedbackDetails = sbSQL2.ToString(); 
                    if (objUserDetails.SaveFeedback(Fb.MobileNumber, feedbackDetails))
                    {
                        TempData["Success"] = "Your feedback have been submitted successfully!";
                        
                        return this.RedirectToAction("Feedback", new { mobile = Fb.MobileNumber });
                    }
                    else
                    {

                        TempData["Success"] = "We found some issue on feedback submit!";
                        return this.RedirectToAction("Feedback", new { mobile = Fb.MobileNumber });
                    }
                }
                else
                {
                    TempData["Success"] = "Invalid  mobile number!";
                    return this.RedirectToAction("Feedback", new { mobile = Fb.MobileNumber });
                }
                  
            }
            return View(Fb);
        }
        public ActionResult Home()
        {
            Session["Feedback"] = null;
            Session["Other"] = null;
            return View();
        }
        public ActionResult Register()
        {
            Session["Feedback"] = null;
            Session["Other"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "Email,Password,FirstName,LastName,ContactNumber,Location1,Location2,Location3,Comments,ContactLevelUnderstating,Notification,IsContactDetailsAdded")] WebSiteContact contact)
        {
            if (ModelState.IsValid)
            {
                
                    string websitesoruceId=  System.Configuration.ConfigurationManager.AppSettings["WebsiteUser"];
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

                        if (objUserDetails.SaveWebsiteContact(Convert.ToInt32(websitesoruceId), contact.FirstName, contact.LastName, contact.ContactNumber, contact.Location1, contact.Location2, contact.Location3, contact.Comments, contact.ContactLevelUnderstating, contact.Notification, contact.IsContactDetailsAdded, contact.Email, contact.Password))
                        {
                            TempData["Success"] = "You have been successfully registered! Please login";
                        }
                        else
                        {
                            TempData["Success"] = "This contact number is already exists!";
                        }
                    }
                    else
                    {
                        TempData["Success"] = "Invalid  contact number!";
                        return RedirectToAction("Create", contact);
                    }
                   
            }
            return View();
        }
        public ActionResult LogOff()
        {
            Globals.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult SkipVedio(bool skip)
        {

            string controllerName = string.Empty;
            string actionName = string.Empty;
            ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
            UserDetails objUserDetails = new UserDetails();
            if (objUserDetails.UpdateSkipVedio(objSource.ContactId, skip))
            {
                if (objSource.Role == 2)
                {
                    controllerName = "ContactDetails";
                    actionName = "Create";
                }
                else
                {
                    controllerName = "ContactDetails";
                    actionName = "Create";
                }
            }

            return Json(new
            {
                redirectUrl = Url.Action(actionName, controllerName),
                isRedirect = true,
                Message = ""
            });
        }

        public ActionResult loginSubmit(string userID, string password)
        {
            try
            {

           
            UserDetails objUserDetails = new UserDetails();
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(userID.Trim()) && !string.IsNullOrEmpty(password.Trim()))
            {
                userID = objUserDetails.MobileFormat(userID);
                Int64 isNumber = 0;
                Int64.TryParse(userID, out isNumber);
                if (userID.Length == 10)
                {
                    if (isNumber > 0)
                    {
                        int admin = 0;
                        Source objSource = new Source();

                        DataTable dtSource = new DataTable();
                        dtSource = objUserDetails.GetSourceDetails(userID, password, out admin);

                        if (dtSource.Rows.Count > 0)
                        {

                            msg = "";
                            objSource.Name = Convert.ToString(dtSource.Rows[0]["Name"]);
                            objSource.Mobile = Convert.ToString(dtSource.Rows[0]["ContactNumber"]);
                            objSource.ContactName = Convert.ToString(dtSource.Rows[0]["ContactName"]);
                            objSource.Role = admin;
                            objSource.SourceId = Convert.ToInt32(dtSource.Rows[0]["SourceId"]);
                            objSource.ContactId = Convert.ToInt32(dtSource.Rows[0]["ContactId"]);
                            objSource.IsInterns = Convert.ToString(dtSource.Rows[0]["IsInterns"]).Length > 0 ? Convert.ToBoolean(dtSource.Rows[0]["IsInterns"]) : false;
                            objSource.SkipVideo = Convert.ToString(dtSource.Rows[0]["SkipVideo"]).Length > 0 ? Convert.ToBoolean(dtSource.Rows[0]["SkipVideo"]) : true;
                            string macId = Convert.ToString(dtSource.Rows[0]["MacID"]);
                            objUserDetails.UpdateLoginCount(objSource.ContactId, macId);
                            Globals.TheUserSession = objSource;


                            string controllerName = string.Empty;
                            string actionName = string.Empty;
                            if (objSource.SkipVideo)
                            {
                                controllerName = "SourceContact";
                                actionName = "Vedio";
                            }
                            else
                            {
                                if (objSource.Role == 2)
                                {
                                    controllerName = "ContactDetails";
                                    actionName = "Create";
                                }
                                else
                                {
                                    controllerName = "ContactDetails";
                                    actionName = "Create";
                                }

                            }
                            return Json(new
                            {
                                redirectUrl = Url.Action(actionName, controllerName),
                                isRedirect = true,
                                Message = ""
                            });

                        }
                        else
                        {
                            msg = "Invalid user login.Please provide valid login details.";
                        }
                    }
                    else
                    {
                        msg = "Invalid user mobile number. Not a number :-" + userID;
                    }
                }
                else
                {
                    msg = "Invalid user mobile number. :-" + userID;
                }

            }
                return Json(new
                {
                    Message = msg
                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Message = ex.Message
                });
            }

         
        }

    }

}