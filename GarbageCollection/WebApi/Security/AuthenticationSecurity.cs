using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.Security
{
    public class AuthenticationSecurity
    {
        //for employee login
        public static bool Login(string username, string password)
        {
            using (GarbageCollectionDBEntities entities = new GarbageCollectionDBEntities())
            {
                return entities.Users.Any(user =>
                       user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.PasswordHash == password);
            }
        }

        //for customer login
        public static bool CustomerLogin(string username, string password)
        {
            using (GarbageCollectionDBEntities entities = new GarbageCollectionDBEntities())
            {
                return entities.CustomerUsers.Any(user =>
                       user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.PasswordHash == password);
            }
        }

    }

}