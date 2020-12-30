using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class SubscriptionDueModel
    {

        public int cid { get; set; }
        public string CustNo { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int CollectorId { get; set; }
        public string CollectorName { get; set; }
        public int Subsid { get; set; }

        public int Subsno { get; set; }

        public decimal DueBalance { get; set; }

        public string Status { get; set; }
        public int TotalCount { get; set; }
    }



    public class HistoryModel
    {

        public int CustNo { get; set; }
        public string PostedOnBS { get; set; }
        public string CustomerName { get; set; }
        public decimal Due { get; set; }
        public decimal Balance { get; set; }
        public decimal Paid { get; set; }
       
    }
}