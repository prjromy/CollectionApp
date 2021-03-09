using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;
namespace BuisnessObject.ViewModel
{
    public class CustomerUserViewModel
    {
        public int UserId { get; set; }
        public Nullable<int> MTId { get; set; }
        [Required(ErrorMessage ="Customer Name is required")]
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
        [Required(ErrorMessage = "Email is required")]

        public string Email { get; set; }
          [DisplayName("Password")]
        public string PasswordHash { get; set; }
        [DisplayName("Confirm Password")]
        [Compare("PasswordHash", ErrorMessage = "Confirm password doesn't match, Type again !")]

        public string ReEnterPassword { get; set; }
    


     
        public string NewPassword { get; set; }
        [DisplayName("New Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]

        public string NewReEnterPassword { get; set; }

        [Required(ErrorMessage = "UserName is required")]

        [DisplayName("User Name")]
        [Remote("CheckUsernameAvailable", "CustomerUser", AdditionalFields = "UserId", ErrorMessage = "Duplicate Username entry!!!")]
        public string UserName { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<CustomerUserViewModel> customeruserPagedList { get; set; }
    }
}
