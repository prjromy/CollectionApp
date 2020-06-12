using PagedList;
using System;
using System.Collections.Generic;
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
}
