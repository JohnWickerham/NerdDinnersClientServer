using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NerdDinnerDomain;

using System.Runtime.Serialization;


namespace NerdInnerUnitTests
{
    [TestClass]
    public class DomainTests
    {
        INerdDinnerOperations operations = null;

        [TestInitialize]
        public void Initialize()
        {
            operations = new NerdDinnerOperations();
        }

        [TestCleanup]
        public void Cleanup()
        {
            operations = null;
        }

        [TestMethod]
        public void Test_Get_Dinner_ById()
        {
            Dinner expected = new Dinner { DinnerId = 1 };
            Dinner actual = operations.GetDinner(1);

            Assert.AreEqual(expected.DinnerId, actual.DinnerId);
        }


        [TestMethod]
        public void Test_Get_All_Dinners_Returns_A_List()
        {
            Dinner expected = new Dinner { DinnerId = 1 };
            var list = operations.GetDinners();
            Dinner result = list[0];
            Assert.AreEqual(expected.DinnerId, result.DinnerId);
        }

        [TestMethod]
        public void Test_Get_Dinner_Set_Of_First_Five_Returns_Expected_Set()
        {
            DinnerSet dinnerSet = operations.GetFilteredDinners(0, 5, string.Empty, "asc", string.Empty);
            Assert.AreEqual(dinnerSet.Dinners.Count, 5);
        }

        //[TestMethod]
        //public void Test_Create_Dinner_And_Get_Dinner()
        //{
        //    var newDinnerId = 100;
        //    var newDinner = new Dinner { DinnerId=newDinnerId, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };
        //    operations.CreateDinner(newDinner);
        //    var dinner = operations.GetDinners().Where(a => a.DinnerId == newDinnerId).FirstOrDefault();

        //    Assert.AreEqual(dinner.DinnerId, newDinnerId);
        //}

        //[TestMethod]
        //public void Test_Create_Duplicate_Dinner_Throws_Exception()
        //{
        //    var newDinnerId = 100;
        //    var newDinner = new Dinner { DinnerId = newDinnerId, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };
        //    var threwException = false;
        //    try
        //    {
        //        operations.CreateDinner(newDinner);
        //        operations.CreateDinner(newDinner);
        //    }
        //    catch
        //    {
        //        threwException = true;
        //    }

        //    Assert.IsTrue(threwException);
        //}


        //[TestMethod]
        //public void Test_Delete_Dinner_Deletes_Dinner()
        //{
        //    var newDinnerId = 100;
        //    var newDinner = new Dinner { DinnerId = newDinnerId, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };

        //    try
        //    {
        //        operations.CreateDinner(newDinner);
        //    }
        //    catch
        //    {}

        //    var threwException = false;
        //    try
        //    {
        //        operations.DeleteDinner(newDinnerId);
        //    }
        //    catch
        //    {
        //        threwException = true;
        //    }

        //    Assert.IsFalse(threwException);
        //}

        //[TestMethod]
        //public void Test_Update_Dinner_Updates_Dinner()
        //{
        //    var dinner = operations.GetDinners().FirstOrDefault();
        //    int dinnerId;

        //    if (dinner == null)
        //    {
        //        dinnerId = 100;
        //        dinner = new Dinner { DinnerId = dinnerId, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };
        //        operations.CreateDinner(dinner);
        //    }
        //    dinnerId = dinner.DinnerId;

        //    var titleToCompare = dinner.Title + ":a new title";
        //    dinner.Title = titleToCompare;
        //    operations.UpdateDinner(dinner);

        //    var updatedDinner = operations.GetDinners().Where(a => a.DinnerId == dinnerId).FirstOrDefault();
        //    Assert.IsTrue(updatedDinner.Title == titleToCompare);
        //}

        //private void PopulateDinners()
        //{
        //    var dinners = new List<Dinner>
        //    {
        //        new Dinner { Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //        new Dinner { Title = "Sample Dinner 2", EventDate = DateTime.Parse("12/31/2012"), Address = "Somewhere Else", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //        new Dinner { Title = "Sample Dinner 3", EventDate = DateTime.Parse("12/31/2013"), Address = "Somewhere Else2", Country = "USA", HostedBy="scottgu@microsoft.com"},
        //    };

        //    Dinners = dinners;
        //}

    }
}
