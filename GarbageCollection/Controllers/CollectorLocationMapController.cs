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

namespace GarbageCollection.Controllers
{
    [MyAuthorize]

    public class CollectorLocationMapController : Controller

    {
        private CollectorVsLocationService cl = null;
        private SuscriptionService suscription = null;
        public CollectorLocationMapController()
        {
            cl = new CollectorVsLocationService();
            suscription = new SuscriptionService();
        }
        [HttpPost]
        public ActionResult Index(CollectorLocationViewModel vm)
        {
            if (vm.Id != 0)
            {
                var message = cl.EditCollectionLocation(vm,Convert.ToInt32(vm.CollectorId));
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Save", new { @id = vm.locationNames, @collectorid = vm.CollectorId });

            }
        }
        [HttpPost]
        public ActionResult CheckLocationExist(int?CollectorId ,int? Id, int? LocationId,string locationNames=null, string locationname=null)

        {
            if (Id != 0)
            {
                var message = cl.EditCollectionLocationCheck(CollectorId, Id, LocationId,locationNames, locationname);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var message = cl.AddCollectionLocationCheck (CollectorId, LocationId,locationNames, locationname);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Display the selected attendee id
        /// </summary>
        [HttpGet]
        public ActionResult Save(string id,int collectorId)
        {
            //List<int> ls = new List<int>();
            try
            {
                List<CollectorLocationViewModel> vm = new List<CollectorLocationViewModel>();

            string[] intre = id.Split(',');
            foreach (var item in intre)
            {
                CollectorLocationViewModel single = new CollectorLocationViewModel();
                single.LocationId = Convert.ToInt32(item);
                vm.Add(single);
            }

               

                var message = cl.saveCollectionLocation(vm, collectorId);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// The method the ajax select2 query hits to get the attendees to display in the dropdownlist
        /// </summary>
        [HttpGet]
        public ActionResult GetAttendees(string searchTerm, int pageSize, int pageNum)
        {
            //Get the paged results and the total count of the results for this query. 
            AttendeeRepository ar = new AttendeeRepository();
            List<LocationMaster> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);
            int attendeeCount = ar.GetAttendeesCount(searchTerm, pageSize, pageNum);

            //Translate the attendees into a format the select2 dropdown expects
            Select2PagedResult pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<LocationMaster> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (LocationMaster a in attendees)
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.Lid.ToString(), text = a.LocationName });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    



    // GET: CollectorLocationMap
    public ActionResult CollectorLocationMapCreate(int? id)
         {

            CollectorLocationViewModel collectorLocation = new CollectorLocationViewModel();
            if (id != null)
            {
                collectorLocation = cl.singleCollectorLocation(id);

            }
            return PartialView(collectorLocation);
        }
        //public ActionResult CollectorLocationMapCreateSave(CollectorLocationViewModel collectorLocationViewModel)
        //{
        //    try
        //    {
        //        string[] locationIdList = collectorLocationViewModel.locationNames.Split(',');
         
     
        //        var message = cl.saveCollectionLocation(collectorLocationViewModel, locationIdList);
        //        return Json(message, JsonRequestBehavior.AllowGet);
        //    }
        //    catch(Exception e)
        //    {
        //        return Json(e, JsonRequestBehavior.AllowGet);
        //    }
          
        //}
        public ActionResult CollectorVsLocationList()
        {
            try
            {
                CollectorLocationViewModel customerViewModel = new CollectorLocationViewModel();
                var customerList = cl.getCollectorLocationList( null, null, 1, 10);
                customerViewModel.collectorLocatonList = new StaticPagedList<CollectorLocationViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}

                return PartialView(customerViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _CollectorVsLocationList(string collector, string location,  int pageNo = 1, int pageSize = 10)
        {
            CollectorLocationViewModel customerViewModel = new CollectorLocationViewModel();
            var customerList = cl.getCollectorLocationList(collector, location,   pageNo, pageSize);
            customerViewModel.collectorLocatonList = new StaticPagedList<CollectorLocationViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }

    
        public JsonResult LocationList(string term)
        {
            try
            {
                var LocationList = suscription.getLocation(term)/*.Select(x=>x.LocationName).ToList()*/;

                return Json(LocationList,JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {

                throw;
            }

        }

        //Extra classes to format the results the way the select2 dropdown wants them
        public class Select2PagedResult
        {
            public int Total { get; set; }
            public List<Select2Result> Results { get; set; }
        }

        public class Select2Result
        {
            public string id { get; set; }
            public string text { get; set; }
        }

    }
}