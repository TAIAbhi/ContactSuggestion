using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactSuggestion.Controllers
{
    public class FAQController : Controller
    {
        // GET: FAQ
        public ActionResult FAQ(string mobile , int city )
        {
            return View();
        }
    }
}