using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class ReportService
    {
        public GenericUnitOfWork uow = null;
        public ReportService()
        {
            uow = new GenericUnitOfWork();
        }
        public List<MainViewModel.SubscriberDueViewModel> getSuscriberList(DateTime? tillDate, int pageNo, int pageSize, string Location = "")
        {
            try
            {

                string query = "";
              
                    query = "select COUNT(*) OVER () AS TotalCount,CustNo,Subsno,CustomerName,CustomerType,LocationName,LedgerName,Debit,Credit,Advance,DueBalance,Status from [dbo].[fgetSubscriptionDueReport]('" + tillDate + "')";

              
                 if(Location!=null && Location != "")
                    {
                 
                    query += "where LocationName ='" + Location.Trim()+"'";
                }

                if (pageSize == 0)
                {
                    query += @" ORDER BY  Subsno";
                 
                }
                else
                {
                    query += @" ORDER BY  Subsno
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                }

               
                var suscriptionList = uow.Repository<MainViewModel.SubscriberDueViewModel>().SqlQuery(query).ToList();

                return suscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<MainViewModel.SubscriberDueViewModel> getTotalSuscriberList(DateTime? tillDate,  string Location = "")
        {
            try
            {

                string query = "";

                query = "select SUM(debit) as SumDebit,SUM(credit) as SumCredit,SUM(Advance) as SumAdvance,SUM(DueBalance)as SumDueBalance from [dbo].[fgetSubscriptionDueReport]('" + tillDate + "')";


                if (Location != null && Location != "")
                {
                    query += "where LocationName ='" + Location.Trim() + "'";
                }

               
                 

                
              


                var suscriptionList = uow.Repository<MainViewModel.SubscriberDueViewModel>().SqlQuery(query).ToList();

                return suscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public MainViewModel.SubscriberDueViewModel getSingleTotalSuscriberList(DateTime? tillDate, string Location = "")
        {
            try
            {

                string query = "";

                query = "select SUM(debit) as SumDebit,SUM(credit) as SumCredit,SUM(Advance) as SumAdvance,SUM(DueBalance)as SumDueBalance from [dbo].[fgetSubscriptionDueReport]('" + tillDate + "')";


                if (Location != null && Location != "")
                {
                    query += "where LocationName like'%" + Location.Trim() + "%'";
                }


                var suscriptionList = uow.Repository<MainViewModel.SubscriberDueViewModel>().SqlQuery(query).SingleOrDefault();

                return suscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<MainViewModel.CollectionReport> getCollectorReportList(DateTime? collectionDate, int pageNo, int pageSize, string customerName = "",string collector="",string Location="",int verified=1, string EntryTypeList = "")
        {
            try
            {

                string query = "";
                if (verified == 2)
                {
                    query = "select COUNT(*) OVER () AS TotalCount,Subsno,CustomerName,CollectionType,CustomerNo,CustomerType,LocationName,CollectorName,CollectionDate,CollectionAmt,DiscountAmt from [dbo].[fgetCollectionlist]() where verifiedBy is  null";

                }
                if(verified == 1)
                {
                    query = "select COUNT(*) OVER () AS TotalCount,Subsno,CustomerName,CollectionType,CustomerNo,CustomerType,LocationName,CollectorName,CollectionDate,CollectionAmt,DiscountAmt from [dbo].[fgetCollectionlist]() where verifiedBy is not null";

                }

                if (Location != null && Location != "")
                {
                    query += "  and LocationName like'%" + Location.Trim() + "%'";
                }
                if (customerName != null && customerName != "")
                {
                    query += "  and CustomerName like'%" + customerName.Trim() + "%'";
                }
      
                if (EntryTypeList != null && EntryTypeList != "")
                {
                    query += "  and CollectionType like'%" + EntryTypeList.Trim() + "%'";
                }
                if (collector != null && collector != "")
                {
                    query += "  and collector like'%" + collector.Trim() + "%'";
                }
                if (collectionDate != null)
                {
                    query += "  and CollectionDate <= '"+ collectionDate+"'";
                }
                if (pageSize == 0)// For Excel 
                {
                    query += @" ORDER BY  Subsno";
                      
                }
                else
                {
                    query += @" ORDER BY  Subsno
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                }
          
                var collectorList = uow.Repository<MainViewModel.CollectionReport>().SqlQuery(query).ToList();

                return collectorList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<MainViewModel.SubscriptionReport> getSubscriberStatementList(int? subsid,DateTime? FromDate, DateTime? ToDate ,int pageNo, int pageSize)
        {
            try
            {

                string query = "";
               
                    query = "select COUNT(*) OVER () AS TotalCount,CustNo, SubsNo,Custname,PostedOnAd,PostedOnBs,Debit,Credit,Balance,Sources from [dbo].[fgetSubscriberStmnt] (" + subsid+",'"+FromDate+"','"+ToDate+"')";


                if (pageSize == 0)
                {
                    query += @" ORDER BY  PostedOnAd asc";
                    
                }
                else
                {
                    query += @" ORDER BY  PostedOnAd asc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                }
              
                var subscriptionreportList = uow.Repository<MainViewModel.SubscriptionReport>().SqlQuery(query).ToList();

                return subscriptionreportList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<MainViewModel.MonthlyDueViewModel> getMonthlyDueList(int? Month, int? Year,string Location, int pageNo, int pageSize)
        {
            try
            {

                string query = "";

                query = " select COUNT(*) OVER () AS TotalCount,Subsno,Custno,CustomerName,CustomerType,LocationName,MonthlyDue,PostedOn from FgetMonthlyDuePosted(" + Year + ","+ Month + ")";

                if(Location != null && Location != "")
                {
                    query += "  where LocationName ='" + Location.Trim() + "'";
                }
                if (pageSize == 0)
                {
                    query += @" ORDER BY  Subsno asc";
                      
                }
                else
                {
                    query += @" ORDER BY  Subsno asc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                }
              
                var monthlyreportList = uow.Repository<MainViewModel.MonthlyDueViewModel>().SqlQuery(query).ToList();

                return monthlyreportList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<int> YearList()
        {
            var Year = uow.Repository<string>().SqlQuery("select distinct(CONVERT(varchar(50), year))from [dbo].[SubscriptionDue]").ToList();
            List<int> intYear = new List<int>();
            foreach (var item in Year)
            {
              
                intYear.Add(Convert.ToInt32(item));
            }
             
            return intYear.ToList();
        }
    }
}
