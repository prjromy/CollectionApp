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
    
    public partial class Customer
    {
        public int Cid { get; set; }
        public string CustNo { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> CustomerTypeId { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public Nullable<System.DateTime> PostedOn { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<byte> QRCode { get; set; }
        public System.Data.Entity.Spatial.DbGeometry Geom { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
