using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    [MyAuthorize]

    public class NotificationController : Controller
    {

        private NotificationService notificationService = null;
        public NotificationController()
        {
            notificationService = new NotificationService();
        }
        // GET: Notification
        public ActionResult NotificationIndex()
        {
            return PartialView();
        }

        public ActionResult GetMapCollector(int? LocationId)
        {
            try
            {
               
                var collector = notificationService.GetMappedCollector(LocationId);

                return Json(collector, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}