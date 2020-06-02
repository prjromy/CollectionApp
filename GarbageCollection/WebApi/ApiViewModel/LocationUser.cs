using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class LocationUser
    {
        public int UserId { get; set; }
        public int MTId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.DateTime> EffDate { get; set; }
        public Nullable<System.DateTime> TillDate { get; set; }
        public bool IsUnlimited { get; set; }
        public string UserName { get; set; }
        public string  Email { get; set; }
        public int UserDesignationId { get; set; }
        public int[] Location { get; set; }
    }
}