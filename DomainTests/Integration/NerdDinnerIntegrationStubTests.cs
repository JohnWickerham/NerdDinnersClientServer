//using System;
//using System.Net;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NerdDinnerDomain;
//using System.Runtime.Serialization;


//namespace NerdDinnerIntegrationStubTests
//{
//    [TestClass]
//    public class DomainTests
//    {
//        //INerdDinnerOperations operations = null;
//        string _baseURI = "http://localhost:3602/NerdService";

//        //[TestInitialize]
//        //public void Initialize()
//        //{
//        //    operations = new NerdDinnerOperations();
//        //}

//        //[TestCleanup]
//        //public void Cleanup()
//        //{
//        //    operations = null;
//        //}


//        [TestMethod]
//        public void Test_Get_All_Dinners_From_Returns_Non_Empty_List()
//        {
//            List<Dinner> dinners;
//            using (HttpResponseMessage response = new HttpClient().Get(_baseURI))
//            {
//                dinners = response.Content.ReadAsDataContract<List<Dinner>>();
//            }

//            Assert.IsTrue(dinners.Count > 0);
//        }

//        [TestMethod]
//        public void Test_Get_Dinner_By_Id_Returns_Correct_Dinner()
//        {
//            var dinnerIdExpected = 2;
//            var getByIdURI = _baseURI += "?dinnerId=" + dinnerIdExpected;

//            NerdDinnerDomain.Dinner dinner;
//            using (HttpResponseMessage response = new HttpClient().Get(getByIdURI))
//            {
//                dinner = response.Content.ReadAsDataContract<Dinner>();
//            }

//            Assert.AreEqual(dinner.DinnerId, dinnerIdExpected);
//        }

//        [TestMethod]
//        public void Test_Create_And_Get_Dinner()
//        {
//            var newDinnerId = 100;
//            var newDinner = new Dinner { DinnerId = newDinnerId, Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };

//            var creationURI = _baseURI += "/";

//            HttpContent content = HttpContentExtensions.CreateDataContract(newDinner);        
//            using (HttpResponseMessage response = new HttpClient().Post(creationURI, content))
//            {
//            }

//            Dinner dinner;
//            var getByIdURI = _baseURI += "?dinnerId=" + newDinnerId;
//            using (HttpResponseMessage response = new HttpClient().Get(getByIdURI))
//            {
//                dinner = response.Content.ReadAsDataContract<NerdDinnerDomain.Dinner>();
//            }

//            Assert.IsTrue(dinner.DinnerId == newDinnerId);
//        }

//        [TestMethod]
//        public void Test_Update_Dinner_And_Get_Dinner()
//        {
//            var updateURI = _baseURI += "/";

//            List<Dinner> dinners;
//            using (HttpResponseMessage response = new HttpClient().Get(_baseURI))
//            {
//                dinners = response.Content.ReadAsDataContract<List<NerdDinnerDomain.Dinner>>();
//            }

//            var dinnerToUpdate = dinners[0];
//            var addlTitleText = "some more text";
//            dinnerToUpdate.Title += addlTitleText;
//            var titleTextExpected = dinnerToUpdate.Title;

//            HttpContent content = HttpContentExtensions.CreateDataContract(dinnerToUpdate);
//            using (HttpResponseMessage response = new HttpClient().Put(updateURI, content))
//            {
//            }

//            Dinner dinner;
//            var getByIdURI = _baseURI += "?dinnerId=" + dinnerToUpdate.DinnerId;
//            using (HttpResponseMessage response = new HttpClient().Get(getByIdURI))
//            {
//                dinner = response.Content.ReadAsDataContract<NerdDinnerDomain.Dinner>();
//            }

//            Assert.AreEqual(dinner.Title, titleTextExpected);
//        }


//        [TestMethod]
//        public void Test_Delete_Dinner()
//        {
//            List<Dinner> dinners;
//            using (HttpResponseMessage response = new HttpClient().Get(_baseURI))
//            {
//                dinners = response.Content.ReadAsDataContract<List<NerdDinnerDomain.Dinner>>();
//            }

//            var dinnerToDelete = dinners[0];

//            var deleteURI = _baseURI += "?dinnerId=" + dinnerToDelete.DinnerId;
//            using (HttpResponseMessage response = new HttpClient().Delete(deleteURI))
//            {
//                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
//            }

//        }
        
//    }
//}
