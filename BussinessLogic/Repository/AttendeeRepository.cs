using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BussinessLogic.Extensions;
using DataAccess.DatabaseModel;
//using System.Web.Caching;
//using DataAccess.DatabaseModel;

namespace BussinessLogic.Repository
{
    public class AttendeeRepository
    {
        private GenericUnitOfWork uow = null;

        public IQueryable<LocationMaster> Attendees { get; set; }
        public AttendeeRepository()
        {
            uow = new GenericUnitOfWork();

            Attendees = GenerateAttendees();
        }

        //Return only the results we want
        public List<LocationMaster> GetAttendees(string searchTerm, int pageSize, int pageNum)
        {
            return GetAttendeesQuery(searchTerm)              
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        //And the total count of records
        public int GetAttendeesCount(string searchTerm, int pageSize, int pageNum)
        {
            return GetAttendeesQuery(searchTerm)
                .Count();
        }


        //Our search term
        private IQueryable<LocationMaster> GetAttendeesQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return Attendees
                .Where(
                    a =>
                    a.LocationName.Like(searchTerm) 
                    
                );
        }

        //Generate test data
        private IQueryable<LocationMaster> GenerateAttendees()
        {
            //Check cache first before regenerating test data
            string cacheKey="attendees";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (IQueryable<LocationMaster>)HttpContext.Current.Cache[cacheKey];
            }

            var attendees = uow.Repository<LocationMaster>().GetAll().ToList();

            //for (int i = 0; i < 1000; i++)
            //{
            //    attendees.Add(
            //        new LocationMaster()
            //        {
            //            AttendeeId = i,
            //            FirstName = "First " + i.ToString(),
            //            LastName = "Last " + i.ToString()
            //        }
            //        );
            //}

            var result = attendees.AsQueryable();

            //Cache results
            HttpContext.Current.Cache[cacheKey] = result;

            return result;
        }
    }
}