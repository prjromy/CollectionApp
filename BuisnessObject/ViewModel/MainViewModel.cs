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
            [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter a number")]
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
            //[Required(ErrorMessage = "Phone Number is required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter a number")]
            [DisplayName("Phone No")]
            
            
             // DataType(DataType.PhoneNo)]
              //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string PhoneNo { get; set; }
            [Required(ErrorMessage = "Mobile No is required")]
            [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "10 digit mobile number required.")]
            [DisplayName("Mobile No")]
            public string MobileNo { get; set; }

            [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; }

            //[Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }
            //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter a number")]
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
            public string Printed { get; set; }
            public List<CustomerViewModel> customerViewModelList { get; set; }
            public IPagedList<CustomerViewModel> customerPagedList { get; set; }
        }
        public class SubscriptionViewModel
        {
            public string CustNo { get; set; }
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
            //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter a number")]
            public Nullable<decimal> MonthlyAmount { get; set; }
            [DisplayName("Location Name")]
            
            public Nullable<int> LocationID { get; set; }
            public string LedgerName { get; set; }
            public string CompanyName { get; set; }
            [Required(ErrorMessage = "Location Name is required ")]
            public string LocationName { get; set; }
            public Nullable<int> Status { get; set; }
            [Required(ErrorMessage = "Remarks is required ")]
            public string Remarks { get; set; }
            public Nullable<int> PostedBy { get; set; }
            public Nullable<System.DateTime> PostedOn { get; set; }
            public Nullable<int> ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public int TotalCount { get; set; }
            public string SearchOption { get; set; }
            public string SearchParameter { get; set; }
            public string QRStatus { get; set; }
            public bool IsChecked { get; set; }
            public Nullable<decimal> DharautiAmt { get; set; }
            public Nullable<decimal> RegistrationFee { get; set; }
            public List<SubscriptionViewModel> suscriberViewModelList { get; set; }
            public IPagedList<SubscriptionViewModel> suscriberPagedList { get; set; }
            public string ModelFrom { get; set; }
        }
        public class CollectionViewModel
        {
            public int DueId { get; set; }
            public Nullable<int> SubsId { get; set; }
       
            public int custid { get; set; }
            public Nullable<int> Fyid { get; set; }
            public Nullable<int> BillNo { get; set; }
            public Nullable<int> Year { get; set; }
            public Nullable<int> Month { get; set; }
            [DisplayName("Balance")]
            public Nullable<decimal> MonthlyDueAmt { get; set; }
            [DisplayName("Location Name")]
            public string LocationName { get; set; }
            [DisplayName("Received Amount(In Amount)")]
           
            public Nullable<decimal> ReceivedAmount { get; set; }
            [DisplayName("Discount(In Amount)")]
            public Nullable<decimal> Discount { get; set; }
            [DisplayName("Collected Date")]
            public Nullable<System.DateTime> PostedOn { get; set; }
            public string ModelFrom { get; set; }
            public bool IsChecked { get; set; }
            public string Status { get; set; }
            [DisplayName("Collector Name")]
            [Required(ErrorMessage ="Collector Required")]
            public int CollectorId { get; set; }
            public DateTime CollectionDate { get; set; }
            public int UserId { get; set; }
            public int TotalCount { get; set; }
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
            public int subsid { get; set; }
            public Nullable<int> SubsNo { get; set; }
            public string LocationName { get; set; }
            public String Status { get; set; }
            public decimal MonthlyAmount { get; set; }
            public decimal Debit { get; set; }
            public int TotalCount { get; set; }
            public string CustomerName { get; set; }
            public IPagedList<CollectionEntry> collectionPagedList { get; set; }
        }


        public class CollectionVerificationEntry
        {
            [DisplayName("Customer No.")]
            public int CustId { get; set; }
            public string CustomerNo { get; set; }
            public int subsid { get; set; }
            public int Subscollid { get; set; }
            public string Collectorname { get; set; }
            [DisplayName("Collector Name")]
            public int CollectorId { get; set; }
            [DisplayName("Subscription No.")]

            public Nullable<int> subsno { get; set; }
            [DisplayName("Location Name")]

            public string LocationName { get; set; }
            [DisplayName("Collection Amount")]

            public decimal CollectionAmt { get; set; }
            [DisplayName("Discount Amount")]

            public decimal DiscountAmt { get; set; }
            public int TotalCount { get; set; }
            [DisplayName("Customer Name")]
            public string CustomerName { get; set; }
            [DisplayName("Collection Date")]

            public int LocationID { get; set; }
            public Nullable<System.DateTime> CollectionDate { get; set; }
            public string CollectionType { get; set; }
            public string PostedBy { get; set; }
            public string verifiedBy { get; set; }
            public string Status { get; set; }
            public IPagedList<CollectionVerificationEntry> collectionPagedList { get; set; }
        }
        public class CollectorDetail
        {
            [DisplayName("Collector Name")]

            public int CollectorID { get; set; }
            public string CollectorName { get; set; }
            public string Status { get; set; }
        }

        public class SubscriberDueViewModel
        {
            public string CustNo { get; set; }
            [DisplayName("Subscription No")]
            public int Subsno { get; set; }
            [DisplayName("Customer Name")]

            public string CustomerName { get; set; }
            [DisplayName("Customer Type")]

            public string CustomerType { get; set; }
            [DisplayName("Location Name")]

            public string LocationName { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public decimal Advance { get; set; }
            public decimal DueBalance { get; set; }

            public decimal SumDebit { get; set; }
            public decimal SumCredit { get; set; }
            public decimal SumAdvance { get; set; }
            public decimal SumDueBalance { get; set; }
            public string Status { get; set; }
            [DisplayName("Ledger Name")]

            public string LedgerName { get; set; }
            public int LocationId { get; set; }
            public Nullable<System.DateTime> CollectionDate { get; set; }
            public int TotalCount { get; set; }
            public IPagedList<SubscriberDueViewModel> suscriberPagedList { get; set; }

        }

        public class CollectionReport
        {
    
            public string CustomerNo { get; set; }
            [DisplayName("Subscription No")]
            public int Subsno { get; set; }
            [DisplayName("Customer Name")]

            public string CustomerName { get; set; }
         

            public string CollectionType { get; set; }
            [DisplayName("Customer Type")]

            public string CustomerType { get; set; }
            [DisplayName("Location Name")]

            public string LocationName { get; set; }
            [DisplayName("Collector Name")]
            public string CollectorName { get; set; }

            [DisplayName("Collection Date")]
            public Nullable<System.DateTime> CollectionDate { get; set; }
            [DisplayName("Collection Amount")]
            public Nullable<decimal> CollectionAmt { get; set; }
            [DisplayName("Discount Amount")]
            public Nullable<decimal> DiscountAmt { get; set; }
      
            public int TotalCount { get; set; }
            public IPagedList<CollectionReport> collectorPagedList { get; set; }

        }

     
              public class SubscriptionReport
        {
            public string CustNo { get; set; }
           public int SubsNo { get; set; }
            public DateTime PostedOnAd { get; set; }
            public string PostedOnBs { get; set; }
            public Nullable<decimal> Debit { get; set; }
            public Nullable<decimal> Credit { get; set; }
            public Nullable<decimal> Balance { get; set; }
            public string Sources { get; set; }
            [DisplayName("Customer Name")]
            public string Custname { get; set; }
            public int TotalCount { get; set; }
            public string ModelFrom { get; set; }
            public IPagedList<SubscriptionReport> subscriptionPagedList { get; set; }

        }

        public class MonthlyDueViewModel
        {
           
            public string Custno { get; set; }
            public int Subsno { get; set; }
            [DisplayName("Customer Name")]
            public string CustomerName { get; set; }
            [DisplayName("Customer Type")]
            public string CustomerType { get; set; }
            [DisplayName("Location Name")]
            public string LocationName { get; set; }
            [DisplayName("Monthly Due")]
            public Nullable<decimal> MonthlyDue { get; set; }
            [DisplayName("Posted On")]
            public DateTime PostedOn { get; set; }
            public int TotalCount { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
            public IPagedList<MonthlyDueViewModel> monthlyDuePagedList { get; set; }

        }
    }
}
