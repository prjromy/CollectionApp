using PagedList;
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
            [Required(ErrorMessage ="Customer Name is required ")]
            public string CustomerName { get; set; }
            public int? CustId { get; set; }
            [Required(ErrorMessage = "Effective Date is required ")]
            [DisplayName("Effective Date")]
            public Nullable<System.DateTime> EffectiveDate { get; set; }
            [Required(ErrorMessage = "Ledger Name is required ")]
            [DisplayName("Ledger  Name")]
            public Nullable<int> LedgerId { get; set; }
            [Required(ErrorMessage = "Monthly Amount is required ")]
            public Nullable<decimal> MonthlyAmount { get; set; }
            [DisplayName("Location Name")]
            [Required(ErrorMessage = "Location Name is required ")]
            public Nullable<int> LocationID { get; set; }
            public Nullable<int> Status { get; set; }
            [Required(ErrorMessage = "Remarks is required ")]
            public string Remarks { get; set; }
            public Nullable<int> PostedBy { get; set; }
            public Nullable<System.DateTime> PostedOn { get; set; }
            public Nullable<int> ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public int TotalCount { get; set; }
            public List<SubscriptionViewModel> suscriberViewModelList { get; set; }
            public IPagedList<SubscriptionViewModel> suscriberPagedList { get; set; }
            public string ModelFrom { get; set; }
        }
        public class CollectionViewModel
        {
            public int DueId { get; set; }
            public Nullable<int> SubsId { get; set; }
            public Nullable<int> Fyid { get; set; }
            public Nullable<int> BillNo { get; set; }
            public Nullable<int> Year { get; set; }
            public Nullable<int> Month { get; set; }
            public Nullable<decimal> MonthlyDueAmt { get; set; }
           public string LocationName { get; set; }
            public Nullable<decimal> ReceivedAmount { get; set; }
            public Nullable<decimal> Discount { get; set; }
            public Nullable<System.DateTime> PostedOn { get; set; }
            public string ModelFrom { get; set; }
            public bool IsChecked { get; set; }
            public string Status { get; set; }
           // public Nullable<System.DateTime> EffectiveDate { get; set; }
            public List<CollectionViewModel> collectionList { get; set; }
        }
        public class LedgerViewModel
        {

            public int LedgerId{ get; set; }
            public string LedgerName { get; set; }
        }

        public class CollectionEntry
        {
            public Nullable<int> SubsNo { get; set; }
            public string LocationName { get; set; }
            public String Status { get; set; }
            public decimal MonthlyAmount { get; set; }
        }
        }
}
