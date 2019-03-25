using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Prefeitura_Template.ActionFilter
{
    public class Filter
    {
    }
    public class AutenticacaoAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new
                    {
                        action = "Login",
                        Controller = "Usuarios",
                        area = "Admin"
                    }));
            }
        }
    }
}