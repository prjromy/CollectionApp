using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
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

        public SuscriptionService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();

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

        public List<MainViewModel.SubscriptionViewModel> getSuscriberList(int? customerno, string Name, string Address, string contact, int? cType, int pageNo, int pageSize)
        {
            try
            {

                string query = "";
                query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where CustId=  "+ customerno;
                    
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
    }
}
