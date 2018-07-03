using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public static class Globals
    {
        public static Source TheUserSession
        {
            get
            {
                if ((HttpContext.Current.Session["UserDetails"] == null))
                {
                    HttpContext.Current.Session.Add("UserDetails", new Source());
                    return (Source)HttpContext.Current.Session["UserDetails"];
                }
                else
                {
                    return (Source)HttpContext.Current.Session["UserDetails"];
                }
            }
            set { HttpContext.Current.Session["UserDetails"] = value; }
        }

        public static void SignOut()
        {
            HttpContext.Current.Session["UserDetails"] = null;
            HttpContext.Current.Session.Abandon();
        }
    }
}