// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBNerdDinnerOperations.cs" company="AppliedIS">
//   All rights reserved
// </copyright>
// <summary>
//   Concrete imnplementation of INerdDinnerOperations
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinnerDomainOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NerdDinnerDomain;

    /// <summary>
    /// Concrete imnplementation of INerdDinnerOperations
    /// </summary>
    public class DBNerdDinnerOperations : INerdDinnerOperations
    {
        /// <summary>
        /// Get Dinners
        /// </summary>
        /// <returns>
        /// Returns a list of diners
        /// </returns>
        public List<Dinner> GetDinners()
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                var query = from dinners in nerdDinnersDb.Dinners
                            select dinners;
                return query.ToList();
            }
        }

        /// <summary>
        /// Get Filtered Dinners
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="sortType">
        /// The sort type.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <returns>
        /// A Dinenr Set
        /// </returns>
        public DinnerSet GetFilteredDinners(int start, int count, string filter, string sortType, string sortColumn)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                var totalDinnerCount = nerdDinnersDb.Dinners.Count();

                var filteredDinnerQuery = nerdDinnersDb.Dinners.AsQueryable();
                if (!string.IsNullOrEmpty(filter))
                {
                    filteredDinnerQuery =
                        from d in nerdDinnersDb.Dinners
                        where d.Title.Contains(filter)
                              || d.Address.Contains(filter)
                              || d.HostedBy.Contains(filter)
                        select d;
                }

                var filteredDinnerCount = filteredDinnerQuery.Count();
                var sortColumnLower = sortColumn.ToLower();

                Func<Dinner, string> orderingFunction =
                    d =>
                    sortColumnLower == "dinnerid" ? d.DinnerId.ToString()
                        : sortColumnLower == "title" ? d.Title
                              : sortColumnLower == "address" ? d.Address
                                    : sortColumnLower == "eventdate" ? d.EventDate.ToString()
                                        : d.Country;

                List<Dinner> sortedDinners;
                if (sortType == "asc")
                {
                    sortedDinners = (
                        from d in filteredDinnerQuery
                        select d).OrderBy(orderingFunction).ToList();
                }
                else if (sortType == "desc")
                {
                    sortedDinners =
                        (from d in filteredDinnerQuery
                        select d).OrderByDescending(orderingFunction).ToList();
                }
                else
                {
                    sortedDinners = filteredDinnerQuery.ToList();
                }

                var pageDinners = sortedDinners.Skip(start).Take(count);
                   
                var result = new DinnerSet()
                                 {
                                     Dinners = pageDinners.ToList(),
                                     FilteredDinnerCount = filteredDinnerCount,
                                     TotalDinnerCount = totalDinnerCount
                                 };
                return result;
            }
        }

        /// <summary>
        /// Returns one dinner
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        /// <returns>
        /// Returns a dinner
        /// </returns>
        public Dinner GetDinner(int dinnerId)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                var query = from dinners in nerdDinnersDb.Dinners
                            where dinners.DinnerId.Equals(dinnerId)
                            select dinners;

                var dinner = query.FirstOrDefault();

                var rsvpQuery = from r in nerdDinnersDb.RSVPs where r.DinnerId == dinnerId select r;

                dinner.RsvpCount = rsvpQuery.Count();

                return dinner;
            }
        }

        /// <summary>
        /// Creates a dinner
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// the dinner we created
        /// </returns>
        public Dinner CreateDinner(Dinner dinner)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                nerdDinnersDb.Dinners.Add(dinner);
                nerdDinnersDb.SaveChanges();
                return dinner;
            }
        }

        /// <summary>
        /// Update Dinner
        /// </summary>
        /// <param name="dinner">
        /// The dinner.
        /// </param>
        /// <returns>
        /// Returns a Dinner
        /// </returns>
        public Dinner UpdateDinner(Dinner dinner)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                nerdDinnersDb.Dinners.Attach(dinner);
                nerdDinnersDb.ChangeObjectState(dinner, System.Data.EntityState.Modified);
                nerdDinnersDb.SaveChanges();
            }

            return dinner;
        }

        /// <summary>
        /// Delete Dinner
        /// </summary>
        /// <param name="dinnerId">
        /// The dinner id.
        /// </param>
        public void DeleteDinner(int dinnerId)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                var query = from dinners in nerdDinnersDb.Dinners
                            where dinners.DinnerId.Equals(dinnerId)
                            select dinners;
                var dinner = query.FirstOrDefault();

                nerdDinnersDb.Dinners.Remove(dinner);
                nerdDinnersDb.SaveChanges();
            }
        }

        /// <summary>
        /// Returns a filtered set of RSVPs
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
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="sortType">
        /// The sort type.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <returns>
        /// A set of RSVPs
        /// </returns>
        public RsvpSet GetFilteredRSVPs(int dinnerId, int start, int count, string filter, string sortType, string sortColumn)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                var rsvps = nerdDinnersDb.RSVPs.Where(a => a.DinnerId == dinnerId);

                var totalCount = rsvps.Count();

                var filteredQuery = rsvps.AsQueryable();
                if (!string.IsNullOrEmpty(filter))
                {
                    filteredQuery =
                        from d in rsvps
                        where d.AttendeeEmail.Contains(filter)
                        select d;
                }

                var filteredCount = filteredQuery.Count();
                var sortColumnLower = sortColumn.ToLower();

                Func<Rsvp, string> orderingFunction =
                    d => d.AttendeeEmail.ToString();

                List<Rsvp> sortedRSVPs;
                if (sortType == "asc")
                {
                    sortedRSVPs = (
                        from d in filteredQuery
                        select d).OrderBy(orderingFunction).ToList();
                }
                else if (sortType == "desc")
                {
                    sortedRSVPs =
                        (from d in filteredQuery
                         select d).OrderByDescending(orderingFunction).ToList();
                }
                else
                {
                    sortedRSVPs = filteredQuery.ToList();
                }

                var pageRSVPs = sortedRSVPs.Skip(start).Take(count);

                var result = new RsvpSet()
                {
                    Rsvps = pageRSVPs.ToList(),
                    FilteredRsvpCount = filteredCount,
                    TotalRsvpCount = totalCount
                };
                return result;
            }
        }

        /// <summary>
        /// Creates an RSVP for a dinner
        /// </summary>
        /// <param name="rsvp">
        /// The rsvp object
        /// </param>
        /// <returns>
        /// The updated rsvp object
        /// </returns>
        public Rsvp CreateRsvp(Rsvp rsvp)
        {
            using (NerdDinnersDb nerdDinnersDb = new NerdDinnersDb())
            {
                nerdDinnersDb.RSVPs.Add(rsvp);
                nerdDinnersDb.SaveChanges();
                return rsvp;
            }
        }
    }
}
