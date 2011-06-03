using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NerdDinners.Controllers;
using NerdDinners.Services;
using NerdDinnerDomain;

namespace NerdDinners.Tests.Controllers
{
    [TestClass]
    public class DinnerControllerTest
    {

        //[TestMethod]
        //public void Test_Index_Returns_List_Of_Dinners()
        //{
        //    // Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();

        //    var mockDinners = new Moq.Mock<List<Dinner>>();

        //    moqService.Setup(x => x.GetDinners()).Returns(
        //        new List<Dinner>()
        //            {
        //                GetSomeDinnerWithDinnerIdOfOne()
        //            });
        //    DinnerController controller = new DinnerController(moqService.Object);

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;
        //    var dinners = (List<Dinner>)result.Model;

        //    // Assert
        //    Assert.AreEqual(result.ViewName, "Index");
        //    Assert.AreEqual(dinners.Count, 1);
        //}

        [TestMethod]
        public void Test_Edit_By_Dinner_Id_Returns_Dinner()
        {
            // Arrange
            var moqService = new Moq.Mock<INerdDinnerService>();
            moqService.Setup(x => x.GetDinner(1)).Returns(GetSomeDinnerWithDinnerIdOfOne());

            DinnerController controller = new DinnerController(moqService.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var dinner = (Dinner)result.Model;

            // Assert
            Assert.AreEqual(dinner.DinnerId, 1);
        }

        //[TestMethod]
        //public void Test_Create_Dinner_Returns_Index_View_And_Dinners()
        //{
        //    // Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();
        //    var newDinner = GetSomeDinnerWithDinnerIdOfOne();
        //    moqService.Setup(x => x.CreateDinner(newDinner)).Returns(newDinner);
        //    moqService.Setup(x => x.GetDinners()).Returns(new List<Dinner>(){newDinner});
        //    DinnerController controller = new DinnerController(moqService.Object);

        //    // Act
        //    var result = controller.Create(newDinner) as RedirectToRouteResult;
            
        //    //Assert
        //    var dinnerId = result.RouteValues["dinnerId"];
        //    var action = result.RouteValues["action"];
        //    Assert.AreEqual(action, "Index");
        //    Assert.AreEqual(dinnerId, newDinner.DinnerId);
        //}

        [TestMethod]
        public void Test_Edit_Redirects_With_Incorrect_User()
        {
            // Arrange
            var moqService = new Moq.Mock<INerdDinnerService>();
            var expectedDinner = GetSomeDinnerWithDinnerIdOfOne();
            expectedDinner.CreatedBy = "system";
            moqService.Setup(a => a.GetDinner(expectedDinner.DinnerId)).Returns(expectedDinner);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(p => p.User.Identity.Name).Returns("blablabla");
            var controller = new DinnerController(moqService.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var result = controller.Edit(expectedDinner.DinnerId) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "LogOn");
            Assert.AreEqual(result.RouteValues["controller"], "Account");
        }

        [TestMethod]
        public void Test_Edit_Redirects_With_Correct_User()
        {
            // Arrange
            var moqService = new Moq.Mock<INerdDinnerService>();
            var expectedDinner = GetSomeDinnerWithDinnerIdOfOne();
            expectedDinner.CreatedBy = "system";
            moqService.Setup(a => a.GetDinner(expectedDinner.DinnerId)).Returns(expectedDinner);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(p => p.User.Identity.Name).Returns(expectedDinner.CreatedBy);
            var controller = new DinnerController(moqService.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var result = controller.Edit(expectedDinner.DinnerId) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void Test_Edit_Dinner_Saves_Dinner()
        //{
        //    //Arrange
        //    var moqService = new Moq.Mock<INerdDinnerService>();
        //    var changedDinner = GetSomeDinnerWithDinnerIdOfOne();
        //    moqService.Setup(a => a.UpdateDinner(changedDinner)).Returns(changedDinner);
        //    moqService.Setup(a => a.GetDinners()).Returns(new List<Dinner>() {changedDinner});
        //    var controller = new DinnerController(moqService.Object);

        //    //Act
        //    var result = controller.Edit(changedDinner) as RedirectToRouteResult;

        //    //Assert
        //    var dinnerId = result.RouteValues["dinnerId"];
        //    var action = result.RouteValues["action"];
        //    Assert.AreEqual(action, "Index");
        //    Assert.AreEqual(dinnerId, changedDinner.DinnerId);
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
        //    var result = controller.Delete(1) as RedirectToRouteResult;

        //    //Assert
        //    var action = result.RouteValues["action"];
        //    Assert.AreEqual(action, "Index");
        //}

        [TestMethod]
        public void Test_Detail_By_Id()
        {
            //Arrange
            var moqService = new Moq.Mock<INerdDinnerService>();
            moqService.Setup(a => a.GetDinner(1)).Returns(GetSomeDinnerWithDinnerIdOfOne());
            var controller = new DinnerController(moqService.Object);

            //Act
            var result = controller.Detail(1) as PartialViewResult;
            var dinner = result.Model as Dinner;

            //Assert
            Assert.AreEqual(result.ViewName, "_DinnerDetails");
            Assert.AreEqual(dinner.DinnerId, 1);
        }
        

        private Dinner GetSomeDinnerWithDinnerIdOfOne()
        {
            return new Dinner()
            {
                Address = "One Microsoft Way",
                Country = "USA",
                DinnerId = 1,
                EventDate = DateTime.Parse("1/1/2010"),
                HostedBy = "Me",
                Title = "Good Food!"
            };
        }

    }

}
