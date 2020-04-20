using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessObject.ViewModel
{
    public class ExcelViewModel
    {
        public class CustomerExcelViewModel
        {
            [DisplayName("Customer Number")]
            public string CustNo { get; set; }

            public string CustomerName { get; set; }
      
            public string Customertype { get; set; }

            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }

            public string Address { get; set; }
            public string Email { get; set; }
        }

        public class SubscriptionExcelViewModel
        {
                [DisplayName("Subscription No.")]
                public Nullable<int> SubsNo { get; set; }
                [DisplayName("Customer Name")]
              
                public string CustomerName { get; set; }
              
               
                [DisplayName("Effective Date")]
                public Nullable<System.DateTime> EffectiveDate { get; set; }
              
                [DisplayName("Ledger  Name")]
                public string  LedgerName { get; set; }
               
                public Nullable<decimal> MonthlyAmount { get; set; }
                [DisplayName("Location Name")]

                public string LocationName { get; set; }
                public Nullable<int> Status { get; set; }
       
            }
        public class SubscriberDueExcelViewModel
        {
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
            public decimal DueBalance { get; set; }
            public string Status { get; set; }
            [DisplayName("Ledger Name")]

            public string LedgerName { get; set; }
           
            //public Nullable<System.DateTime> CollectionDate { get; set; }
     

        }
        public class CollectorExcelViewModel
        {

            [DisplayName("Subscription No")]
            public int Subsno { get; set; }
            [DisplayName("Customer Name")]

            public string CustomerName { get; set; }
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

        }

        public class SubscriptionReportExcel
        {

            public int SubsNo { get; set; }
            public DateTime PostedOnAd { get; set; }
            public string PostedOnBs { get; set; }
            public Nullable<decimal> Debit { get; set; }
            public Nullable<decimal> Credit { get; set; }
            public Nullable<decimal> Balance { get; set; }
            public string Sources { get; set; }
            public string Custname { get; set; }
          
      

        }


        public class MonthlyDueExcelViewModel
        {


            public int Subsno { get; set; }
            public string CustomerName { get; set; }
            public string CustomerType { get; set; }

            public string LocationName { get; set; }
            public Nullable<decimal> MonthlyDue { get; set; }
            public DateTime PostedOn { get; set; }
            

        }
    }
}
