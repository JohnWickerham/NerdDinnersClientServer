// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INerdDinnerOperations.cs" company="AppliedIS">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the SortType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinnerDomain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides sorting types
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// No sorting
        /// </summary>
        None,

        /// <summary>
        /// Ascending sorting
        /// </summary>
        Asc,

        /// <summary>
        /// Descending sorting
        /// </summary>
        Desc
    }

    public interface INerdDinnerOperations
    {
        Dinner CreateDinner(Dinner dinner);
        List<Dinner> GetDinners();
        DinnerSet GetFilteredDinners(int start, int count, string filter, string sortType, string sortColumn);
        Dinner UpdateDinner(Dinner dinner);
        void DeleteDinner(int dinnerId);
        Dinner GetDinner(int dinnerId);
        RsvpSet GetFilteredRSVPs(int dinnerId, int start, int count, string filter, string sortType, string sortColumn);
        Rsvp CreateRsvp(Rsvp rsvp);
    }
}
