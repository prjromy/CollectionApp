using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BuisnessObject.ViewModel
{
    public class CustomerUserViewModel
    {
        public int CustomerUserId { get; set; }
        public Nullable<int> MTId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Customer Name is required")]
        [DisplayName("Customer Name")]
        public Nullable<int> CustomerId { get; set; }
        [DisplayName("From Date")]
        public Nullable<System.DateTime> EffDate { get; set; }
        [DisplayName("To Date")]
        public Nullable<System.DateTime> TillDate { get; set; }
        [DisplayName("Is Unlimited")]
        public bool IsUnlimited { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email is required")]

        public string Email { get; set; }
          [DisplayName("Password")]
        public string PasswordHash { get; set; }
        [Compare("PasswordHash", ErrorMessage = "Password and Confirmation Password must match.")]
        [DisplayName("Confirm Password")]
        public string ReEnterPassword { get; set; }
        [DisplayName("User Name")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "UserName is required")]

        public string UserName { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<CustomerUserViewModel> customeruserPagedList { get; set; }
    }
}
