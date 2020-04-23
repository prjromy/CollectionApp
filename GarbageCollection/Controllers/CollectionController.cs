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
    public class CollectionController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        private CollectionService collection = null;
        private CustomerService customerService = null;


        public CollectionController()
        {
            returnMessage = new ReturnBaseMessageModel();

            collection = new CollectionService();
            customerService = new CustomerService();

        }
        public ActionResult Index() {
            MainViewModel.SubscriptionViewModel collection = new MainViewModel.SubscriptionViewModel();
            collection.ModelFrom = "Collection";
            return PartialView(collection);
        }

        public ActionResult CollectionEntry(int? customerId)

        {
            try
            {
                 MainViewModel.CollectionViewModel CollectionViewModel = new MainViewModel.CollectionViewModel();
                var suscriberList = collection.getSuscriberListEntry(customerId);
                foreach (var item in suscriberList)
                {
                    if (item.TotalCount == 1){
                        item.IsChecked = true;
                    }

                }
                CollectionViewModel.collectionList = suscriberList;

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(CollectionViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult Create(List<MainViewModel.CollectionViewModel> collecton)
        {
            var collectionResult = collection.CreateCollection(collecton);
            return Json(collectionResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult collectionEdit(int? Subscollid)
        {
            var collectionResult = collection.GetSingleCollection(Subscollid);
            ViewBag.CollectionDate = collectionResult.CollectionDate.Value.Month.ToString() + "-" + collectionResult.CollectionDate.Value.Day.ToString() + "-" + collectionResult.CollectionDate.Value.Year.ToString();
            return PartialView(collectionResult);
        }
        public ActionResult collectionEditSave(MainViewModel.CollectionVerificationEntry collections)
        {
            var collectionResult = collection.EditCollection(collections);
            return Json(collectionResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CollectionVerify(string CollectorName="",string LocationName="")
        {
            try
            {
                MainViewModel.CollectionVerificationEntry collectionViewModel = new MainViewModel.CollectionVerificationEntry();
                var collectionList = collection.getCollectionList(CollectorName, LocationName, 1, 10);
                collectionViewModel.collectionPagedList = new StaticPagedList<MainViewModel.CollectionVerificationEntry>(collectionList, 1, 10, (collectionList.Count == 0) ? 0 : collectionList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}

                return PartialView(collectionViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult _CollectionVerify(string CollectorName = "",  string LocationName = "",int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.CollectionVerificationEntry collectionViewModel = new MainViewModel.CollectionVerificationEntry();
            var collectionList = collection.getCollectionList(CollectorName, LocationName, pageNo, pageSize);
            collectionViewModel.collectionPagedList = new StaticPagedList<MainViewModel.CollectionVerificationEntry>(collectionList, pageNo, pageSize, (collectionList.Count == 0) ? 0 : collectionList.FirstOrDefault().TotalCount);

            //foreach (var item in customerList)
            //{
            //    customerViewModel.customerViewModelList.Add(item);
            //}

            return PartialView(collectionViewModel);
        }
        public ActionResult collectionVerifySave(List<MainViewModel.CollectionVerificationEntry> collectonS,decimal? amounttotal)
        {
            
                returnMessage = collection.VerifyCollection(collectonS, amounttotal);

          
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCollector(string prefix)
        {
            var collector = collection.getCollector(prefix);
            return Json(collector);

        }
    }
}