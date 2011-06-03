//-----------------------------------------------------------------------
// <copyright file="NerdDinnerOperations.cs" company="AppliedIS">
//     Copyright AppliedIS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdDinnerDomain
{
    public class NerdDinnerOperations : INerdDinnerOperations
    {
        private static List<Dinner> _dinners = PopulateDinners();
        private static List<Rsvp> _rsvps = PopulateRSVPs();
        
        public List<Dinner> GetDinners()
        {
            return _dinners;
        }

        public DinnerSet GetFilteredDinners(int start, int count, string sortType, string sortColumn, string filter)
        {
            var totalDinnerCount = _dinners.Count;
            var filteredDinnerCount = _dinners.Count;
            var dinnersToReturn = _dinners.Skip(start).Take(count);

            var result = new DinnerSet() { Dinners = dinnersToReturn.ToList(), FilteredDinnerCount = filteredDinnerCount, TotalDinnerCount = totalDinnerCount };
            return result;
        }

        public Dinner GetDinner(int dinnerId)
        {
            return _dinners.Where(d => d.DinnerId == dinnerId).FirstOrDefault();
        }

        public Dinner CreateDinner(Dinner dinner)
        {
            if (_dinners.Where(a => a.DinnerId == dinner.DinnerId).Count() > 0)
                throw new Exception(string.Format("Dinner with Id:{0} already exists", dinner.DinnerId));

            _dinners.Add(dinner);
            return dinner;
        }

        public Dinner UpdateDinner(Dinner dinner)
        {
            var dinnerToUpdate = _dinners.Where(a => a.DinnerId == dinner.DinnerId).FirstOrDefault();

            if (dinnerToUpdate == null)
                throw new Exception(string.Format("Dinner with Id:{0} does not exist", dinner.DinnerId));

            _dinners.Remove(dinnerToUpdate);
            _dinners.Add(dinner);
            return dinner;
        }

        public void DeleteDinner(int dinnerId)
        {
            var dinnerToDelete = _dinners.Where(a => a.DinnerId == dinnerId).FirstOrDefault();

            if (dinnerToDelete == null)
                throw new Exception(string.Format("Dinner with Id:{0} does not exist", dinnerId));

            _dinners.Remove(dinnerToDelete);
        }

        public RsvpSet GetFilteredRSVPs(int dinnerId, int start, int count, string sortType, string sortColumn, string filter)
        {
            var rsvps = _rsvps.Where(a => a.DinnerId == dinnerId).ToList();

            var totalCount = rsvps.Count();
            var filteredCount = rsvps.Count;
            var toReturn = rsvps.Skip(start).Take(count);

            var result = new RsvpSet()
                             {FilteredRsvpCount = filteredCount, Rsvps = toReturn.ToList(), TotalRsvpCount = totalCount};
            return result;
        }

        private static List<Dinner> PopulateDinners()
        {
            var dinners = new List<Dinner>
            {
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=1, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=2, Title = "Sample Dinner 2", EventDate = DateTime.Parse("12/31/2012"), Address = "Somewhere Else", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=3, Title = "Sample Dinner 3", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=4, Title = "Sample Dinner 4", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=5, Title = "Sample Dinner 5", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=6, Title = "Sample Dinner 6", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=7, Title = "Sample Dinner 7", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=8, Title = "Sample Dinner 8", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=9, Title = "Sample Dinner 9", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
                new Dinner { CreatedBy = "JohnWickerham",DinnerId=10, Title = "Sample Dinner 10", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com" },
            };

            return dinners;
        }

        private static List<Rsvp> PopulateRSVPs()
        {
            var rsvps = new List<Rsvp>
            {
                new Rsvp() { RsvpId  = 1, DinnerId=1, AttendeeEmail = "Fred1@aol.com" },
                new Rsvp() { RsvpId  = 2, DinnerId=1, AttendeeEmail = "Fred2@aol.com" },
                new Rsvp() { RsvpId  = 3, DinnerId=1, AttendeeEmail = "Fred3@aol.com" },
                new Rsvp() { RsvpId  = 4, DinnerId=1, AttendeeEmail = "Fred4@aol.com" },
                new Rsvp() { RsvpId  = 5, DinnerId=1, AttendeeEmail = "Fred5@aol.com" },
                new Rsvp() { RsvpId  = 6, DinnerId=1, AttendeeEmail = "Fred6@aol.com" },
                new Rsvp() { RsvpId  = 7, DinnerId=1, AttendeeEmail = "Fred7@aol.com" },
                new Rsvp() { RsvpId  = 8, DinnerId=1, AttendeeEmail = "Fred8@aol.com" },
                new Rsvp() { RsvpId  = 9, DinnerId=1, AttendeeEmail = "Fred9@aol.com" },
                new Rsvp() { RsvpId  = 10, DinnerId=1, AttendeeEmail = "Fred10@aol.com" },
            };

            return rsvps;
        }


        public Rsvp CreateRsvp(Rsvp rsvp)
        {
            if (_dinners.Where(a => a.DinnerId == rsvp.DinnerId).Count() > 0)
                throw new Exception(string.Format("Rsvp with Id:{0} already exists", rsvp.RsvpId));

            _rsvps.Add(rsvp);
            return rsvp;
        }
    }
}
