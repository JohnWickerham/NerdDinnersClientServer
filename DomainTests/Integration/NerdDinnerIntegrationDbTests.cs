using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NerdDinnerDomain;
using RestfulNerds;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using StructureMap;
//using System.IO;
//using Microsoft.Net.Http;

namespace NerdDinnerIntegrationDbTests
{
    using NerdDinnerDomainOperations;

    [TestClass]
    public class DomainTests
    {
        //INerdDinnerOperations operations = null;
        const string BaseUri = "http://localhost:3604/NerdService/";
        private WebServiceHost host;

        [TestInitialize]
        public void Initialize()
        {
            ObjectFactory.Initialize(p => p.For<INerdDinnerOperations>().Use<DBNerdDinnerOperations>());
            SetupDatabase();
            host = GetHost();
        }


        [TestCleanup]
        public void Cleanup()
        {
            host.Close();
        }

        [TestMethod]
        public void Test_Get_Filtered_RSVPs_First_10_Succeeds()
        {
            try
            {
                host.Open();
                var client = new HttpClient() { MaxResponseContentBufferSize = 196608 };

                var qst = BaseUri + "RsvpSearch/22/1/10/asc/AttendeeEmail?filter=";
                var responseGet = client.Get(qst);
                Assert.IsTrue(responseGet.IsSuccessStatusCode);

                var rsvpSet = GetRsvpSetFromResponse(responseGet);
                Assert.IsTrue(rsvpSet.TotalRsvpCount > 0);
                Assert.IsTrue(rsvpSet.FilteredRsvpCount > 0);
                Assert.IsTrue(rsvpSet.Rsvps.Count == 10);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }

        [TestMethod]
        public void Test_Get_Filtered_Dinners_2_For_3_With_Filter_Succeeds()
        {
            try
            {
                host.Open();
                var client = new HttpClient() { MaxResponseContentBufferSize = 196608 };

                var responseGet = client.Get(BaseUri + "DinnerSearch/2/3/asc/DinnerId?filter=a");
                Assert.IsTrue(responseGet.IsSuccessStatusCode);

                var dinnerSet = GetDinnerSetFromResponse(responseGet);
                Assert.IsTrue(dinnerSet.TotalDinnerCount > 90);
                Assert.IsTrue(dinnerSet.Dinners.Count == 3);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }

        [TestMethod]
        public void Test_Get_Filtered_Dinners_2_For_3_Without_Filter_Succeeds()
        {
            try
            {
                host.Open();
                var client = new HttpClient() { MaxResponseContentBufferSize = 196608 };

                var responseGet = client.Get(BaseUri + "DinnerSearch/2/3/asc/DinnerId");
                Assert.IsTrue(responseGet.IsSuccessStatusCode);

                var dinnerSet = GetDinnerSetFromResponse(responseGet);
                Assert.IsTrue(dinnerSet.TotalDinnerCount > 90);
                Assert.IsTrue(dinnerSet.Dinners.Count == 3);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }

       
        [TestMethod]
        public void Test_Get_Dinner_Succeeds_For_Valid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdExpected = 1; 
                var responseGetSpecific = client.Get(BaseUri + "Dinner/" + dinnerIdExpected);
                Assert.IsTrue(responseGetSpecific.IsSuccessStatusCode);

                var dinner = GetSingleDinnerFromResponse(responseGetSpecific);

                Assert.AreEqual(dinnerIdExpected, dinner.DinnerId);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        [TestMethod]
        public void Test_Get_Dinner_Fails_For_Invalid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdExpected = -1;
                var responseGetSpecific = client.Get(BaseUri + "Dinner/" + dinnerIdExpected);
                Assert.IsFalse(responseGetSpecific.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        [TestMethod]
        public void Test_Create_Succeeds_With_Valid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var newDinner = new Dinner { CreatedBy = "JohnWickerham", Title = "Sample Dinner 1", EventDate = DateTime.Parse("12/31/2010"), Address = "One Microsoft Way", Country = "USA", HostedBy = "scottgu@microsoft.com" };

                HttpContent content = this.CreateDinnerContent(newDinner);
                var response = client.Post(BaseUri + "Dinner/", content);
                Assert.IsTrue(response.IsSuccessStatusCode);

                var dinnerReturned = GetSingleDinnerFromResponse(response);
                Assert.IsNotNull(dinnerReturned);
                Assert.AreNotEqual(dinnerReturned.DinnerId, 0);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        [TestMethod]
        public void Test_Create_Fails_With_Invalid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                Dinner newDinner = GetInvalidDinner();

                HttpContent content = this.CreateDinnerContent(newDinner);
                var response = client.Post(BaseUri + "Dinner/", content);
                Assert.IsFalse(response.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }

       


        [TestMethod]
        public void Test_Update_Succeeds_With_Valid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdExpected = 1;
                var responseGetSpecific = client.Get(BaseUri + "Dinner/" + dinnerIdExpected);
                var dinnerToUpdate = GetSingleDinnerFromResponse(responseGetSpecific);

                var dateTimeExpected = DateTime.Now;

                //Modify eventdate
                dinnerToUpdate.EventDate = dateTimeExpected;
                
                //Update the dinner
                HttpContent content = this.CreateDinnerContent(dinnerToUpdate);
                var responsePut = client.Put(BaseUri + "Dinner/", content);
                Assert.IsTrue(responsePut.IsSuccessStatusCode);

                //Check the response
                var dinnerReturned = GetSingleDinnerFromResponse(responsePut);
                Assert.IsNotNull(dinnerReturned);
                Assert.AreEqual(dinnerReturned.EventDate, dateTimeExpected);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }



        [TestMethod]
        public void Test_Update_Fails_With_Invalid_Dinner()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdExpected = 1;
                var responseGetSpecific = client.Get(BaseUri + "Dinner/" + dinnerIdExpected);
                var dinnerToUpdate = GetSingleDinnerFromResponse(responseGetSpecific);

                //Modify the title, making it too long so that the save will fail
                dinnerToUpdate.Title = new string('*', 100);

                //Update the dinner
                HttpContent content = this.CreateDinnerContent(dinnerToUpdate);
                var responsePut = client.Put(BaseUri + "Dinner/", content);
                Assert.IsFalse(responsePut.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        [TestMethod]
        public void Test_Delete_Of_Existing_Dinner_Succeeds()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdExpected = 50;
                var responseGetSpecific = client.Get(BaseUri + "Dinner/" + dinnerIdExpected);
                var dinnerToDelete = GetSingleDinnerFromResponse(responseGetSpecific);

                //Delete it
                var responseDelete = client.Delete(BaseUri + "Dinner/" + dinnerToDelete.DinnerId);
                Assert.IsTrue(responseDelete.IsSuccessStatusCode);

                //Get the dinner explicitly and see if our changes were saved
                var responseGetDinner = client.Get(BaseUri + "Dinner/" + dinnerToDelete.DinnerId);
                Assert.IsFalse(responseGetDinner.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        [TestMethod]
        public void Test_Delete_Of_Non_Existing_Dinner_Fails()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var dinnerIdToDelete = -1;

                //Delete it
                var responseDelete = client.Delete(BaseUri + "Dinner/" + dinnerIdToDelete);
                Assert.IsFalse(responseDelete.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }

        [TestMethod]
        public void Test_Create_Succeeds_With_Valid_Rsvp()
        {
            try
            {
                host.Open();
                var client = new HttpClient { MaxResponseContentBufferSize = 196608 };

                var newRsvp = new Rsvp() { AttendeeEmail = "fred@flinstone.net", DinnerId = 1 };

                HttpContent content = this.CreateRsvpContent(newRsvp);
                var response = client.Post(BaseUri + "Rsvp/", content);
                Assert.IsTrue(response.IsSuccessStatusCode);

                var rsvpReturned = GetSingleRsvpFromResponse(response);
                Assert.IsNotNull(rsvpReturned);
                Assert.AreNotEqual(rsvpReturned.RsvpId, 0);
            }
            catch (Exception ex)
            {
                host.Abort();
                Assert.Fail(ex.Message);
            }
            finally
            {
                host.Close();
            }
        }


        //[TestMethod]
        //public void Test_Get_HttpClient()
        //{
        //    try
        //    {
        //        host.Open();
        //        var client = new HttpClient { MaxResponseContentBufferSize = 500000 };

        //        var responseGet = client.Get(BaseUri + "Dinner/");
        //        Assert.IsTrue(responseGet.IsSuccessStatusCode);

        //        List<Dinner> dinners = GetDinnersFromResponse(responseGet);
        //        Assert.IsTrue(dinners.Count > 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        host.Abort();
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        host.Close();
        //    }
        //}

        //[TestMethod]
        //public void Test_Get_All_Dinners_From_Returns_Non_Empty_List()
        //{
        //    try
        //    {
        //        host.Open();
        //        ChannelFactory<INerdService> channelFactory = 
        //            new ChannelFactory<INerdService>(new WebHttpBinding(), new EndpointAddress(BaseUri));
        //        channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
        //        var channel = channelFactory.CreateChannel();
        //        var dinners = channel.GetDinners();

        //        Assert.IsTrue(dinners.Count > 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        host.Abort();
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        host.Close();
        //    }
        //}
        #region Private helper methods

        private Dinner GetInvalidDinner()
        {
            return new Dinner
                       {
                           CreatedBy = "JohnWickerham",
                           Title = "Sample Dinner 1",
                            /*EventDate = DateTime.Parse("12/31/2010")*/
                           Address = "One Microsoft Way",
                           Country = "USA",
                           HostedBy = "scottgu@microsoft.com"
                       };
        }

        private HttpContent CreateDinnerContent(Dinner dinnerToUpdate)
        {
            DataContractSerializer singleSerializer = new DataContractSerializer(typeof(Dinner));
            HttpContent content = dinnerToUpdate.ToContentUsingDataContractSerializer(singleSerializer);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
            return content;
        }

        private HttpContent CreateRsvpContent(Rsvp rsvpToUpdate)
        {
            DataContractSerializer singleSerializer = new DataContractSerializer(typeof(Rsvp));
            HttpContent content = rsvpToUpdate.ToContentUsingDataContractSerializer(singleSerializer);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
            return content;
        }

        private static List<Dinner> GetDinnersFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(List<Dinner>));
            return (List<Dinner>)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static DinnerSet GetDinnerSetFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(DinnerSet));
            return (DinnerSet)serializer.ReadObject(response.Content.ContentReadStream);
        }

        private static RsvpSet GetRsvpSetFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(RsvpSet));
            return (RsvpSet)serializer.ReadObject(response.Content.ContentReadStream);
        }

