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
                var suscriberList = suscription.getSuscriberList(customerId, custtype, effectivedate,  pageNo, pageSize, Location, status);
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
        public FileContentResult CustomerExportToExcel(int? customerno, string name, string address, string contact, int? cType,string cTypetext="", int status = 1,string statustext="", int pageNo = 1, int pageSize = 0)
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
            byte[] fileContent = ExcelExportHelper.ExportExcel(customerExcelList, parameterSearch,null,"Customer Report" ,columns );
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Customer Report.xlsx");
        }



        [HttpGet]
        public FileContentResult SubscriptionExportToExcel(int? customerId, int? custtype, DateTime? effectivedate, string cTypetext = "", string statustext = "", string Location = "", int status = 1, int pageNo = 1, int pageSize = 0)
        {
            MainViewModel.SubscriptionViewModel customerViewModel = new MainViewModel.SubscriptionViewModel();
            var suscriberList = suscription.getSuscriberList(customerId, custtype, effectivedate, pageNo, pageSize, Location, status);
            var subscriptionExcelList = suscriberList.Select(x => new ExcelViewModel.SubscriptionExcelViewModel()
            {

              custno=x.CustNo,
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
            string[] columns = { "Customer No", "Subscription No.","Customer Name", "LocationName", "Effective Date", "Ledger Name", "Monthly Amount", "Status" };
            string[] parameterSearch = { "Location.:" + Location, "Effective Date:" + effectivedate,  "Customer Type:" + cTypetext, "Status:" + statustext };
            byte[] fileContent = ExcelExportHelper.ExportExcel(subscriptionExcelList, parameterSearch,null, "Subscription Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Subscription Report.xlsx");
        }


        public ActionResult SubscriptionDueReportList()
        {
            try
            {
                MainViewModel.SubscriberDueViewModel subscriptionViewModel = new MainViewModel.SubscriberDueViewModel();
                var suscriberList = reportService.getSuscriberList(DateTime.Now, 1, 10, "");
                var totalsubscriberList = reportService.getSingleTotalSuscriberList(DateTime.Now, "");
                ViewBag.DebitSum = totalsubscriberList.SumDebit;
                ViewBag.CreditSum = totalsubscriberList.SumCredit;
                ViewBag.AdvanceSum = totalsubscriberList.SumAdvance;
                ViewBag.DueBalance = totalsubscriberList.SumDueBalance;
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
                var totalsubscriberList = reportService.getSingleTotalSuscriberList(tillDate, Location);
                ViewBag.DebitSum = totalsubscriberList.SumDebit;
                ViewBag.CreditSum = totalsubscriberList.SumCredit;
                ViewBag.AdvanceSum = totalsubscriberList.SumAdvance;
                ViewBag.DueBalance = totalsubscriberList.SumDueBalance;

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
        public FileContentResult SubscriptionDueExportToExcel(DateTime? tillDate, int pageNo = 1, int pageSize = 0, string Location = "")
        {
            MainViewModel.SubscriberDueViewModel subscriptionViewModel = new MainViewModel.SubscriberDueViewModel();
            var suscriberList = reportService.getSuscriberList(tillDate, pageNo, pageSize, Location);
            var totalsubscriberList = reportService.getSingleTotalSuscriberList(tillDate, Location);
            var subscriptionDueExcelList = suscriberList.Select(x => new ExcelViewModel.SubscriberDueExcelViewModel()
            {
                custNo=x.CustNo,
               Subsno=x.Subsno,
               CustomerName=x.CustomerName,
               CustomerType=x.CustomerType,
               LocationName=x.LocationName,
               LedgerName=x.LedgerName,
               Debit=x.Debit,
               Credit=x.Credit,
               Advance=x.Advance,
               DueBalance=x.DueBalance,
               
               Status=x.Status

            }).ToList();

            //var totalsubscriptionDueExcelList = totalsubscriberList.Select(x => new ExcelViewModel.SubscriberDueExcelViewModel()
            //{

            //    SumCredit=x.SumCredit,
            //    SumDebit=x.SumDebit,
            //    SumAdvance=x.SumAdvance,
            //    SumDueBalance=x.SumDueBalance

            //}).();
           //  var DebitSum = totalsubscriberList.SumDebit;
           // var CreditSum = totalsubscriberList.SumCredit;
           //var AdvanceSum = totalsubscriberList.SumAdvance;
           // var DueBalance = totalsubscriberList.SumDueBalance;
            decimal[] totals = { totalsubscriberList.SumDebit, totalsubscriberList.SumCredit, totalsubscriberList.SumAdvance, totalsubscriberList.SumDueBalance };

            string[] columns = { "Customer No.","Subscription No.", "Customer Name", "Customer Type","Location Name","Ledger Name", "Debit", "Credit","Advance", "DueBalance", "Status" };
            string[] parameterSearch = { "Location.:  " + Location, "Effective Date:  " + tillDate };
            byte[] fileContent = ExcelExportHelper.ExportExcel(subscriptionDueExcelList, parameterSearch, totals, "Subscription Due Report", columns);
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


        public ActionResult _collectionReportList(DateTime? collectionDate, int pageNo=1, int pageSize=10, string customerName = "", string collector = "", string Location = "", int verified = 1, string EntryTypeList="")
        {
            try
            {
                MainViewModel.CollectionReport collectorViewModel = new MainViewModel.CollectionReport();
                var collectorList = reportService.getCollectorReportList(collectionDate, pageNo, pageSize, customerName,collector, Location, verified, EntryTypeList);
                collectorViewModel.collectorPagedList = new StaticPagedList<MainViewModel.CollectionReport>(collectorList, pageNo, pageSize, (collectorList.Count == 0) ? 0 : collectorList.FirstOrDefault().TotalCount);
                return PartialView(collectorViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public FileContentResult CollectionExportToExcel(DateTime? collectionDate, int pageNo = 1, int pageSize = 0, string customerName = "", string collector = "", string Location = "", int verified = 1 ,string statustext="", string EntryTypeList = "")
        {
            MainViewModel.CollectionReport collectorViewModel = new MainViewModel.CollectionReport();
            var collectorList = reportService.getCollectorReportList(collectionDate, pageNo, pageSize, customerName, collector, Location, verified,  EntryTypeList );
            var collectorExcelList = collectorList.Select(x => new ExcelViewModel.CollectorExcelViewModel()
            {
                CustomerNo=x.CustomerNo,
                Subsno = x.Subsno,
                CustomerName = x.CustomerName,
                CustomerType = x.CustomerType,
                LocationName=x.LocationName,
                CollectionAmt=x.CollectionAmt,
                CollectionDate=x.CollectionDate,
                CollectorName=x.CollectorName,
                DiscountAmt=x.DiscountAmt,
                CollectionTYpe=x.CollectionType
                
            }).ToList();

            string[] columns = { "Customer No","Subscription No.", "Customer Name", "Customer Type", "Location Name", "Collector Name", "Collection Date", "Collection Amount", "Discount Amount",  "CollectionType" };
            string[] parameterSearch = { "Customer Name.:  " + customerName, "Collector.:  " + collector, "Collection Date:  " + collectionDate, "Location:  " + Location, "Status:  " + statustext,"CollectionType :"+ EntryTypeList };
            byte[] fileContent = ExcelExportHelper.ExportExcel(collectorExcelList, parameterSearch,null, "Collector Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Collector  Report.xlsx");
        }

        public ActionResult SubscriberStatement()
        {
            try
            {
                MainViewModel.SubscriptionViewModel subscriptionViewModel = new MainViewModel.SubscriptionViewModel();
                //var suscriberList = reportService.getSubscriberStatementList(subsid, FromDate, ToDate, 1, 10);
                //subscriptionViewModel.subscriptionPagedList = new StaticPagedList<MainViewModel.SubscriptionReport>(suscriberList, 1, 10, (suscriberList.Count == 0) ? 0 : suscriberList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}
                
                subscriptionViewModel.ModelFrom = "SubscriptionReport";
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
                var suscriberList = reportService.getSubscriberStatementList(subsid, FromDate, ToDate, pageNo, pageSize);
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

        [HttpGet]
        public FileContentResult SubscriptionStatementExportToExcel( DateTime? FromDate, DateTime? ToDate, int? subsid,string substext="")
        {
            int pageNo = 1;
            int pageSize =0;
            MainViewModel.SubscriptionReport subscriptionViewModel = new MainViewModel.SubscriptionReport();
            var suscriberList = reportService.getSubscriberStatementList(subsid, FromDate, ToDate, pageNo, pageSize);
            var subscriptionExcelList = suscriberList.Select(x => new ExcelViewModel.SubscriptionReportExcel()
            {
               custNo=x.CustNo,
               SubsNo=x.SubsNo,
               Custname=x.Custname,
               Debit=x.Debit,
               Credit=x.Credit,
               Balance=x.Balance,
               PostedOnAd=x.PostedOnAd,
               PostedOnBs=x.PostedOnBs,
                Sources=x.Sources
               

            }).ToList();

            string[] columns = { "Customer No","Subscription No.", "Customer Name", "Debit", "Credit", "Balance", "Posted On Ad", "Posted On Bst", "Sources" };
            string[] parameterSearch = { "From Date.:  " + FromDate, "To Date.:  " + ToDate, "Customer Name:  " + substext };
            byte[] fileContent = ExcelExportHelper.ExportExcel(subscriptionExcelList, parameterSearch,null, "Subscription Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Subscription  Report.xlsx");
        }

        public ActionResult MonthlyDueReport()
        {
            try
            {
                MainViewModel.MonthlyDueViewModel subscriptionViewModel = new MainViewModel.MonthlyDueViewModel();
                 ViewBag.Year = reportService.YearList();
                return PartialView(subscriptionViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult _MonthlyDueReport(int? Month, int? Year, string Location, int pageNo=1,int pageSize=10)
        {
            try
            {
                MainViewModel.MonthlyDueViewModel MonthlyModel = new MainViewModel.MonthlyDueViewModel();
                var monthlyList = reportService.getMonthlyDueList(Month, Year, Location, pageNo, pageSize);
                MonthlyModel.monthlyDuePagedList = new StaticPagedList<MainViewModel.MonthlyDueViewModel>(monthlyList, pageNo, pageSize, (monthlyList.Count == 0) ? 0 : monthlyList.FirstOrDefault().TotalCount);

            


                return PartialView(MonthlyModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public FileContentResult MonthlyDueReportExportToExcel(int? Month, int? Year, string Location, int pageNo = 1, int pageSize = 0,string Monthtext = "")
        {
            MainViewModel.MonthlyDueViewModel subscriptionViewModel = new MainViewModel.MonthlyDueViewModel();
            var monthlyList = reportService.getMonthlyDueList(Month, Year, Location, pageNo, pageSize);
            var monthlyExcelList = monthlyList.Select(x => new ExcelViewModel.MonthlyDueExcelViewModel()
            {
              custNo=x.Custno,
              Subsno=x.Subsno,
              CustomerName=x.CustomerName,
              CustomerType=x.CustomerType,
              LocationName=x.LocationName,
              MonthlyDue=x.MonthlyDue,
              PostedOn=x.PostedOn

            }).ToList();

            string[] columns = { "Customer No","Subscription No.", "Customer Name", "Customer Type", "Location Name", "Monthly Due", "PostedOn"};
            string[] parameterSearch = { "Year.:  " + Year, "MonthText.:  " + Monthtext,"Location.:  "+ Location };
            byte[] fileContent = ExcelExportHelper.ExportExcel(monthlyExcelList, parameterSearch,null, "Monthly Due Report", columns);
            return File(fileContent, ExcelExportHelper.ExcelContentType, "Monthly Due  Report.xlsx");
        }
    }
}