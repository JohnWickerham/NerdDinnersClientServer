using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinnerDomain;

namespace NerdDinners.Controllers
{
    public class JSonTestController : Controller
    {
        //
        // GET: /JSonTest/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClickMe()
        {
            //var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            ////Anonymous type serialized
            //ViewBag.JsonAnonymous = oSerializer.Serialize(new { dinnerTitle = "Veal" }); ;

            ////strong type serialized
            var dinner = new NerdDinnerDomain.Dinner()
            {
                Title = "I'm a dinner",
            };
            //ViewBag.JsonDinner = oSerializer.Serialize(dinner);

            ////unserialized dinner
            //ViewBag.UnserializedDinner = dinner;


            var listOfDinners =
                new List<NerdDinnerDomain.Dinner>()
                    {
                        new Dinner() {Title = "dinner 1", Address="Somewhere one", Country = "USA", EventDate = DateTime.Parse("1/1/2012")},
                        new Dinner() {Title = "dinner 2", Address="Somewhere two", Country = "England", EventDate = DateTime.Parse("1/1/2013")},
                        new Dinner() {Title = "dinner 3", Address="Somewhere three", Country = "France", EventDate = DateTime.Parse("1/1/2014")},
                    };

            ViewBag.ListOfDinners = listOfDinners;
            return View("Index", dinner);
            //return Json(dinner, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxHandler( string sEchoParam, string iDisplayLength)
        {
            var listOfDinners =
                new List<NerdDinnerDomain.Dinner>()
                    {
                        new Dinner() {DinnerId = 1, Title = "dinner 1", Address="Somewhere one", Country = "USA", EventDate = DateTime.Parse("1/1/2012")},
                        new Dinner() {DinnerId = 2, Title = "dinner 2", Address="Somewhere two", Country = "England", EventDate = DateTime.Parse("1/1/2013")},
                        new Dinner() {DinnerId = 3, Title = "dinner 3", Address="Somewhere three", Country = "France", EventDate = DateTime.Parse("1/1/2014")},
                    };

            var result = from d in listOfDinners
                         select new []
                                    {
                                        Convert.ToString(d.DinnerId),
                                        d.Title, 
                                        d.Address, 
                                        d.Country, 
                                        d.EventDate.ToShortDateString()
                                    };
            return Json(new
            {
                sEcho = string.IsNullOrEmpty(sEchoParam) ? "1": sEchoParam,
                iTotalRecords = listOfDinners.Count(),
                iTotalDisplayRecords = listOfDinners.Count(),
                aaData = result
            },JsonRequestBehavior.AllowGet);


        }

    }
}
