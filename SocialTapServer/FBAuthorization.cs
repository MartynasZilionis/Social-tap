using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SocialTapServer
{
    public enum Role { Anonymous, User, Admin };
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class FBAuthorization : AuthorizationFilterAttribute
    {
        private Role[] allowedRoles;
        public FBAuthorization(params Role[] roles)
        {
            allowedRoles = roles;
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
            }
            else filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden); // <-- meti toki, jei tokenas neteisingas arba neatitinka role
            filterContext.Request.Headers.SingleOrDefault(x => x.Key == "Token");// <-- tokenas
            //allowedRoles <-- is cia gauni roles, kurios turi access
            //[...]
            
        }
    }
}