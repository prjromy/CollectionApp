//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubscriptionDue
    {
        public int DueId { get; set; }
        public Nullable<int> SubsId { get; set; }
        public Nullable<int> Fyid { get; set; }
        public Nullable<int> BillNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<decimal> MonthlyDueAmt { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
    }
}
