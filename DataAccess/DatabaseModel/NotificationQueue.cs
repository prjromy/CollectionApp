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
    
    public partial class NotificationQueue
    {
        public int NId { get; set; }
        public Nullable<int> Cid { get; set; }
        public Nullable<int> Custno { get; set; }
        public Nullable<int> locationId { get; set; }
        public Nullable<int> CollectorId { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public string Message { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
    }
}
