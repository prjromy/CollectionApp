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

        public ReturnBaseMessageModel EditCollectionLocationCheck(int? CollectorId, int? Id, int? LocationId, string locationNames = null, string locationname = null)
        {
            bool status = true;
            var result = uow.Repository<LocationVsCollector>().FindBy(x => x.Id != Id && x.LocationId == LocationId&&x.CollectorId==CollectorId).Any();
            var locationnames = uow.Repository<LocationVsCollector>().FindBy(x => x.CollectorId == CollectorId).Select(x => x.LocationId).ToList().ToArray();
            //foreach (var item in locationnames)
            //{

            if (locationnames.Contains(LocationId)==true && result==true) {
                var singleLocationName = uow.Repository<LocationMaster>().FindBy(x => x.Lid == LocationId).Select(x => x.LocationName).SingleOrDefault();

                returnMessage.Success = false;
                returnMessage.Value = singleLocationName;
                }
                else
                {
                returnMessage.Success = true;

            }
            //}
            return returnMessage;
        }

        public ReturnBaseMessageModel AddCollectionLocationCheck(int? CollectorId, int? LocationId, string locationNames = null, string locationname = null)
        {
            var locationnames = uow.Repository<LocationVsCollector>().FindBy(x => x.CollectorId == CollectorId).Select(x=>x.LocationId).ToList().ToArray();
            string[] intre = locationNames.Split(',');

            List<int?> formList = new List<int?>();
            foreach (var item in intre)
            {
                
                formList.Add(Convert.ToInt32(item));

            }
           // var result = locationnames.Where(a => formList.Any(b => b.Contains(a)));
            //bool status = true;
            if (/*Array.Equals(locationnames, formList.ToList().ToArray())*/ locationnames.Intersect(formList).Any())

            {
                List<string> list = new List<string>();
                var result = locationnames.Intersect(formList).ToList();
                foreach (var item in result)
                {
                    var singleLocationName = uow.Repository<LocationMaster>().FindBy(x => x.Lid == item).Select(x=>x.LocationName).SingleOrDefault();
                   
                    list.Add(singleLocationName);
                   
                }
                String[] str = list.ToArray();
                returnMessage.Value = string.Join(",", str);
                returnMessage.Success = false;
            }
            else
            {
                returnMessage.Success = true;
            }
            return returnMessage;
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
