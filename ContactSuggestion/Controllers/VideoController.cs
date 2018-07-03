using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactSuggestion.Models;
namespace ContactSuggestion.Controllers
{
    public class VideoController : Controller
    {
        // 
        // GET: /Video/
        public ActionResult Index()
        {
            return new VideoResult();
        }
    }
}