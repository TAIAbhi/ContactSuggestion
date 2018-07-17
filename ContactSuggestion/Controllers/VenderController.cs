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
    public class VenderController : Controller
    {
        public ActionResult Upload()
        {
            VenderViewModel objVenderViewModel = new VenderViewModel();
            if (Session["Token"] != null)
            {
                string token = Convert.ToString(Session["Token"]);
              
                DataTable dtVenders = new DataTable();
                UserDetails objUserDetails = new UserDetails();
                dtVenders = objUserDetails.GetVenderDetails(token).Tables[0];
                IList<VenderViewModel> items = dtVenders.AsEnumerable().Select(row =>
                 new VenderViewModel
                 {
                     VendorId = row.Field<int>("VendorId"),
                     LoginToken = row.Field<string>("LoginToken"),
                     Password = row.Field<string>("Password"),
                     VendorName = row.Field<string>("VendorName"),
                     CatId = row.Field<int>("CatId"),
                     SubCatId = row.Field<int>("SubCatId"),
                     MicroCatId = row.Field<int?>("MicroCatId"),
                     Address = row.Field<string>("Address"),
                     LocationId = row.Field<int?>("LocationId"),
                     ContactNumber = row.Field<string>("ContactNumber"),
                     Email = row.Field<string>("Email"),
                     ContactId = row.Field<int>("ContactId")

                 }).ToList();
                objVenderViewModel = items.Where(a => a.LoginToken == token).FirstOrDefault();
                FillDocumentDropdown(objVenderViewModel.CatId, objVenderViewModel.SubCatId, objVenderViewModel.MicroCatId);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View(objVenderViewModel);
        }
        private void FillDocumentDropdown(int catid, int subcatid, int? micId)
        {
            DataTable dtContact = new DataTable();
            UserDetails objUserDetails = new UserDetails();
            dtContact = objUserDetails.GetDocumentType(catid, subcatid, micId).Tables[0];
            IList<Document> items = dtContact.AsEnumerable().Select(row =>
             new Document
             {
                 DocumentTypeId = row.Field<Int16>("DocumentTypeId"),
                 DocumentTypeName = row.Field<string>("DocumentTypeName")
             }).ToList();

            var list = new SelectList(items, "DocumentTypeId", "DocumentTypeName");
            ViewData["DocumentId"] = list;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "VendorId,Address,Email,DocumentId,ContactId")] VenderViewModel objVenderViewModel, HttpPostedFileBase fileupload1)
        {
            if (ModelState.IsValid)
            {
                if (fileupload1 != null)
                {
                    var fileName = Path.GetFileName(fileupload1.FileName);
                    string[] splitFileName = fileName.Split('.');
                    var newFile = splitFileName[0].ToString();
                    var extension = splitFileName[1].ToString();
                    Guid g = Guid.NewGuid();
                    var newFileName = objVenderViewModel.VendorId.ToString()+"_" + g.ToString() + "." + extension;
                    string path = Path.Combine(Server.MapPath("~/FileUpload"), newFileName);
                    fileupload1.SaveAs(path);
                    objVenderViewModel.DocDetail = fileName;
                    objVenderViewModel.DocURL = newFileName;
                    UserDetails objUserDetails = new UserDetails();
                   if(objUserDetails.UpdateVenderDetails(objVenderViewModel.VendorId, objVenderViewModel.Address, objVenderViewModel.Email, objVenderViewModel.DocumentId, objVenderViewModel.DocDetail, objVenderViewModel.DocURL, objVenderViewModel.ContactId))
                    {
                        TempData["Success"] = "Details are uploaded.";
                    }               

                }             
                return RedirectToAction("Upload");
            }
            FillDocumentDropdown(objVenderViewModel.CatId, objVenderViewModel.SubCatId, objVenderViewModel.MicroCatId);
            return View(objVenderViewModel);
        }
    }
}