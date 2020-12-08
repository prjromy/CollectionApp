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
            
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string ClientId { get; set; }
        }
        public class PostResponse
        {
            public string Result { get; set; }
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}