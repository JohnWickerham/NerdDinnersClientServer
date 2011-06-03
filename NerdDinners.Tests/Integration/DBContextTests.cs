using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NerdDinnersDbContext;
using System.Data.Entity;
using NerdDinnerDomain;

namespace IntegerationTests
{
    using NerdDinnerDomainOperations;

    [TestClass]
    public class DomainTests
    {
        [TestMethod]
        public void TestGetAllDinnersReturnsAList()
        {
            DBNerdDinnerOperations operations = new DBNerdDinnerOperations();
            var dinners = operations.GetDinners();
            Assert.IsNotNull(dinners);            
        }

        [TestMethod]
        public void TestCreateDinner()
        {
            var newDinner = new Dinner { CreatedBy = "JohnWickerham", Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };

            DBNerdDinnerOperations operations = new DBNerdDinnerOperations();
            var dinner = operations.CreateDinner(newDinner);

            Assert.IsNotNull(dinner);
        }

        [TestMethod]
        public void TestGetRSVPsByDinnerId()
        {
            DBNerdDinnerOperations operations = new DBNerdDinnerOperations();
            var rsvps = operations.GetFilteredRSVPs(1, 0, 10, "", "asc", "AttendeeEmail");
            Assert.IsNotNull(rsvps.FilteredRsvpCount);            
        }


        //public class NerdDinnersInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<NerdDinnersDbContext.NerdDinnersDb>
        //{
        //    protected override void Seed(NerdDinnersDbContext.NerdDinnersDb context)
        //    {
        //        var dinners = new List<NerdDinnerDomainObjects.Dinner>
        //    {
        //        new NerdDinnerDomainObjects.Dinner { Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //        new NerdDinnerDomainObjects.Dinner { Title = "Sample Dinner 2", EventDate = DateTime.Parse("12/31/2012"), Address = "Somewhere Else", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //        new NerdDinnerDomainObjects.Dinner { Title = "Sample Dinner 3", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //    };

        //        dinners.ForEach(d => context.Dinners.Add(d));
        //    }
        //}
    }
}
