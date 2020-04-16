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
        private ReportService reportService = null;

        public ReportController()
        {
           
            customerService = new CustomerService();
            suscription = new SuscriptionService();
            reportService = new ReportService();

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


        public ActionResult SubscriptionDueReportList()
        {
            try
            {
                MainViewModel.SubscriberDueViewModel subscriptionViewModel = new MainViewModel.SubscriberDueViewModel();
                var suscriberList = reportService.getSuscriberList(DateTime.Now, 1, 10, "");
                subscriptionViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriberDueViewModel>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(subscriptionViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _SubscriptionDueReportList(DateTime? tillDate, int pageNo=1, int pageSize=10, string Location = "")
        {
            try
            {
                MainViewModel.SubscriberDueViewModel subscriptionViewModel = new MainViewModel.SubscriberDueViewModel();
                var suscriberList = reportService.getSuscriberList(tillDate, pageNo, pageSize, Location);
                subscriptionViewModel.suscriberPagedList = new StaticPagedList<MainViewModel.SubscriberDueViewModel>(suscriberList, pageNo, pageSize, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(subscriptionViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public FileContentResult SubscriptionDueExportToExcel(DateTime? tillDate, int pageNo = 1, int pageSize = 10, string Location = "")
        {
            MainViewModel.SubscriberDueViewModel subscriptionViewModel = new MainViewModel.SubscriberDueViewModel();
            var suscriberList = reportService.getSuscriberList(tillDate, pageNo, pageSize, Location);
            var subscriptionDueExcelList = suscriberList.Select(x => new ExcelViewModel.SubscriberDueExcelViewModel()
            {

               Subsno=x.Subsno,
               CustomerName=x.CustomerName,
               CustomerType=x.CustomerType,
               LocationName=x.LocationName,
               LedgerName=x.LedgerName,
               Debit=x.Debit,
               Credit=x.Credit,
               DueBalance=x.DueBalance,
               
               Status=x.Status

            }).ToList();
          
            string[] columns = { "Subscription No.", "Customer Name", "Customer Type","Location Name","Ledger Name", "Debit", "Credit", "DueBalance", "Status" };
            string[] parameterSearch = { "Location.:  " + Location, "Effective Date:  " + tillDate };
            byte[] fileContent = ExcelExportHelper.ExportExcel(subscriptionDueExcelList, parameterSearch, "Subscription Due Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Subscription Due  Report.xlsx");
        }

        public ActionResult collectionReportList()
        {
            try
            {
                MainViewModel.CollectionReport collectorViewModel = new MainViewModel.CollectionReport();
                var suscriberList = reportService.getCollectorReportList(null,1,10,"","","",1);
                collectorViewModel.collectorPagedList = new StaticPagedList<MainViewModel.CollectionReport>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(collectorViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _collectionReportList(DateTime? collectionDate, int pageNo=1, int pageSize=10, string customerName = "", string collector = "", string Location = "", int verified = 1)
        {
            try
            {
                MainViewModel.CollectionReport collectorViewModel = new MainViewModel.CollectionReport();
                var collectorList = reportService.getCollectorReportList(null, pageNo, pageSize, customerName,collector, Location, verified);
                collectorViewModel.collectorPagedList = new StaticPagedList<MainViewModel.CollectionReport>(collectorList, pageNo, pageSize, (collectorList.Count == 0) ? 0 : collectorList.FirstOrDefault().TotalCount);
                return PartialView(collectorViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public FileContentResult CollectionExportToExcel(DateTime? collectionDate, int pageNo = 1, int pageSize = 10, string customerName = "", string collector = "", string Location = "", int verified = 1 ,string statustext="")
        {
            MainViewModel.CollectionReport collectorViewModel = new MainViewModel.CollectionReport();
            var collectorList = reportService.getCollectorReportList(null, pageNo, pageSize, customerName, collector, Location, verified);
            var collectorExcelList = collectorList.Select(x => new ExcelViewModel.CollectorExcelViewModel()
            {

                Subsno = x.Subsno,
                CustomerName = x.CustomerName,
                CustomerType = x.CustomerType,
                LocationName=x.LocationName,
                CollectionAmt=x.CollectionAmt,
                CollectionDate=x.CollectionDate,
                CollectorName=x.CollectorName,
                DiscountAmt=x.DiscountAmt

            }).ToList();

            string[] columns = { "Subscription No.", "Customer Name", "Customer Type", "Location Name", "Collector Name", "Collection Date", "Collection Amount", "Discount Amount" };
            string[] parameterSearch = { "Customer Name.:  " + customerName, "Collector.:  " + collector, "Collection Date:  " + collectionDate, "Location:  " + Location, "Status:  " + statustext };
            byte[] fileContent = ExcelExportHelper.ExportExcel(collectorExcelList, parameterSearch, "Collector Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Collector  Report.xlsx");
        }

        public ActionResult SubscriberStatement(int? subsid, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                MainViewModel.SubscriptionReport subscriptionViewModel = new MainViewModel.SubscriptionReport();
                var suscriberList = reportService.getSubscriberStatementList(subsid, FromDate, ToDate, 1, 10);
                subscriptionViewModel.subscriptionPagedList = new StaticPagedList<MainViewModel.SubscriptionReport>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(subscriptionViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _SubscriberStatement(int? subsid, DateTime? FromDate, DateTime? ToDate, int pageNo=1, int pageSize=10)
        {
            try
            {
                MainViewModel.SubscriptionReport subscriptionViewModel = new MainViewModel.SubscriptionReport();
                var suscriberList = reportService.getSubscriberStatementList(null, null, null, pageNo, pageSize);
                subscriptionViewModel.subscriptionPagedList = new StaticPagedList<MainViewModel.SubscriptionReport>(suscriberList, pageNo, pageSize, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}


                return PartialView(subscriptionViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}