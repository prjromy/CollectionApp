using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using System.Data.Entity.Validation;
using PagedList;
using BussinessLogic.CustomHelper;
using Loader;

namespace GarbageCollection.Controllers
{
    [MyAuthorize]
    public class CustomerController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        private CustomerService customerService = null;
        private SuscriptionService suscription = null;


        public CustomerController()
        {
            returnMessage = new ReturnBaseMessageModel();
            suscription = new SuscriptionService();
            customerService = new CustomerService();


        }
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Create(int? cId)
        {
            MainViewModel.CustomerViewModel customer = new MainViewModel.CustomerViewModel();
            if (cId != 0)
            {
                customer = customerService.getSingleCustomerList(cId);
            }
            return PartialView(customer);
        }


        public ActionResult UpdateStatus(int? cId)
        {


            try
            {


                returnMessage = customerService.changeStatus(cId);

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

        [HttpPost]
        public ActionResult Create(MainViewModel.CustomerViewModel customer)
        {


            try
            {


                returnMessage = customerService.Save(customer);

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

        public ActionResult List()
        {
            try
            {
                MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
                var customerList = customerService.getCustomerList(0, "", "", "", null, 1, 1, 10);
                customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

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


        public ActionResult _List(int? customerno, string name, string address, string contact, int? cType, int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
            var customerList = customerService.getCustomerList(customerno, name, address, contact, cType, 1, pageNo, pageSize);
            customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }


        #region CustomerSearch
        public ActionResult CustomerInfoList(int[] listBox, string mode, string custType, int pageNo = 1, int pageSize = 5)
        {
            if (mode == "SubscriptionReport")
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(null, null, null, 1, 10, "", 1);
                customerViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriptionViewModel>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView("~/Views/Suscription/_SubscriptionPopup.cshtml", customerViewModel);

            }
            else
            {
                MainViewModel.CustomerViewModel custInfoModel = new MainViewModel.CustomerViewModel();
                var custtomerList = customerService.CustomerInfoList("", "", "", custType, pageNo, pageSize);
                custInfoModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(custtomerList, pageNo, pageSize, (custtomerList.Count == 0) ? 0 : custtomerList.FirstOrDefault().TotalCount);

                return PartialView(custInfoModel);

            }

        }

        public ActionResult _CustomerInfoList(string searchParam, string searchOption, string mode, string custType, int pageNo = 1, int pageSize = 5)
        {
            if (mode == "SubscriptionReport")
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(null, null, null, pageNo, pageSize, "", 1);
                if (searchOption != null && searchOption == "Customer Name")
                {
                    suscriberList = suscriberList.Where(x => x.CustomerName.ToLower().Contains(searchParam.ToLower())).ToList();
                }
                if (searchOption != null && searchOption == "Subscription No")
                {
                    suscriberList = suscriberList.Where(x => x.SubsNo == Convert.ToInt32(searchParam)).ToList();

                }
                customerViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriptionViewModel>(suscriberList, pageNo, pageSize, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}

                return PartialView("~/Views/Suscription/_SubscriptionPopupPartial.cshtml", customerViewModel);
            }
            else
            {
                MainViewModel.CustomerViewModel custInfoModel = new MainViewModel.CustomerViewModel();
                List<MainViewModel.CustomerViewModel> custList = new List<MainViewModel.CustomerViewModel>();
                var custtomerList = customerService.CustomerInfoList(searchParam, searchOption, mode, custType, pageNo, pageSize);
                custInfoModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(custtomerList, pageNo, pageSize, (custtomerList.Count == 0) ? 0 : custtomerList.FirstOrDefault().TotalCount);
                //if (listBox != null)
                //{
                //    foreach (var item in listBox)
                //    {
                //        MainViewModel.CustomerViewModel selectCustInfoModel = new MainViewModel.CustomerViewModel();
                //        selectCustInfoModel.CID = item;
                //        custList.Add(selectCustInfoModel);
                //    }
                //}

                //custInfoModel.SelectedCustInfoList = custList;
                return PartialView("_CustomerInfoList", custInfoModel);
            }

        }

        public ActionResult GetSelectedCustomer(int customerId, int[] listBox, string mode, string custType)
        {

            if (mode == ECustomerSearchType.Suscription.GetDescription()|| mode == ECustomerSearchType.Collection.GetDescription())
            {
                var single = customerService.GetSelectedCustomer(customerId, custType);

                single.Isselect = true;
               return Json(single, JsonRequestBehavior.AllowGet);

            }
            else
            {

                var single = suscription.GetSelectedSubscription(customerId, custType);

                //single.Isselect = true;
                return Json(single, JsonRequestBehavior.AllowGet);

            }
               
                

                

            
        }

        public ActionResult GetMultipleSelectedCustomer(int listBox,string mode)
        {
            if (mode == ECustomerSearchType.Suscription.GetDescription() || mode == ECustomerSearchType.Collection.GetDescription())
            {
                var multipleCustomer = customerService.GetSelectedMultipleCustomer(listBox);
                return Json(multipleCustomer, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var multipleCustomer = suscription.GetSelectedSubscription(listBox,"");
                return Json(multipleCustomer, JsonRequestBehavior.AllowGet);
            }
        }

      public ActionResult GetDetail(int customerId)
        {
            var multipleCustomer = customerService.GetSelectedMultipleCustomer(customerId);
            return PartialView(multipleCustomer);
        }
        #endregion

        public ActionResult Getsuscription(int customerId)
        {
            var multipleCustomer = customerService.GetSelectedMultipleCustomer(customerId);
            return PartialView(multipleCustomer);
        }
        public JsonResult CustomerList(string prefix)
        {
            var CustomerList = customerService.getCustomer(prefix);
            return Json(CustomerList);
        }
        public JsonResult AddressList(string prefix)
        {
            var AddressList = customerService.getAddress(prefix);
            return Json(AddressList);
        }

    }
}