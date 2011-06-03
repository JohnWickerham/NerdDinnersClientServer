//-----------------------------------------------------------------------
// <copyright file="NerdService.cs" company="AppliedIS">
//     Copyright AppliedIS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace RestfulNerds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using System.Text;
    using NerdDinnerDomain;
    using StructureMap;

    /// <summary>    
    /// Implementation of INerdService.  Provides dinners for nerds.
    /// </summary>    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]    
    public class NerdService : RestfulNerds.INerdService
    {
        private INerdDinnerOperations _operations;
        public NerdService():this(ObjectFactory.GetInstance<INerdDinnerOperations>())
        {
        }

        public NerdService(INerdDinnerOperations operations)
        {
            _operations = operations;
        }


        public DinnerSet GetSortedDinners(string start, string count, string sortType, string sortColumn)
        {
            string filter = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["filter"] ??
                            string.Empty;

            try
            {
                int iStart;
                if (!int.TryParse(start, out iStart))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredDinners operation failed: start must be numeric");
                    return null;
                }

                int iCount;
                if (!int.TryParse(count, out iCount))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredDinners operation failed: count must be numeric");
                    return null;
                }

                if (!(new List<string> { "asc", "desc", string.Empty, null }).Contains(sortType))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredDinners operation failed: sort type unrecognized");
                    return null;
                }

                if (!string.IsNullOrEmpty(sortType))
                {
                    var dinner = new Dinner();
                    var properties = dinner.GetType().GetProperties().Select(a => a.Name.ToLower()).ToList();
                    if (!properties.Contains(sortColumn.ToLower()))
                    {
                        SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredDinners operation failed: unrecognized sort column");
                        return null;
                    }
                }

                var result = _operations.GetFilteredDinners(iStart, iCount, filter, sortType, sortColumn);
                return result;
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredDinners operation failed: " + ex.Message);
            }
            return null;
        }

        public RsvpSet GetSortedRsvps(string dinnerId, string start, string count, string sortType, string sortColumn)
        {
            string filter = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["filter"] ??
                            string.Empty;
            
            try
            {
                int iDinnerId;
                if (!int.TryParse(dinnerId, out iDinnerId))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredRSVPs operation failed: dinnerId must be numeric");
                    return null;
                }

                int iStart;
                if (!int.TryParse(start, out iStart))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredRSVPs operation failed: start must be numeric");
                    return null;
                }

                int iCount;
                if (!int.TryParse(count, out iCount))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredRSVPs operation failed: count must be numeric");
                    return null;
                }

                if (!(new List<string> { "asc", "desc", string.Empty, null }).Contains(sortType))
                {
                    SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilGetFilteredRSVPsteredDinners operation failed: sort type unrecognized");
                    return null;
                }

                if (!string.IsNullOrEmpty(sortType))
                {
                    var rsvp = new Rsvp();
                    var properties = rsvp.GetType().GetProperties().Select(a => a.Name.ToLower()).ToList();
                    if (!properties.Contains(sortColumn.ToLower()))
                    {
                        SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredRSVPs operation failed: unrecognized sort column");
                        return null;
                    }
                }

                var result = this._operations.GetFilteredRSVPs(iDinnerId, iStart, iCount, filter, sortType, sortColumn);
                return result;
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "GetFilteredRSVPs operation failed: " + ex.Message);
            }
            return null;
        }

        public Dinner GetDinner(string dinnerId)
        {
            int id;
            if (!int.TryParse(dinnerId, out id))
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Get dinner operation failed: dinnerId must be numeric");
                return null;
            }

            try
            {
                var dinner = _operations.GetDinner(id);
                if (dinner == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
                return dinner;
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Get dinner operation failed");
            }

            return null;
        }

        public Dinner CreateDinner(Dinner dinner)
        {
            try
            {
                return _operations.CreateDinner(dinner);
            }
            catch (Exception ex)
            {
               SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Create dinner operation failed");
            }

            return null;
        }


        
        public Dinner UpdateDinner(Dinner dinner)
        {
            try
            {
                return _operations.UpdateDinner(dinner);
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Update dinner operation failed");
            }

            return null;
        }


        
        public void DeleteDinner(string dinnerId)
        {
            int id;
            if (!int.TryParse(dinnerId, out id))
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Get dinner operation failed: dinnerId must be numeric");
                return;
            }

            try
            {
                _operations.DeleteDinner(id);
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Delete dinner operation failed");
            }
        }


        private static void SetResponseHttpStatus(HttpStatusCode statusCode, string errorMessage)
        {
            var context = WebOperationContext.Current;
            if (context != null)
            {
                context.OutgoingResponse.StatusCode = statusCode;
                if (!string.IsNullOrEmpty(errorMessage))
                    context.OutgoingResponse.StatusDescription = errorMessage;    
            }
        }


        public Rsvp CreateRsvp(Rsvp rsvp)
        {
            try
            {
                return _operations.CreateRsvp(rsvp);
            }
            catch (Exception ex)
            {
                SetResponseHttpStatus(HttpStatusCode.InternalServerError, "Create rsvp operation failed");
            }

            return null;
        }
    }
}
