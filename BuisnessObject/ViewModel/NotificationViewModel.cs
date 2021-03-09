using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessObject.ViewModel
{
    public class NotificationViewModel
    {
        public int LocationId { get; set;}
        public int collectorId { get; set; }
        public int CustNo { get; set; }
        public int subsno { get; set; }
        public string CustomerName { get; set; }
        public string CollectorName { get; set; }

       
        //public string collectorName { get; set; }
        public List<NotificationViewModel> notificationList { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<NotificationViewModel> notificationpagedList { get; set; }


    }


    public class MainCalenderNotificationViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public Nullable<int> LocationId { get; set; }

        public string LocationName { get; set; }
        public string Weekday { get; set; }
        [Required(ErrorMessage = "WeekDay is required")]
        public Nullable<int> CollwkDay { get; set; }
        public string weekDays { get; set; }
        public Nullable<int> CollectionNotificationTypeId { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CollectorArriveDate { get; set; }
        public Nullable<int> postedby { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
        public int TotalCount { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int CollectorId { get; set; }
        public bool IsChecked { get; set; }

        public IPagedList<MainCalenderNotificationViewModel> calenderList { get; set; }

    }


}
