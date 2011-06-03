using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinners
{
    using System.Web.Security;

    public static class NerdDinnerSecurity
    {
        /// <summary>
        /// Constant for identifying Administrator role
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// Constant for identifying DinnerCreator role
        /// </summary>
        public const string DinnerCreator = "DinnerCreator";

        /// <summary>
        /// Constant for identifying DinnerAttender role
        /// </summary>
        public const string DinnerAttender = "DinnerAttender";

        public static bool CanCreateDinner()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole(Administrator) 
                    || HttpContext.Current.User.IsInRole(DinnerCreator))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CanEditDinner(NerdDinnerDomain.Dinner dinner)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.Current.User.Identity.Name;
                if (HttpContext.Current.User.IsInRole(Administrator) 
                    || ////are a dinner creator, and you are the creator if this dinner
                        (HttpContext.Current.User.IsInRole(DinnerCreator)
                        && dinner != null 
                        && !string.IsNullOrEmpty(dinner.CreatedBy) 
                        && userName.Equals(dinner.CreatedBy, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CanDeleteDinner(NerdDinnerDomain.Dinner dinner)
        {
            return CanEditDinner(dinner);
        }

        public static bool CanAttendDinner()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole(Administrator) 
                    || HttpContext.Current.User.IsInRole(DinnerCreator)
                    || HttpContext.Current.User.IsInRole(DinnerAttender))
                {
                    return true;
                }
            }

            return false;
        }

        public static string CurrentUserEmail()
        {
            try
            {
                return Membership.GetUser(HttpContext.Current.User.Identity.Name).Email;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }
    }
}