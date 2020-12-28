using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using DataAccess.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class CollectionService
    {
        ReturnBaseMessageModel returnMessage = null;
        private GenericUnitOfWork uow = null;
       

        public CollectionService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();
            
        }
        public List<MainViewModel.CollectionViewModel> getSuscriberListEntry(int? customerno)
        {
            try
            {
                //string skjns= "select  COUNT(*) OVER () AS TotalCount,Subsno as SubsNo,LocationName,DueBalance as MonthyAmount,Status from  fgetSubscriptionDueReport('"DateTime.Now()"') as ainner join[dbo].[LocationVsCollector] as b on a.LocationId=b.LocationId" + customerno;
                string query = "";
               
                    var suscriptionList=uow.Repository<MainViewModel.CollectionEntry>().SqlQuery("select COUNT(*) OVER () AS TotalCount,subsid,Subsno as SubsNo,CustomerName,LocationName,DueBalance as MonthlyAmount,Debit,Status from fgetSubscriptionDueReport('" + DateTime.Now + "') where custid = " + customerno).ToList();
                var viewModelsuscriptionList= suscriptionList.Select(x => new MainViewModel.CollectionViewModel
                {
                    SubsId=x.subsid,
                    BillNo = x.SubsNo,
                   // CustId = x.CustId,
                    Status=x.Status,
                    LocationName=x.LocationName,
                     MonthlyDueAmt= x.MonthlyAmount,
                     TotalCount=x.TotalCount
                      
                   

                }).ToList();




                return viewModelsuscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ReturnBaseMessageModel CreateCollection(List<MainViewModel.CollectionViewModel> collection)
        {
            try
            {
                
                foreach (var item in collection)
                {
                    if (item.Discount == null)
                    {
                        item.Discount = 0;
                    }
                    uow.ExecWithStoreProcedure("[dbo].[PsetCollectionDue] @SubsCollId,@custId,@Subsid,@CollectorId,@CollectionAmt,@CollDiscount,@CollectionDate,@PostedBy",
                                new SqlParameter("@SubsCollId", item.DueId),
                                new SqlParameter("@custId",item.custid),
                                 new SqlParameter("@Subsid", item.SubsId),
                                new SqlParameter("@CollectorId", item.CollectorId),
                                new SqlParameter("@CollectionAmt", item.ReceivedAmount),
                                new SqlParameter("@CollDiscount", item.Discount),
                              
                                 new SqlParameter("@CollectionDate", item.CollectionDate),
                                new SqlParameter("@PostedBy", Global.UserId)
                                );
                   
                }

                returnMessage.Msg = "Added Sucessfully";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Customer Not Saved";
                returnMessage.Success = false;
                return returnMessage;

                throw ex;
            }
        }
        public ReturnBaseMessageModel EditCollection(MainViewModel.CollectionVerificationEntry collection)
        {
            try
            {
                
                    uow.ExecWithStoreProcedure("[dbo].[PsetCollectionDue] @SubsCollId,@custId,@Subsid,@CollectorId,@CollectionAmt,@CollDiscount,@CollectionDate,@PostedBy",
                                new SqlParameter("@SubsCollId", collection.Subscollid),
                                new SqlParameter("@custId", collection.CustId),
                                 new SqlParameter("@Subsid",collection.subsid),
                                new SqlParameter("@CollectorId", collection.CollectorId),
                                new SqlParameter("@CollectionAmt", collection.CollectionAmt),
                                new SqlParameter("@CollDiscount", collection.DiscountAmt),

                                 new SqlParameter("@CollectionDate", collection.CollectionDate),
                                new SqlParameter("@PostedBy", Global.UserId)
                                );

              

                returnMessage.Msg = "Edited Sucessfully";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = " Not Saved";
                returnMessage.Success = false;
                return returnMessage;

                throw ex;
            }
        }

        public List<MainViewModel.CollectorDetail> getCollector(string prefix)
        {
            var collectionList = uow.Repository<MainViewModel.CollectorDetail>().SqlQuery("select * from FgetCollectorInfo()").Where(x=>x.CollectorName.ToLower().StartsWith(prefix.ToLower())).ToList();
            var collections = (from c in collectionList
                               select new
                               {
                                   CollectorName = c.CollectorName,
                                   CollectorID=c.CollectorID
                               }).ToList();
            return collectionList;
        }

        public ReturnBaseMessageModel VerifyCollection(List<MainViewModel.CollectionVerificationEntry> collectonS, decimal? amounttota)
        {
            try
            {
                SubscriptionCollection verifyCollection = new SubscriptionCollection();
                foreach (var item in collectonS)
                {
                    verifyCollection = uow.Repository<SubscriptionCollection>().FindBy(x => x.SubsCollId == item.Subscollid).SingleOrDefault();

                    verifyCollection.VerifiedBy = Global.UserId;
                    verifyCollection.VerifiedOn = DateTime.Now;


                    uow.Repository<SubscriptionCollection>().Edit(verifyCollection);

                }


                CollectionVerifyLog colverify = new CollectionVerifyLog();
                colverify.Tdate = DateTime.Now;
                colverify.VerifiedAMount = amounttota;
                colverify.VerifiedBy = Global.UserId;
                uow.Repository<CollectionVerifyLog>().Add(colverify);
                uow.Commit();
                returnMessage.Msg = "Verified Successfuly";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public MainViewModel.CollectionVerificationEntry GetSingleCollection(int? subscollid)
        {
            string query = "";
            query = "select COUNT(*) OVER () AS TotalCount,SubsId,CustId,Subscollid,CollectorId,Collectorname,subsno as SubsNo,CustomerName,LocationName,CollectionDate,CollectionAmt ,DiscountAmt  from fgetCollectionlist()  where verifiedby is null  and subscollid=" + subscollid;

            
            
            var collectionList = uow.Repository<MainViewModel.CollectionVerificationEntry>().SqlQuery(query).SingleOrDefault();

            return collectionList;
        }

        public List<MainViewModel.CollectionVerificationEntry> getCollectionList(string CollectorName="",string LocationName="", string EntryTypeList="", int pageNo=1, int pageSize=10)
        {
            try
            {

                string query = "";
                query = "select COUNT(*) OVER () AS TotalCount,SubsId,CollectorId,CustomerNo,LocationID,Collectorname,CustId,Subscollid,subsno,LocationID as SubsNo,CustomerName,LocationName,CollectionDate,CollectionAmt ,DiscountAmt,CollectionType,PostedBy   from fgetCollectionlist()  where verifiedby is null";

                if (CollectorName != "" )
                {
                    query += "  and Collectorname like'%" + CollectorName.Trim() + "%'";
                }
                if (LocationName != "")
                {
                    query += "  and LocationName like'%" + LocationName.Trim() + "%'";
                }
                if (EntryTypeList != "")
                {
                    query += "  and CollectionType like'%" + EntryTypeList.Trim() + "%'";
                }
                query += @" ORDER BY  Subscollid
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var collectionList = uow.Repository<MainViewModel.CollectionVerificationEntry>().SqlQuery(query).ToList();

                return collectionList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