        private static Dinner GetSingleDinnerFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(Dinner));
            return (Dinner)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static Rsvp GetSingleRsvpFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(Rsvp));
            return (Rsvp)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static WebServiceHost GetHost()
        {
            //http://localhost:3604/NerdService/
            WebServiceHost host = new WebServiceHost(typeof(NerdService), new Uri(BaseUri));
            host.AddServiceEndpoint(typeof(INerdService), new WebHttpBinding(), "");
            return host;
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

        
        private class NerdDinnersDropCreateIfChangedInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<NerdDinnersDb>
        {
            protected override void Seed(NerdDinnersDb context)
            {
                LoadDefaultDinners(context);
            }
        }

        private static void LoadDefaultDinners(NerdDinnersDb context)
        {
            SeedForTesting(context);  //adds the dinners that need to be there for our end to end tests to pass.

            var NumberOfDinners = 100;
            var dinnerTemplate = new Dinner
            {
                CreatedBy = "JohnWickerham",
                Title = "Sample Dinner 1",
                EventDate = DateTime.Parse("12/31/2010"),
                Address = "One Microsoft Way",
                Country = "USA",
                HostedBy = "scottgu@microsoft.com"
            };

            var addresses = new string[] { "1 Somewhere Street", "2 Nowhere Lane", "100 Elm Street", "900 Executive Blvd" };
            var dates = new DateTime[4];
            dates[0] = DateTime.Parse("1/1/2011");
            dates[1] = DateTime.Parse("2/1/2011");
            dates[2] = DateTime.Parse("3/1/2011");
            dates[3] = DateTime.Parse("4/1/2011");

            var rand = new Random(1);

            for (int ii = 0; ii < NumberOfDinners; ii++)
            {
                var d = new Dinner();

                d.Address = addresses[rand.Next(4)];
                d.Country = dinnerTemplate.Country;
                d.CreatedBy = "JohnWickerham";
                d.EventDate = dates[rand.Next(4)];
                d.Title = RandomString(20, true);
                d.HostedBy = dinnerTemplate.HostedBy;

                context.Dinners.Add(d);
            }

            context.SaveChanges();

            //Add rsvps
            foreach (var d in context.Dinners.ToList())
            {
                int iStart = rand.Next(1000);
                for (int ii = iStart; ii < iStart+20; ii++)
                {
                    context.RSVPs.Add(new Rsvp {AttendeeEmail = "Fred" + ii + "@aol.com", DinnerId = d.DinnerId});
                }
            }

        }

        private static ICollection<Rsvp> GetRsvps(Dinner dinner)
        {
            var result = new List<Rsvp>();

            result.Add(
                new Rsvp { AttendeeEmail = "One@Two.com"/*, Dinner = dinner*/});

            return result;
        }


        protected static void SeedForTesting(NerdDinnersDb context)
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



        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        #endregion

    }
}
