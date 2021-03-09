using BuisnessObject.ViewModel;
using BussinessLogic.CustomHelper;
using BussinessLogic.Repository;
using BussinessLogic.Service;
using DataAccess.DatabaseModel;
using Loader;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GarbageCollection.Controllers.CollectorLocationMapController;

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
        public ActionResult CollectionAmount()
        {
            return PartialView();
        }
        public ActionResult CollectionWasteEdit()
        {
            return PartialView();
        }
        public ActionResult GeneralMessage()
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
        public ActionResult Pushpush(DateTime? oldDate, DateTime? Date,string _topic,string Textarea, int? collectorid, string collectorname, int locationId,int notificationType)
        {
            FCMPushNotification fcmPush = new FCMPushNotification();
            var result=fcmPush.SendNotification(oldDate, Date,_topic,  Textarea, collectorid,  collectorname, locationId, notificationType);
            return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult getCollectorsLocationNames(int? locationid)
        {
            var result = notificationService.getCollectorsLocationName(locationid);
            return Json( notificationService.getCollectorsLocationName(locationid),JsonRequestBehavior.AllowGet);

        }


        
        public ActionResult _List(int? locationid,  int pageNo = 1, int pageSize = 10)
        {
            NotificationViewModel customerViewModel = new NotificationViewModel();
            var customerList = notificationService.getCustomerList(locationid, 1, 10);
            customerViewModel.notificationpagedList = new StaticPagedList<NotificationViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

       

            return PartialView(customerViewModel);
        }

        /// <summary>
        /// calender create
        /// </summary>
        /// <returns></returns>
        public ActionResult CalenderCreate()
        {
            return PartialView();
        }
        /// <summary>
        /// calender post
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(MainCalenderNotificationViewModel vm)
        {
            //if (vm.Id != 0)
            //{
            //    var message = notificationService.EditCollectionLocation(vm, Convert.ToInt32(vm.CollectorId));
            //    return Json(message, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
                return RedirectToAction("Save", new { @id = vm.weekDays, @locationId = vm.LocationId });

            //}
        }
        [HttpGet]
        public ActionResult Save(string id, int locationId)
        {
            //List<int> ls = new List<int>();
            try
            {
                List<MainCalenderNotificationViewModel> vm = new List<MainCalenderNotificationViewModel>();

                string[] intre = id.Split(',');
                foreach (var item in intre)
                {
                    MainCalenderNotificationViewModel single = new MainCalenderNotificationViewModel();
                    single.CollwkDay = Convert.ToInt32(item);
                    vm.Add(single);
                }



                var message = notificationService.saveNotification(vm, locationId);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e, JsonRequestBehavior.AllowGet);
            }


        }


        public ActionResult CalenderList()
        {
            try
            {
                MainCalenderNotificationViewModel customerViewModel = new MainCalenderNotificationViewModel();
                var calenderList = notificationService.getCalenderNotificationList(null, null, 1, 10);
                customerViewModel.calenderList = new StaticPagedList<MainCalenderNotificationViewModel>(calenderList, 1, 10, (calenderList.Count == 0) ? 0 : calenderList.FirstOrDefault().TotalCount);

             

                return PartialView(customerViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _CalenderList(string location, string weekday, int pageNo = 1, int pageSize = 10)
        {
            MainCalenderNotificationViewModel customerViewModel = new MainCalenderNotificationViewModel();
            var calenderList = notificationService.getCalenderNotificationList(location, weekday, pageNo, pageSize);
            customerViewModel.calenderList = new StaticPagedList<MainCalenderNotificationViewModel>(calenderList, pageNo, pageSize, (calenderList.Count == 0) ? 0 : calenderList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }
        ///for multiselect of weekdays
        ///
        [HttpGet]
        public ActionResult GetWeekDays(string searchTerm, int pageSize, int pageNum)
        {
            //Get the paged results and the total count of the results for this query. 
            WeekDayRepository ar = new WeekDayRepository();
            List<WeekDayList> attendees = ar.GetWeekDays(searchTerm, pageSize, pageNum);
            int attendeeCount = ar.GetAttendeesCount(searchTerm, pageSize, pageNum);

            //Translate the attendees into a format the select2 dropdown expects
            Select2PagedResult pagedAttendees = WeekDaysToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult WeekDaysToSelect2Format(List<WeekDayList> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (WeekDayList a in attendees)
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.Id.ToString(), text = a.Day });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }


        public JsonResult GetWeekDayList(string prefix)
        {
            try
            {
                var LocationList = notificationService.getWeekDayList(prefix);

                return Json(LocationList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {

                throw;
            }

        }


        public ActionResult CollectionWasteEditList(int year,int month,int location )
        {
            try
            {
                MainViewModel.CalenderMainViewModel calenderMainViewModel = new MainViewModel.CalenderMainViewModel();
                calenderMainViewModel.calenderMainViewModel = notificationService.getWasteCollectionCalenderList(year, month, location);
                        


                return PartialView(calenderMainViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}