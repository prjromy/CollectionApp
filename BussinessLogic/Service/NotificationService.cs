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
using static BuisnessObject.ViewModel.MainViewModel;

namespace BussinessLogic.Service
{
    public class NotificationService
    {
        ReturnBaseMessageModel returnMessage = null;
        private GenericUnitOfWork uow = null;


        public NotificationService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();

        }

        public NotificationViewModel GetMappedCollector(int? locationId)
        {
            NotificationViewModel notification = new NotificationViewModel();
            var collector = uow.Repository<LocationVsCollector>().FindBy(x => x.LocationId== locationId).ToList();
            var newcollector = (from u in collector
                                select new NotificationViewModel
                                {
                                    collectorId = Convert.ToInt32(u.CollectorId)
                                }).ToList();

            foreach (var item in newcollector)
            {
                notification.CollectorName = uow.Repository<User>().FindBy(x => x.UserId == item.collectorId).Select(x => x.UserName).SingleOrDefault();
            }
            notification.notificationList.Add(notification);
            return notification;
        }

        public List<NotificationViewModel> getCustomerList(int? LocationID, int pageNo, int pageSize)
        {
            try
            {

                string query = "";
           
                    query = "select  COUNT(*) OVER () AS TotalCount,CustNo,subsno,CustomerName,CollectorName from[dbo].[FgetNotificationlocationwiseCustomer]('" + LocationID+"') ";

               
          
                
                query += @" ORDER BY  CustNo 
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var customerList = uow.Repository<NotificationViewModel>().SqlQuery(query).ToList();

                return customerList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public CollectorViewModel getCollectorsLocationName(int? locationid)
        {


            CollectorViewModel collectorViewModel = new CollectorViewModel();
            collectorViewModel = uow.Repository<CollectorViewModel>().SqlQuery("select collectorname,l.CollectorId from dbo.fgetcollectorinfo() as c inner join[dbo].[LocationVsCollector] as l on c.CollectorID = l.CollectorId where locationid = " + locationid).SingleOrDefault();
            //if (collectorname == null)
            //{
            //    return "";
            //}
            //else
            //{
                return collectorViewModel;
            //}
          
        }





        public ReturnBaseMessageModel saveNotification(List<MainCalenderNotificationViewModel> mainCalenderNotificationViewModel, int locationId)
        {





            foreach (var item in mainCalenderNotificationViewModel)
            {

                //WasteCollDaySetupCustAuto wcs = new WasteCollDaySetupCustAuto();
                //wcs.LocationId = item.CollwkDay;
                //wcs.CollwkDay = locationId;
                //wcs.postedby = Global.UserId;
                //wcs.PostedOn = DateTime.Now;
                //wcs.CollectionNotificationTypeId = 3;
                //uow.Repository<WasteCollDaySetupCustAuto>().Add(wcs);
                uow.ExecWithStoreProcedure("[dbo].[PCreateCollSetup] @LocationId,@CollweekDay,@CollectionNotificationTypeId,@Message,@PostedBy,@CollectorArriveDate,@CollectorId",

                                              new SqlParameter("@LocationId", locationId),
                                              new SqlParameter("@CollweekDay", item.CollwkDay),
                                              new SqlParameter("@CollectionNotificationTypeId", 3),
                                                    new SqlParameter("@Message", DBNull.Value),
                                                    new SqlParameter("@PostedBy", Global.UserId),
                                              new SqlParameter("@CollectorArriveDate", DBNull.Value),
                                              new SqlParameter("@CollectorId", DBNull.Value)


                                              );
              
            }



            returnMessage.Msg = "Added Successfully";
            returnMessage.Success = true;

            return returnMessage;

    
        }
        public List<MainCalenderNotificationViewModel> getCalenderNotificationList(string location, string weekDay, int pageNo, int pageSize)
        {
            try
            {
              

                string query = "";
                query = @"select COUNT(*) OVER () AS TotalCount,a.Id,CollwkDay,w.Day ,lc.LocationName as LocationName,
                        LocationId,postedby,PostedOn from WasteCollDaySetupCustAuto as a
                        inner join WeekDayList as w on a.CollwkDay = w.Id

                        inner join LocationMaster as lc on a.LocationId = lc.Lid

                        where CollectionNotificationTypeId = 3 ";

                if (location != "" && location != null)
                {
                    query += "  and LocationName like'%" + location.Trim() + "%'";
                }
                if (weekDay != "" && weekDay != null)
                {
                    query += " and Day like '%" + weekDay.Trim() + "%'";
                }

                query += @" ORDER BY  Id
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var collectionList = uow.Repository<MainCalenderNotificationViewModel>().SqlQuery(query).ToList();

                return collectionList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<WeekDayList> getWeekDayList(string prefix)
        {
            return uow.Repository<WeekDayList>().FindBy(x => x.Day.ToLower().StartsWith(prefix.ToLower())).ToList();
        }

        public List<MainViewModel.CalenderMainViewModel> getWasteCollectionCalenderList(int year, int month, int location)
        {
            string query = "select DatesAd,AssignedLocationName from FgetAssignLocationfoCalander('"+year+"','"+ month + "','"+ location + "') where locationid=" + location+" and convert(DATE,Datesad)>CONVERT(DATE,GETDATE())";

            
            var calenderList = uow.Repository<MainViewModel.CalenderMainViewModel>().SqlQuery(query).ToList();

            return calenderList;
        }
    }
}
