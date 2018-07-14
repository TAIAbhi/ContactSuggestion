using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactSuggestion.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
       
       // Entities context = new Entities(); // my entity  
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            Source up = (Source)Globals.TheUserSession;            
            foreach (var role in allowedroles)
            {
                if(up.Role==Convert.ToInt32(role))
                {
                    authorize = true;
                }               
            }
           
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {            
            filterContext.Result = new RedirectResult("~/Login/Index");
        }
    }  
}