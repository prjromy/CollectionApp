using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    public class CollectorLocationMapController : Controller

    {
        private CollectorVsLocationService cl = null;
        private SuscriptionService suscription = null;
        public CollectorLocationMapController()
        {
            cl = new CollectorVsLocationService();
            suscription = new SuscriptionService();
        }
        // GET: CollectorLocationMap
        public ActionResult CollectorLocationMapCreate(int? id )
         {

            CollectorLocationViewModel collectorLocation = new CollectorLocationViewModel();
            if (id != null)
            {
                collectorLocation = cl.singleCollectorLocation(id);

            }
            return PartialView(collectorLocation);
        }
        public ActionResult CollectorLocationMapCreateSave(CollectorLocationViewModel collectorLocationViewModel)
        {
            try
            {
                string[] locationIdList = collectorLocationViewModel.locationNames.Split(',');
         
     
                var message = cl.saveCollectionLocation(collectorLocationViewModel, locationIdList);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(e, JsonRequestBehavior.AllowGet);
            }
          
        }
        public ActionResult CollectorVsLocationList()
        {
            try
            {
                CollectorLocationViewModel customerViewModel = new CollectorLocationViewModel();
                var customerList = cl.getCollectorLocationList( 0, 0, 1, 10);
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


        public ActionResult _CollectorVsLocationList(int? collector, int? location,  int pageNo = 1, int pageSize = 10)
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
       
    }
}