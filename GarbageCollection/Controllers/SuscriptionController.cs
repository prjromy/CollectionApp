using BuisnessObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinessLogic.Service;
using System.Data.Entity.Validation;
using PagedList;

namespace GarbageCollection.Controllers
{
    public class SuscriptionController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        private SuscriptionService suscription  = null;
        private CustomerService customerService = null;


        public SuscriptionController()
        {
            returnMessage = new ReturnBaseMessageModel();

            suscription = new SuscriptionService();
            customerService = new CustomerService();

        }
        // GET: Suscription
        public ActionResult Index()
        {
            MainViewModel.SubscriptionViewModel suscriptionModel = new MainViewModel.SubscriptionViewModel();
            return PartialView(suscriptionModel);
        }
        public ActionResult Create(int customerid,string custname)
        {
            MainViewModel.SubscriptionViewModel suscriptionModel = new MainViewModel.SubscriptionViewModel();
            suscriptionModel.CustId = customerid;
            suscriptionModel.CustomerName = custname;
            return PartialView(suscriptionModel);
        }
        [HttpPost]
        public ActionResult Create(MainViewModel.SubscriptionViewModel suscriptions)
        {


            try
            {


                returnMessage = suscription.Save(suscriptions);

                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException ex)
            {
                //returnMessage.Msg = "Not Saved" + ex.Message;
                //return returnMessage;
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw ex;

            }

        }

        public ActionResult List(int customerId)
        {
            try
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(customerId, "", "", "", null, 1, 10);
                customerViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriptionViewModel>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

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


        //public ActionResult _List(int? customerno, string name, string address, string contact, int? cType, int pageNo = 1, int pageSize = 10)
        //{
        //    MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
        //    var customerList = customerService.getCustomerList(customerno, name, address, contact, cType, pageNo, pageSize);
        //    customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
        //    return PartialView(customerViewModel);
        //}


    }
}