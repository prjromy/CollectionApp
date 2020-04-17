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
    public class SuscriptionService

    {
        ReturnBaseMessageModel returnMessage = null;

        private GenericUnitOfWork uow = null;
        private CustomerService customer = null;
     
        public SuscriptionService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();
            customer = new CustomerService();
        }
        public ReturnBaseMessageModel Save(MainViewModel.SubscriptionViewModel suscription)
        {
            try
            {
                var subsid = suscription.Subsid.GetType();
                var CustId = suscription.CustId.GetType();
                uow.ExecWithStoreProcedure("[dbo].[PcreateSubscription] @Subsid,@CustId,@EffectiveDate,@LedgerId,@MonthlyAmount,@LocationId,@Remarks,@PostedBy",
                                                new SqlParameter("@Subsid", suscription.Subsid),
                                                new SqlParameter("@CustId", suscription.CustId),
                                                 new SqlParameter("@EffectiveDate", suscription.EffectiveDate),
                                                new SqlParameter("@LedgerId", suscription.LedgerId),
                                                new SqlParameter("@MonthlyAmount", suscription.MonthlyAmount),
                                                new SqlParameter("@LocationId", suscription.LocationID),
                                                new SqlParameter("@Remarks",suscription.Remarks),
                                                 new SqlParameter("@PostedBy", Global.UserId)
                                                
                                                );
                if (suscription.Subsid == 0)
                {


                    returnMessage.Msg = "Suscription Added Sucessfully";

                }
                else
                {
                    returnMessage.Msg = "Suscription Edited Sucessfully";
                }
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Suscription Not Saved";
                returnMessage.Success = false;
                return returnMessage;

                throw ex;
            }
        }

        public MainViewModel.SubscriptionViewModel getSingleSuscriptonList(int? sno)
        {
        
            var subsList = uow.Repository<Subscription>().FindBy(x => x.Subsid == sno);

            MainViewModel.SubscriptionViewModel subsSingleList = subsList.Select(x => new MainViewModel.SubscriptionViewModel
            {
                Subsid=x.Subsid,
               CustId=x.CustId,
               LedgerId=x.LedgerId,
               MonthlyAmount=x.MonthlyAmount,
               LocationID=x.LocationID,
               
               EffectiveDate=x.EffectiveDate,
               Remarks=x.Remarks
               
            }).SingleOrDefault();
          
            return subsSingleList;
        }


        public List<MainViewModel.SubscriptionViewModel> getSuscriberList(int? customerId, int? custtype, DateTime? effectivedate, int pageNo, int pageSize, string Location = "",int status = 1)
        {
            try
            {

                string query = "";
                if (customerId == null && status == 0)
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where status=0";

                }
               if (customerId != null && status == 1)
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where CustId=  " + customerId + " and status=1";
                }

                if (customerId == null && status == 1)
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where status=1";

                }
                //where  CustomerName like'%" + Name.Trim() + "%'";

                if (custtype !=0 && custtype!=null)
                {
                    query += " and custtypeId =" + custtype;
                }
                if (effectivedate != null )
                {
                    query += " and EffectiveDate ='"+effectivedate+"'";
                }
                if (Location != "")
                {
                    query += " and LocationName like'%" + Location.Trim() + "%'";
                }
       
                query += @" ORDER BY  SubsNo
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var suscriptionList = uow.Repository<MainViewModel.SubscriptionViewModel>().SqlQuery(query).ToList();

                return suscriptionList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public MainViewModel.SubscriptionViewModel GetSelectedSubscription(int susid, string custType)
        {
            MainViewModel.SubscriptionViewModel customerInfoList = new MainViewModel.SubscriptionViewModel();

            customerInfoList = uow.Repository<MainViewModel.SubscriptionViewModel>().SqlQuery(@"select  * from[dbo].[fgetSubscriptionList]() where subsid={0} ", susid).SingleOrDefault();


            return customerInfoList;
        }

        public ReturnBaseMessageModel changeStatus(int? sId)
        {
            var subscription = uow.Repository<Subscription>().FindBy(x => x.Subsid == sId).FirstOrDefault();
            subscription.Status = 0;
            uow.Repository<Subscription>().Edit(subscription);
            uow.Commit();
            returnMessage.Msg = "Status Changed Successfully!";
            returnMessage.Success = true;
            returnMessage.BoolValue = true;
            return returnMessage;
        }

        public List<LocationMaster> getLocation(string prefix)
        {
            return uow.Repository<LocationMaster>().FindBy(x => x.LocationName.ToLower().StartsWith(prefix.ToLower())).ToList();
        }
    }
}
