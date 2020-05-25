
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessObject.ViewModel
{
    public class CollectorLocationViewModel
    {
        public int Id { get; set; }
        public int? CollectorId { get; set; }
        public int? LocationId { get; set; }
        public string locationNames { get; set; }
        public int? Postedby { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<CollectorLocationViewModel> collectorLocatonList { get; set; }
    }
}
