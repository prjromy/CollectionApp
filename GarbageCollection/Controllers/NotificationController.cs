using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using Loader;
using PagedList;
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
        [HttpPost]
        public ActionResult Pushpush()
        {
            FCMPushNotification fcmPush = new FCMPushNotification();
            fcmPush.SendNotification("sbsbs", "Your body message", "Sinamangal");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
            

        public ActionResult List()
        {
            try
            {
                NotificationViewModel customerViewModel = new NotificationViewModel();
                var customerList = notificationService.getCustomerList(null, 1, 10);
                customerViewModel.notificationpagedList = new StaticPagedList<NotificationViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

               

                return PartialView(customerViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _List(int? locationid,  int pageNo = 1, int pageSize = 10)
        {
            NotificationViewModel customerViewModel = new NotificationViewModel();
            var customerList = notificationService.getCustomerList(locationid, 1, 10);
            customerViewModel.notificationpagedList = new StaticPagedList<NotificationViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

       

            return PartialView(customerViewModel);
        }
    }
}