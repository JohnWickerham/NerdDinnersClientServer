// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DinnerController.cs" company="AppliedIS">
//   All rights reserved.
// </copyright>
// <summary>
//   Dinner controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinners.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using Microsoft.Runtime.Serialization;
    using NerdDinnerDomain;
    using NerdDinnerDomain;
    using NerdDinners.Services;
    using StructureMap;

    /// <summary>
    /// Dinner controller
    /// </summary>
    public class DinnerController : Controller
    {
        /// <summary>
        /// Instance provides access to nerd dinners
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private INerdDinnerService _nerdDinnerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DinnerController"/> class.
        /// </summary>
        /// <param name="nerdDinnerService">
        /// The nerd dinner service.
        /// </param>
        public DinnerController(INerdDinnerService nerdDinnerService)
        {
            this._nerdDinnerService = nerdDinnerService;
        }

        /// <summary>
        /// Returns index view
        /// </summary>
        /// <returns>
        /// Index view
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Detail partial view for a specific dinenr
        /// </summary>
        /// <param name="id">
        /// The id corresponding to a dinner.
        /// </param>
        /// <returns>
        /// Returns a dinner view
        /// </returns>
        [OutputCache(VaryByParam = "id", Duration = 10)]
        public PartialViewResult Detail(int id)
        {
            var dinner = this._nerdDinnerService.GetDinner(id);
            this.ViewBag.UserButtonVisibility = NerdDinnerSecurity.CanEditDinner(dinner) ? "visible" : "hidden";

            return PartialView("_DinnerDetails", dinner);
        }

        [Authorize(Roles = "Administrator, DinnerAttender")]
        public PartialViewResult Attend(int id)
        {
            var dinner = this._nerdDinnerService.GetDinner(id);
            this.ViewBag.UserButtonVisibility = NerdDinnerSecurity.CanEditDinner(dinner) ? "visible" : "hidden";

            var rsvp = new Rsvp() { AttendeeEmail = NerdDinnerSecurity.CurrentUserEmail(), DinnerId = id };
            rsvp = this._nerdDinnerService.AttendDinner(rsvp);

            return PartialView("_DinnerDetails", dinner);            
        }

        /// <summary>
        /// Implements cancel
        /// </summary>
        /// <returns>
        /// Redirect to Index
        /// </returns>
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Implements the ajax call to populate the dinners grid.
        /// </summary>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <returns>
        /// JSON data for the grid
        /// </returns>
        public ActionResult AjaxHandlerDinner(jQueryDataTableParamModel param)
        {
            var sortDirection = Request["sSortDir_0"];

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumn = param.sColumns.Split(',')[sortColumnIndex];
            if (sortColumnIndex == 0)
            {
                sortColumn = "DinnerId";
            }

            var filter = param.sSearch;
            var dinnerSet = this._nerdDinnerService.GetFilteredDinners(
                param.iDisplayStart, param.iDisplayLength, sortDirection, sortColumn, filter);
            var dinnersToReturn = from d in dinnerSet.Dinners
                                  select
                                      new[]
                                          {
                                              Convert.ToString(d.DinnerId), this.AttendButtonHtml(d.DinnerId), d.Title, d.Address, d.EventDate.ToShortDateString(), d.Country,
                                          };

            var result =
                Json(
                    new
                        {
                            sEcho = param.sEcho,
                            iTotalRecords = dinnerSet.TotalDinnerCount,
                            iTotalDisplayRecords = dinnerSet.FilteredDinnerCount,
                            aaData = dinnersToReturn,
                        },
                    JsonRequestBehavior.AllowGet);

            return result;
        }

        private string AttendButtonHtml(int dinnerId)
        {
            if (NerdDinnerSecurity.CanAttendDinner())
            {
                return "<input class=\"attendbutton\" type=\"button\" value=\"Attend\" data-dinnerid=\"" + Convert.ToString(dinnerId) + "\"/>";
            }

            return "<input class=\"attendbutton\" type=\"button\" disabled=\"disabled\" value=\"Attend\" data-dinnerid=\"" + Convert.ToString(dinnerId) + "\"/>";
        }

        /// <summary>
        /// Implements the ajax call to populate the rsvp grid.
        /// </summary>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <param name="dinnerId">
        /// The o containing dinnerid.
        /// </param>
        /// <returns>
        /// JSON data for the grid
        /// </returns>
        public ActionResult AjaxHandlerRsvp(jQueryDataTableParamModel param, int dinnerId)
        {
            var sortDirection = Request["sSortDir_0"];

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumn = param.sColumns.Split(',')[sortColumnIndex];
            if (sortColumnIndex == 0)
            {
                sortColumn = "AttendeeEmail";
            }

            var filter = param.sSearch;

            var rsvpSet = this._nerdDinnerService.GetRsvps(dinnerId, param.iDisplayStart, param.iDisplayLength, sortDirection, sortColumn, filter);
            var rsvpsToReturn = from d in rsvpSet.Rsvps
                                  select
                                      new[]
                                          {
                                              d.AttendeeEmail,
                                          };

            var result =
                Json(
                    new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = rsvpSet.TotalRsvpCount,
                        iTotalDisplayRecords = rsvpSet.FilteredRsvpCount,
                        aaData = rsvpsToReturn,
                    },
                    JsonRequestBehavior.AllowGet);

            return result;
        }

        [Authorize(Roles = "Administrator, DinnerCreator")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a dinner
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// The view with a dinner in the model.
        /// </returns>
        [HttpPost]
        public ActionResult Create(Dinner dinner)
        {
            if (ModelState.IsValid)
            {
                var dinnerReturned = this._nerdDinnerService.CreateDinner(dinner);
                ViewBag.Result = "Your dinner was created successfully";

                return RedirectToAction("Index", new { dinnerId = dinnerReturned.DinnerId });
            }

            return View(dinner);
        }

        ////[NerdDinners.Filters.AuthenticatedAttribute] //If we use Authorize, then it forward you on to your original destination after logging in.

        /// <summary>
        /// GO to the edit view
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        /// <returns>
        /// The edit view with a dinner in the model
        /// </returns>
        [Authorize(Roles = "Administrator, DinnerCreator")]
        public ActionResult Edit(int dinnerId)
        {
            var dinner = this._nerdDinnerService.GetDinner(dinnerId);

            if (!NerdDinnerSecurity.CanEditDinner(dinner))
            {
                return RedirectToAction("LogOn", "Account");
            }

            ViewBag.Rsvps = this._nerdDinnerService.GetRsvps(dinnerId, 0, dinner.RsvpCount, "asc", "AttendeeEmail", string.Empty).Rsvps;


            return View(dinner);
        }

        /// <summary>
        /// Save a dinner.
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// Returns index view if successful.  Stays on edit view otherwise.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(Dinner dinner)
        {
            if (ModelState.IsValid)
            {
                var dinnerReturned = this._nerdDinnerService.UpdateDinner(dinner);
                return RedirectToAction("Index", new { dinnerId = dinnerReturned.DinnerId });
            }

            return View(dinner);
        }

        /// <summary>
        /// Implements delete of a dinner
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        /// <returns>
        /// Returns the index view.
        /// </returns>
        [Authorize(Roles = "Administrator, DinnerCreator")]
        public ActionResult Delete(int dinnerId)
        {
            var dinner = this._nerdDinnerService.GetDinner(dinnerId);
            if (!NerdDinnerSecurity.CanDeleteDinner(dinner))
            {
                return RedirectToAction("LogOn", "Account");
            }

            this._nerdDinnerService.DeleteDinner(dinnerId);
            return RedirectToAction("Index");
        }

        ///// <summary>
        ///// Determines if the current user is the same as the record was createdby.
        ///// </summary>
        ///// <param name="dinner">
        ///// The dinner.
        ///// </param>
        ///// <returns>
        ///// boolean flag true or false
        ///// </returns>
        //private bool CurrentUserEqualsCreatedBy(NerdDinnerDomain.Dinner dinner)
        //{
        //    if (string.IsNullOrEmpty(dinner.CreatedBy))
        //    {
        //        return false;
        //    }
            
        //    return dinner.CreatedBy.Equals(User.Identity.Name);
        //}

        /// <summary>
        /// Class that encapsulates most common parameters sent by DataTables plugin
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public class jQueryDataTableParamModel
        {
            /// <summary>
            /// Request sequence number sent by DataTable,
            /// same value must be returned in response
            /// </summary>       
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public string sEcho { get; set; }

            /// <summary>
            /// Text used for filtering
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public string sSearch { get; set; }

            /// <summary>
            /// Number of records that should be shown in table
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public int iDisplayLength { get; set; }

            /// <summary>
            /// First record that should be shown(used for paging)
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public int iDisplayStart { get; set; }

            /// <summary>
            /// Number of columns in table
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public int iColumns { get; set; }

            /// <summary>
            /// Number of columns that are used in sorting
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public int iSortingCols { get; set; }

            /// <summary>
            /// Comma separated list of column names
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
                "SA1623:PropertySummaryDocumentationMustMatchAccessors",
                Justification = "Reviewed. Suppression is OK here.")]
            public string sColumns { get; set; }
        }
    }
}