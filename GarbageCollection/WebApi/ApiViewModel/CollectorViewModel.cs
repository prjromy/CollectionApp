using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class CollectorViewModel
    {
        
            public Nullable<int> SubsId { get; set; }

            public int custid { get; set; }
         
           
            public Nullable<decimal> ReceivedAmount { get; set; }
      
            public Nullable<decimal> Discount { get; set; }
           
      
            public int CollectorId { get; set; }
            public DateTime CollectionDate { get; set; }
            public int UserId { get; set; }
   

        
    }
}