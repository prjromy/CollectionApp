using BussinessLogic.Extensions;
using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BussinessLogic.Repository
{
    public class WeekDayRepository
    {
        private GenericUnitOfWork uow = null;

        public IQueryable<WeekDayList> weekDays { get; set; }
        public WeekDayRepository()
        {
            uow = new GenericUnitOfWork();

            weekDays = GenerateWeeks();
        }

        //Return only the results we want
        public List<WeekDayList> GetWeekDays(string searchTerm, int pageSize, int pageNum)
        {
            return GetWeeksQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        //And the total count of records
        public int GetAttendeesCount(string searchTerm, int pageSize, int pageNum)
        {
            return GetWeeksQuery(searchTerm)
                .Count();
        }


        //Our search term
        private IQueryable<WeekDayList> GetWeeksQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return weekDays
                .Where(
                    a =>
                    a.Day.Like(searchTerm)

                ) ;
        }

        //Generate test data
        private IQueryable<WeekDayList> GenerateWeeks()
        {
            //Check cache first before regenerating test data
            string cacheKey = "weekdays";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (IQueryable<WeekDayList>)HttpContext.Current.Cache[cacheKey];
            }

            var attendees = uow.Repository<WeekDayList>().GetAll().ToList();



            var result = attendees.AsQueryable();

            //Cache results
            HttpContext.Current.Cache[cacheKey] = result;

            return result;
        }
    }
}
