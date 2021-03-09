
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BuisnessObject.ViewModel
{
    public class CollectorLocationViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Collector is required")]
        public int? CollectorId { get; set; }
        [Required(ErrorMessage = "Location is required")]
        //[Remote("IsAlreadyAsSigned", "CollectorLocationMap", AdditionalFields = "CollectorId", HttpMethod = "POST", ErrorMessage = "EmailId already exists in database.")]
        public int? LocationId { get; set; }
        public string locationNames { get; set; }
        public int? Postedby { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<CollectorLocationViewModel> collectorLocatonList { get; set; }
        public string CollectorName { get; set; }
        public string LocationName { get; set; }

    }
}
