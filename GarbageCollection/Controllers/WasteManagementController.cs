using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    public class WasteManagementController : Controller
    {
        public ActionResult Index()
        {
            
            return File(Server.MapPath("/Views/Home/") + "Waste Management Privacy Policy.html", "text/html");
        }
    }
}