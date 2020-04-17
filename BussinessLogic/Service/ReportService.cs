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
              
                    query = "select COUNT(*) OVER () AS TotalCount,Subsno,CustomerName,CustomerType,LocationName,LedgerName,Debit,Credit,DueBalance,Status from [dbo].[fgetSubscriptionDueReport]('" + tillDate + "')";

                 if(Location!=null && Location != "")
                    {
                    query += "where LocationName like'%" + Location.Trim() + "%'";
                }
             
                query += @" ORDER BY  Subsno
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var suscriptionList = uow.Repository<MainViewModel.SubscriberDueViewModel>().SqlQuery(query).ToList();

                return suscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<MainViewModel.CollectionReport> getCollectorReportList(DateTime? collectionDate, int pageNo, int pageSize, string customerName = "",string collector="",string Location="",int verified=1)
        {
            try
            {

                string query = "";
                if (verified == 2)
                {
                    query = "select COUNT(*) OVER () AS TotalCount,Subsno,CustomerName,CustomerType,LocationName,CollectorName,CollectionDate,CollectionAmt,DiscountAmt from [dbo].[fgetCollectionlist]() where verifiedBy is  null";

                }
                if(verified == 1)
                {
                    query = "select COUNT(*) OVER () AS TotalCount,Subsno,CustomerName,CustomerType,LocationName,CollectorName,CollectionDate,CollectionAmt,DiscountAmt from [dbo].[fgetCollectionlist]() where verifiedBy is not null";

                }

                if (Location != null && Location != "")
                {
                    query += "  and LocationName like'%" + Location.Trim() + "%'";
                }
                if (customerName != null && customerName != "")
                {
                    query += "  and CustomerName like'%" + customerName.Trim() + "%'";
                }
      
                if (collector != null && collector != "")
                {
                    query += "  and collector like'%" + collector.Trim() + "%'";
                }
      
                if (collectionDate != null)
                {
                    query += "  and CollectionDate = '"+ collectionDate+"'";
                }
                query += @" ORDER BY  Subsno
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
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
               
                    query = "select COUNT(*) OVER () AS TotalCount, SubsNo,Custname,PostedOnAd,PostedOnBs,Debit,Credit,Balance,Sources from [dbo].[fgetSubscriberStmnt] (" + subsid+",'"+FromDate+"','"+ToDate+"')";

              

                query += @" ORDER BY  PostedOnAd asc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var subscriptionreportList = uow.Repository<MainViewModel.SubscriptionReport>().SqlQuery(query).ToList();

                return subscriptionreportList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
