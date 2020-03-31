﻿using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessObject.ViewModel
{
    public class MainViewModel
    {
        public class CustomerViewModel
        {
            public int Cid { get; set; }
            [DisplayName("Customer Number")]
            public string CustNo { get; set; }
            [Required(ErrorMessage = "Customer Name is required")]
            [DisplayName("Customer Name")]
            public string CustomerName { get; set; }
            [Required(ErrorMessage = "Customer Type is required")]
            [DisplayName("Customer Type")]
            public Nullable<int> CustomerTypeId { get; set; }

            [Required(ErrorMessage = "Customer type is required")]
            [DisplayName("Customer Type Name")]
            public string Customertype { get; set; }
            [Required(ErrorMessage = "Phone Number is required")]
            [DisplayName("Phone No")]
            
            
             // DataType(DataType.PhoneNo)]
              //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string PhoneNo { get; set; }
            [Required(ErrorMessage = "Mobile No is required")]
            [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
            [DisplayName("Mobile No")]
            public string MobileNo { get; set; }

            [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }
            public string PanNo { get; set; }
            public System.Data.Entity.Spatial.DbGeometry Geom { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public Nullable<int> PostedBy { get; set; }
            public Nullable<System.DateTime> PostedOn { get; set; }
            public int TotalCount { get; set; }
           public bool IsSelected { get; set; }
            public bool Isselect { get; set; }
            public string SearchOption { get; set; }
            public IEnumerable<int> CIDs { get; set; }
            public string SearchParameter { get; set; }
            public List<CustomerViewModel> customerViewModelList { get; set; }
            public IPagedList<CustomerViewModel> customerPagedList { get; set; }
        }
        public class SubscriptionViewModel
        {
            public int Subsid { get; set; }
            public Nullable<int> SubsNo { get; set; }
            [DisplayName("Customer Name")]
            public string CustomerName { get; set; }
            public int CustId { get; set; }
            [DisplayName("Effective Date")]
            public Nullable<System.DateTime> EffectiveDate { get; set; }
            [DisplayName("Ledger  Name")]
            public Nullable<int> LedgerId { get; set; }
            public Nullable<decimal> MonthlyAmount { get; set; }
            [DisplayName("Location Name")]
            public Nullable<int> LocationID { get; set; }
            public Nullable<int> Status { get; set; }
            public string Remarks { get; set; }
            public Nullable<int> PostedBy { get; set; }
            public Nullable<System.DateTime> PostedOn { get; set; }
            public Nullable<int> ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public int TotalCount { get; set; }
            public List<SubscriptionViewModel> suscriberViewModelList { get; set; }
            public IPagedList<SubscriptionViewModel> suscriberPagedList { get; set; }
        }

        public class LedgerViewModel
        {

            public int LedgerId{ get; set; }
            public string LedgerName { get; set; }
        }
        }
}
