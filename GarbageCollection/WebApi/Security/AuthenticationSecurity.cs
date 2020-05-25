using DataAccess.DatabaseModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GarbageCollection.WebApi.Security
{
    public class AuthenticationSecurity
    {
        //for employee login
        public static HttpStatusCode Login(string username, string password)
        {
            using (GarbageCollectionDBEntities entities = new GarbageCollectionDBEntities())
            {
                PasswordHasher pass = new PasswordHasher();
                var user = entities.Users.Where(x => x.UserName == username).FirstOrDefault();
                var cususer = entities.CustomerUserTables.Where(x => x.UserName == username).FirstOrDefault();
                if (user != null && pass.VerifyHashedPassword(user.PasswordHash, password) != PasswordVerificationResult.Failed)
                {
                    return HttpStatusCode.OK;
                }

                else if(cususer!=null && pass.VerifyHashedPassword(cususer.PasswordHash, password) != PasswordVerificationResult.Failed)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            }
        }

        //for customer login
        //public static bool CustomerLogin(string username, string password)
        //{
        //    PasswordHasher pass = new PasswordHasher();
        //    using (GarbageCollectionDBEntities entities = new GarbageCollectionDBEntities())
        //    {

        //    }
        //}

    }

}