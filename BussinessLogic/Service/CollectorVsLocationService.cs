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
        public List<CollectorLocationViewModel> getCollectorLocationList(int? collector, int? location, int pageNo, int pageSize)
        {
            try
            {
                if (collector == 0)
                {
                    collector = null;
                }

                string query = "";
                query = "select COUNT(*) OVER () AS TotalCount,Id,CollectorId,LocationId,Postedby,PostedOn from LocationVsCollector where Postedby is not null";

                if (collector != 0 && collector != null)
                {
                    query += "  and CollectorId =" + collector;
                }
                if (location!=0&&location!=null)
                {
                    query += "and LocationId =" + location;
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

        public ReturnBaseMessageModel saveCollectionLocation(CollectorLocationViewModel collectorLocationViewModel, string[] locationIdList)
        {
            foreach (string locationId in locationIdList)
            {
                int locationid = Convert.ToInt32(locationId);
                var singlecollectorLocations = uow.Repository<LocationVsCollector>().FindBy(x => x.Id == locationid).SingleOrDefault();
                if (singlecollectorLocations == null)
                {
                    LocationVsCollector loc = new LocationVsCollector();
                    singlecollectorLocations.LocationId = locationid;
                    singlecollectorLocations.CollectorId = collectorLocationViewModel.CollectorId;
                    singlecollectorLocations.Postedby = Global.UserId;
                    singlecollectorLocations.PostedOn = DateTime.Now;
                    uow.Repository<LocationVsCollector>().Add(singlecollectorLocations);
                    returnMessage.Msg = "Added Successfully";
                    returnMessage.Success = true;

                }
                else
                {
                    singlecollectorLocations.LocationId = collectorLocationViewModel.LocationId;
                    singlecollectorLocations.CollectorId = collectorLocationViewModel.CollectorId;
                    singlecollectorLocations.Postedby = Global.UserId;
                    singlecollectorLocations.PostedOn = DateTime.Now;
                    uow.Repository<LocationVsCollector>().Edit(singlecollectorLocations);
                    returnMessage.Msg = "Edited Successfully";
                    returnMessage.Success = true;

                }
            }
            
        
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
