using System;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NerdDinners.Controllers;
using NerdDinners.Services;
using NerdDinnerDomain;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NerdDinnerDomain;
using StructureMap;

namespace NerdDinners.Tests.Controllers
{
    [TestClass]
    public class EndToEndTest
    {
        //INerdDinnerOperations operations = null;
        const string BaseUri = "http://localhost:3604/NerdService/";
        private NerdDinners.MvcApplication _application;

        [TestInitialize]
        public void Initialize()
        {
            SetupDatabase();

            var container = (IContainer)NerdDinners.IoC.Initialize();
            DependencyResolver.SetResolver(new NerdDinners.SmDependencyResolver(container));
        }


        [TestMethod]
        public void Test_Index_Returns_List_Of_Dinners()
        {
            // Arrange
            var controller = ObjectFactory.GetInstance<DinnerController>();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var dinners = (List<Dinner>)result.Model;

            // Assert
            Assert.AreEqual(result.ViewName, "Index");
            Assert.IsTrue(dinners.Count > 0);
        }


        [TestMethod]
        public void Test_Edit_By_Dinner_Id_Returns_Dinner()
        {
            // Arrange
            var controller = ObjectFactory.GetInstance<DinnerController>();
            ViewResult resultGet = controller.Index() as ViewResult;
            //var dinners = (List<Dinner>)resultGet.Model;
            var dinnerIdExpected = 1;

            // Act
            ViewResult result = controller.Edit(dinnerIdExpected) as ViewResult;
            var dinner = (Dinner)result.Model;

            // Assert
            Assert.AreEqual(dinner.DinnerId, dinnerIdExpected);
        }

        //[TestMethod]
        //public void Test_Create_Dinner_Returns_Index_View_And_Dinners()
        //{
        //    // Arrange
        //    var controller = ObjectFactory.GetInstance<DinnerController>();
        //    var newDinner = GetNewDinner();
        //    var service = (ObjectFactory.GetInstance<INerdDinnerService>());

        //    // Act
        //    var countOfDinnersBeforeCreate = service.GetDinners().Count;
        //    var result = controller.Create(newDinner) as RedirectToRouteResult;

        //    //Assert
        //    Assert.AreEqual(result.RouteValues["action"], "Index");
        //    Assert.IsTrue(service.GetDinners().Count == countOfDinnersBeforeCreate + 1);
        //}


        //[TestMethod]
        //public void Test_Edit_Dinner_Saves_Dinner()
        //{
        //    //Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();
        //    var changedDinner = GetNewDinner();
        //    moqService.Setup(a => a.UpdateDinner(changedDinner)).Returns(changedDinner);
        //    moqService.Setup(a => a.GetDinners()).Returns(new List<Dinner>() { changedDinner });
        //    var controller = new DinnerController(moqService.Object);

        //    //Act
        //    var viewResult = controller.Edit(changedDinner) as ViewResult;
        //    var dinners = (List<Dinner>)viewResult.Model;

        //    //Assert
        //    Assert.AreEqual(viewResult.ViewName, "Index");
        //    Assert.AreEqual(dinners[0].DinnerId, 1);
        //}


        //[TestMethod]
        //public void Test_Delete_Dinner_By_Id_Deletes_Dinner()
        //{
        //    //Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();
        //    moqService.Setup(a => a.GetDinners()).Returns(new List<Dinner>());
        //    moqService.Setup(a => a.DeleteDinner(1));
        //    var controller = new DinnerController(moqService.Object);

        //    //Act
        //    var viewResult = controller.Delete(1) as ViewResult;
        //    var dinners = (List<Dinner>)viewResult.Model;

        //    //Assert
        //    Assert.AreEqual(viewResult.ViewName, "Index");
        //    Assert.AreEqual(dinners.Where(a => a.DinnerId == 1).Count(), 0);
        //}

        //[TestMethod]
        //public void Test_Detail_By_Id()
        //{
        //    //Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();
        //    moqService.Setup(a => a.GetDinner(1)).Returns(GetNewDinner());
        //    var controller = new DinnerController(moqService.Object);

        //    //Act
        //    var result = controller.Detail(1) as PartialViewResult;
        //    var dinner = result.Model as Dinner;

        //    //Assert
        //    Assert.AreEqual(result.ViewName, "_DinnerDetails");
        //    Assert.AreEqual(dinner.DinnerId, 1);
        //}


        





        #region Private helper methods

        private Dinner GetNewDinner()
        {
            return new Dinner()
            {
                Address = "One Microsoft Way",
                Country = "USA",
                EventDate = DateTime.Parse("1/1/2010"),
                HostedBy = "Me@there.com",
                Title = "Good Food!"
            };
        }





        private static void SetupDatabase()
        {
            Database.SetInitializer<NerdDinnersDb>(new NerdDinnersDropCreateInitializer());
        }


        private class NerdDinnersDropCreateInitializer : System.Data.Entity.DropCreateDatabaseAlways<NerdDinnersDb>
        {
            protected override void Seed(NerdDinnersDb context)
            {
                LoadDefaultDinners(context);
            }
        }


        private static void LoadDefaultDinners(NerdDinnersDb context)
        {
            var dinners = new List<Dinner>
                                  {
                                      new Dinner
                                          {
                                              CreatedBy = "JohnWickerham",
                                              Title = "Sample Dinner 1",
                                              EventDate = DateTime.Parse("12/31/2010"),
                                              Address = "One Microsoft Way",
                                              Country = "USA",
                                              HostedBy = "scottgu@microsoft.com"
                                          },
                                      new Dinner
                                          {
                                              CreatedBy = "JohnWickerham",
                                              Title = "Sample Dinner 2",
                                              EventDate = DateTime.Parse("12/31/2012"),
                                              Address = "Somewhere Else",
                                              Country = "USA",
                                              HostedBy = "scottgu@microsoft.com"
                                          },
                                      new Dinner
                                          {
                                              CreatedBy = "JohnWickerham",
                                              Title = "Sample Dinner 3",
                                              EventDate = DateTime.Parse("12/31/2013"),
                                              Address = "Somewhere Else",
                                              Country = "USA",
                                              HostedBy = "scottgu@microsoft.com"
                                          },
                                  };

            dinners.ForEach(d => context.Dinners.Add(d));
        }
        #endregion
    }
}
