//-----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="AppliedIS">
//     Copyright AppliedIS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace RestfulNerds
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.ServiceModel.Activation;
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using NerdDinnerDomain;
    using NerdDinnerDomainOperations;
    using StructureMap;

    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer<NerdDinnersDb>(new NerdDinnersInitializer());
            
            ObjectFactory.Initialize(p => p.For<INerdDinnerOperations>().Use<DBNerdDinnerOperations>());
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("NerdService", new WebServiceHostFactory(), typeof(NerdService)));
        }


        public class NerdDinnersInitializer : System.Data.Entity.CreateDatabaseIfNotExists<NerdDinnersDb>
        {
            protected override void Seed(NerdDinnersDb context)
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
            }

            ICollection<Rsvp> GetRsvps(Dinner dinner)
            {
                var result = new List<Rsvp>();

                result.Add(
                    new Rsvp {AttendeeEmail = "One@Two.com"/*, Dinner = dinner*/});

                return result;
            }


            protected void SeedForTesting(NerdDinnersDb context)
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
            private string RandomString(int size, bool lowerCase)
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


        }

    }
}
