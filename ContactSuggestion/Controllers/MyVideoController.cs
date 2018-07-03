using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;

namespace ContactSuggestion.Controllers
{
    [CustomAuthorize("2")]
    public class MyVideoController : Controller
    {
        // GET: MyVideo
        public ActionResult Index()
        {
            return View();
        }
    }
}