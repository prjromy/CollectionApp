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
    }
}
