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
    }
}