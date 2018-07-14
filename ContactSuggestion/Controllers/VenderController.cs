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

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View(objVenderViewModel);
        }
    }
}