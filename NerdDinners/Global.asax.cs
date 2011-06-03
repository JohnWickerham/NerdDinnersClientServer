using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NerdDinners.Services;


namespace NerdDinners
{
    using System.Web.Security;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //go directly to dinner instead of home
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Dinner", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                //new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //new { controller = "JSonTest", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "LogOnToSite", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Account", action = "LogOn" }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            this.CreateRoles();
        }

        private void CreateRoles()
        {
            if (!Roles.RoleExists(NerdDinnerSecurity.Administrator))
            {
                Roles.CreateRole("Administrator");
            }

            if (!Roles.RoleExists(NerdDinnerSecurity.DinnerCreator))
            {
                Roles.CreateRole("DinnerCreator");
            }
            
            if (!Roles.RoleExists(NerdDinnerSecurity.DinnerAttender))
            {
                Roles.CreateRole("DinnerAttender");
            }

            if (Membership.GetUser("JohnWickerham") != null)
            {
                if (!Roles.IsUserInRole("JohnWickerham", "Administrator"))
                {
                    Roles.AddUsersToRole(new string[] { "JohnWickerham" }, NerdDinnerSecurity.Administrator);
                }
            }
        }
    }
}