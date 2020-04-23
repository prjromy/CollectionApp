using BuisnessObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinessLogic.Service;
using System.Data.Entity.Validation;
using PagedList;
using Loader;

namespace GarbageCollection.Controllers
{
    [MyAuthorize]
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
            suscriptionModel.ModelFrom= "Suscription";
            return PartialView(suscriptionModel);
        }
        public ActionResult Create(int? sNo,int? customerid,string custname="")
        {
            MainViewModel.SubscriptionViewModel suscriptionModel = new MainViewModel.SubscriptionViewModel();
            if (sNo == null)
            {
               
                suscriptionModel.CustId = customerid;
                suscriptionModel.CustomerName = custname;
               
            }
            else
            {
                suscriptionModel = suscription.getSingleSuscriptonList(sNo);
                ViewBag.EffectiveDate = suscriptionModel.EffectiveDate.Value.Month.ToString() + "-" + suscriptionModel.EffectiveDate.Value.Day.ToString() + "-" + suscriptionModel.EffectiveDate.Value.Year.ToString();

                var customerName = customerService.GetSelectedMultipleCustomer(Convert.ToInt32(suscriptionModel.CustId));
                suscriptionModel.CustomerName = customerName.CustomerName;
            }
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

        public ActionResult List(int? customerId)
        {
            try
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(customerId, null, null, 1, 10, "", 1);
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


        public ActionResult _List(int? customerId, int pageNo = 1, int pageSize = 10)
        {
            try
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(customerId, null, null, 1, 10, "", 1);
                customerViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriptionViewModel>(suscriberList, pageNo, pageSize, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

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


        ////for subscription popup in report
        //public ActionResult _SubscriptionPopupPartial(string customername,int?subsno, int pageNo = 1, int pageSize = 10)
        //{
        //    try
        //    {
                


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public ActionResult UpdateStatus(int? sId)
        {


            try
            {


                returnMessage = suscription.changeStatus(sId);

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
        public  JsonResult LocationList(string prefix)
        {
            var LocationList = suscription.getLocation(prefix);
            return Json(LocationList);
        }
    }
}