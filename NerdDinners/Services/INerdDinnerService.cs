// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INerdDinnerService.cs" company="AppliedIS">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the INerdDinnerService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinners.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NerdDinnerDomain;

    public interface INerdDinnerService
    {
        /// <summary>
        /// Gets a list of dinners by page, with filtering and sorting
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="sortType">
        /// The sort type.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// A dinnerSet
        /// </returns>
        DinnerSet GetFilteredDinners(int start, int count, string sortType, string sortColumn, string filter);
        ////List<Dinner> GetDinners();

        /// <summary>
        /// Returns a dinner with its rsvp
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        /// <returns>
        /// A DinnerClientModel with a set of RSVPs
        /// </returns>
        Dinner GetDinner(int dinnerId);

        /// <summary>
        /// Updates a dinner but not the rsvps
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// The updated dinner
        /// </returns>
        Dinner UpdateDinner(NerdDinnerDomain.Dinner dinner);

        /// <summary>
        /// Creates a new dinner but not the rsvps
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// The created dinner
        /// </returns>
        Dinner CreateDinner(NerdDinnerDomain.Dinner dinner);

        /// <summary>
        /// Deletes a dinner
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        void DeleteDinner(int dinnerId);

        /// <summary>
        /// Creates an rsvp record
        /// </summary>
        /// <param name="rsvp">
        /// The rsvp object
        /// </param>
        /// <returns>
        /// The rsvp with id.
        /// </returns>
        Rsvp AttendDinner(Rsvp rsvp);

        /// <summary>
        /// Get rsvps for a dinner
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="sortType">
        /// The sort type.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The set of rsvps
        /// </returns>
        RsvpSet GetRsvps(int dinnerId, int start, int count, string sortType, string sortColumn, string filter);
    }
}
