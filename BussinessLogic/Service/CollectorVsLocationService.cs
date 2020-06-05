using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using DataAccess.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class CollectorVsLocationService
    {
        ReturnBaseMessageModel returnMessage = null;
        private GenericUnitOfWork uow = null;


        public CollectorVsLocationService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();

        }
        public List<CollectorLocationViewModel> getCollectorLocationList(string collector, string location, int pageNo, int pageSize)
        {
            try { 
            //{
            //    if (collector == 0)
            //    {
            //        collector = null;
            //    }

                string query = "";
                query = "select COUNT(*) OVER () AS TotalCount,Id,CollectorId,LocationId,Postedby,PostedOn,CollectorName,LocationName from [dbo].[FgetCollectorVsLocationList]() where Postedby is not null ";

                if (collector !="" && collector != null)
                {
                    query += "  and CollectorName like'%" + collector.Trim()+"%'";
                }
                if (location!=""&&location!=null)
                {
                    query += " and LocationName like '%" + location.Trim()+ "%'";
                }
            
                query += @" ORDER BY  Id
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var collectionList = uow.Repository<CollectorLocationViewModel>().SqlQuery(query).ToList();

                return collectionList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ReturnBaseMessageModel saveCollectionLocation( List<CollectorLocationViewModel> collectorLocationViewModel, int collectorId)
        {
           
           
            
                
               
                foreach (var item in collectorLocationViewModel)
                {
                //var singlecollectorLocations = uow.Repository<LocationVsCollector>().FindBy(x => x.Id == item.Id).SingleOrDefault();

                LocationVsCollector loc = new LocationVsCollector();
                    loc.LocationId = item.LocationId;
                    loc.CollectorId = collectorId;
                    loc.Postedby = Global.UserId;
                    loc.PostedOn = DateTime.Now;
                    uow.Repository<LocationVsCollector>().Add(loc);
                    returnMessage.Msg = "Added Successfully";
                    returnMessage.Success = true;
                }
                
              
          
            
        
            uow.Commit();
            return returnMessage;

            //if (collectorLocationViewModel.Id != 0)
            //{

            //}
        }
        public ReturnBaseMessageModel EditCollectionLocation(CollectorLocationViewModel singleCollector, int collectorId)
           
        {




            var singlecollectorLocations = uow.Repository<LocationVsCollector>().FindBy(x => x.Id == singleCollector.Id).SingleOrDefault();
         
                singlecollectorLocations.LocationId = singleCollector.LocationId;
                singlecollectorLocations.CollectorId = collectorId;
                singlecollectorLocations.Postedby = Global.UserId;
                singlecollectorLocations.PostedOn = DateTime.Now;
                uow.Repository<LocationVsCollector>().Edit(singlecollectorLocations);
                returnMessage.Msg = "Edited Successfully";
                returnMessage.Success = true;

            



            uow.Commit();
            return returnMessage;

            //if (collectorLocationViewModel.Id != 0)
            //{

            //}
        }
        public CollectorLocationViewModel singleCollectorLocation(int? id)
        {
            var singleLocationCollector = uow.Repository<LocationVsCollector>().FindBy(x => x.Id == id);
            var singleLocationCollect = (from u in singleLocationCollector
                                           select new CollectorLocationViewModel
                                           {
                                               Id = u.Id,
                                               CollectorId = u.CollectorId,
                                               LocationId = u.LocationId,
                                               Postedby = u.Postedby,
                                               PostedOn = u.PostedOn
                                           }).SingleOrDefault();
            return singleLocationCollect;
        }
    }
}
