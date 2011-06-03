// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticatedAttribute.cs" company="AppliedIS">
//   All rights reserved
// </copyright>
// <summary>
//   Authenticated Attribute
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinners.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Authenticated Attribute
    /// </summary>
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ////http://localhost:50357/Account/LogOn
                ////filterContext.Result = new RedirectToRouteResult("LogOnToSite", null);
                filterContext.Result = new RedirectResult("~/Account/LogOn");
                ////filterContext.Result = new RedirectToRouteResult("LogOnToSite", new RouteValueDictionary());
            }            
        }
    }
}