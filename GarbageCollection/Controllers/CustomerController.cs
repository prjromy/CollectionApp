using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using System.Data.Entity.Validation;
using PagedList;

namespace GarbageCollection.Controllers
{
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
                var customerList= customerService.getCustomerList(0,"", "", "", null, 1, 10);
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
            var customerList = customerService.getCustomerList(customerno,name, address, contact, cType, pageNo, pageSize);
            customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }
    }
}