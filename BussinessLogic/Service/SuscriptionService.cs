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


        public List<MainViewModel.SubscriptionViewModel> getSuscriberList(int? customerno, string Name, string Address, string contact, int? cType, int pageNo, int pageSize)
        {
            try
            {

                string query = "";
                if (customerno == null)
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where status=1";

                }
                else
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where CustId=  " + customerno+ " and status=1";
                }
                //where  CustomerName like'%" + Name.Trim() + "%'";

                //if (Name != "")
                //{
                //    query += " and Name like'" + Name + "%'";
                //}
                //if (customerno != null && customerno != 0)
                //{
                //    query += " and CustNo =" + customerno;
                //}
                //if (contact != "")
                //{
                //    query += " and MobileNo like'%" + contact.Trim() + "%'";
                //}
                //if (Address != "")
                //{
                //    query += " and Address like'%" + Address.Trim() + "%'";
                //}

                //if (cType != 0 && cType != null)
                //{
                //    query += " and CustTypeId =" + cType;
                //}

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
    }
}
