using BuisnessObject.ViewModel;
using BussinessLogic.CustomHelper;
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
    public class ReportController : Controller
    {
        
        private CustomerService customerService = null;
        private SuscriptionService suscription = null;


        public ReportController()
        {
           
            customerService = new CustomerService();
            suscription = new SuscriptionService();

        }
        public ActionResult CustomerReportList()
        {
            try
            {
                MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
                var customerList = customerService.getCustomerList(0, "", "", "", null,1, 1, 10);
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


        public ActionResult _CustomerReportList(int? customerno, string name, string address, string contact, int? cType,int status=1, int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
            var customerList = customerService.getCustomerList(customerno, name, address, contact, cType, status, pageNo, pageSize);
            customerViewModel.customerPagedList = new StaticPagedList<MainViewModel.CustomerViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }



        public ActionResult SuscriptionReportList(int? customerId)
        {
            try
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(customerId,null, null,  1, 10,"",1);
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


        public ActionResult _SuscriptionReportList( int? customerId, int? custtype, DateTime? effectivedate, string Location ="",   int status=1, int pageNo = 1, int pageSize = 10)
        {
            try
            {
                MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
                var suscriberList = suscription.getSuscriberList(customerId, custtype, null,  pageNo, pageSize, Location, status);
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

        [HttpGet]
        public FileContentResult CustomerExportToExcel(int? customerno, string name, string address, string contact, int? cType,string cTypetext="", int status = 1,string statustext="", int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.CustomerViewModel customerViewModel = new MainViewModel.CustomerViewModel();
            var customerList = customerService.getCustomerList(customerno, name, address, contact, cType, status, pageNo, pageSize);
                  var customerExcelList = customerList.Select(x => new ExcelViewModel.CustomerExcelViewModel()
                  {

                      CustNo=x.CustNo,
                      CustomerName=x.CustomerName,
                      Email=x.Email,
                      PhoneNo=x.PhoneNo,
                      MobileNo=x.MobileNo,
                      Customertype=x.Customertype,
                      Address=x.Address
                      

                  }).ToList();
            if (cTypetext == "--Select Event--")
            {
                cTypetext = "";
            }
            string[] columns = { "Customer No.", "Customer Name", "Customer Type", "Phone No.", "Mobile No", "Address", "Email", "Status" };
            string[] parameterSearch = { "Customer No.:" + customerno , "Customer Name:" + name, "Address:" + address , "Mobile No:" + contact , "Customer Type:" + cTypetext , "Status:" + statustext };
            byte[] fileContent = ExcelExportHelper.ExportExcel(customerExcelList, parameterSearch,"Customer Report" ,columns );
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Customer Report.xlsx");
        }



        [HttpGet]
        public FileContentResult SubscriptionExportToExcel(int? customerId, int? custtype, DateTime? effectivedate, string cTypetext = "", string statustext = "", string Location = "", int status = 1, int pageNo = 1, int pageSize = 10)
        {
            MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
            var suscriberList = suscription.getSuscriberList(customerId, custtype, null, pageNo, pageSize, Location, status);
            var subscriptionExcelList = suscriberList.Select(x => new ExcelViewModel.SubscriptionExcelViewModel()
            {

               SubsNo=x.SubsNo,
               
               CustomerName=x.CustomerName,
                LocationName = x.LocationName,
                EffectiveDate =x.EffectiveDate,
               LedgerName=x.LedgerName,
               MonthlyAmount=x.MonthlyAmount,
               Status=x.Status


            }).ToList();
            if (cTypetext == "--Select Event--")
            {
                cTypetext = "";
            }
            string[] columns = { "Subscription No.", "Customer Name", "LocationName", "Effective Date", "Ledger Name", "Monthly Amount", "Status" };
            string[] parameterSearch = { "Location.:" + Location, "Effective Date:" + effectivedate,  "Customer Type:" + cTypetext, "Status:" + statustext };
            byte[] fileContent = ExcelExportHelper.ExportExcel(subscriptionExcelList, parameterSearch, "Subscription Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Subscription Report.xlsx");
        }

    }
}