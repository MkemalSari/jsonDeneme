using jsonDeneme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace jsonDeneme.Controllers
{
    public class HomeController : Controller
    {
        jsonTestEntities dbContext = new jsonTestEntities();
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Index(JsonDatas jsonDatas)
        {
            
              
           
           
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


      
    }
}