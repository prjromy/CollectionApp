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
        public string collectorName { get; set; }
        public List<NotificationViewModel> notificationList { get; set; }
    }
}
