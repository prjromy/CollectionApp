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
        


        public CustomerController()
        {
            returnMessage = new ReturnBaseMessageModel();
           
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
                var customerList= customerService.getCustomerList(0,"", "", "", null,1, 1, 10);
                customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}

                return PartialView( customerViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        }


        public ActionResult _List(int? customerno,string name, string address, string contact, int? cType, int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
            var customerList = customerService.getCustomerList(customerno,name, address, contact, cType,1, pageNo, pageSize);
            customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }


        #region CustomerSearch
        public ActionResult CustomerInfoList(int[] listBox, string mode, string custType, int pageNo = 1, int pageSize = 5)
        {
            MainViewModel.CustomerViewModel custInfoModel = new MainViewModel.CustomerViewModel();
            var custtomerList = customerService.CustomerInfoList("", "", "", custType, pageNo, pageSize);
            custInfoModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(custtomerList, pageNo, pageSize, (custtomerList.Count == 0) ? 0 : custtomerList.FirstOrDefault().TotalCount);
            List<MainViewModel.CustomerViewModel> selectMultipleList = new List<MainViewModel.CustomerViewModel>();
            //if (mode != ECustomerSearchType.SingleType.GetDescription())
            //{
            //    selectMultipleList = customerService.GetSelectedMultipleCustomer(listBox);
            //    custInfoModel.CIDs = listBox;
            //}
            //custInfoModel.Mode = mode;
            //custInfoModel.cus = custType;
            //custInfoModel.SelectedCustInfoList = selectMultipleList;
            return PartialView( custInfoModel);
        }

        public ActionResult _CustomerInfoList(string searchParam, string searchOption,  string mode, string custType, int pageNo = 1, int pageSize = 5)
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

        public ActionResult GetSelectedCustomer(int customerId, int[] listBox, string mode, string custType)
        {
            var singleCustomer = customerService.GetSelectedCustomer(customerId, custType);

            if (mode == ECustomerSearchType.Suscription.GetDescription())
            {

                //    if (singleCustomer.isind == 0 && listBox.Count() > 0)
                //    {
                //        if (listBox.Where(x => x == 0).Count() == 1)
                //            singleCustomer.Isselect = true;
                //        else
                //            singleCustomer.Isselect = false;
                //    }
                //    else
                //    {
                //        var isCheck = customerService.GetSelectedCustomer(listBox[0], custType);
                //        if (isCheck != null)
                //        {
                //            if (isCheck.isind == 0)
                //                singleCustomer.Isselect = false;
                //            else
                //                singleCustomer.Isselect = true;
                //        }
                //        else
                //        {
                //            singleCustomer.Isselect = true;
                //        }
                //    }
                //}
                //else
                //{
                singleCustomer.Isselect = true;
                //}
            }
            return Json(singleCustomer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMultipleSelectedCustomer(int listBox)
        {
            var multipleCustomer = customerService.GetSelectedMultipleCustomer(listBox);
            return Json(multipleCustomer, JsonRequestBehavior.AllowGet);
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


    }
}