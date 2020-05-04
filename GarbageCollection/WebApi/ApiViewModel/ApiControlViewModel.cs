using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class ApiControlViewModel
    {

        public class LoginViewModel
        {
            //public int EmployeeId { get; set; }
            //public int UserId { get; set; }
            //public int IsCustomer { get; set; }
            //public string PasswordHash { get; set; }
            //public string UserName { get; set; }
            //public int CustomerId { get; set; }
            public CustomerUser Album { get; set; }
            public User User { get; set; }
            //public bool IsCustomer { get; set; }
        }
        public class PostResponse
        {
            public string Result { get; set; }
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}