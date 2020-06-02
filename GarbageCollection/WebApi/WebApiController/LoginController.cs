
using BuisnessObject.ViewModel;
using DataAccess.DatabaseModel;
using GarbageCollection.WebApi.ApiViewModel;
using GarbageCollection.WebApi.Providers;
using GarbageCollection.WebApi.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

using System.Web.Script.Serialization;
using System.Web.Security;

namespace GarbageCollection.WebApi.WebApiController
{

    //[RoutePrefix("api/user")]
    public class LoginController : ApiController
    {


        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();

        private static async void GetAPIToken(ApiControlViewModel.LoginViewModel model)
        {
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath;
            string responseJson = await ResponseAsStringAsync(string.Format("{0}/token", tokenServiceUrl),
                 new[]
                 {
                new KeyValuePair<string, string>("password", model.Password),
                new KeyValuePair<string, string>("username", model.UserName),
                new KeyValuePair<string, string>("grant_type", "password"),
                 });

            var jObject = JObject.Parse(responseJson);
            string token = jObject.GetValue("access_token").ToString();
        }

        public static async Task<string> ResponseAsStringAsync(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            string responseString = string.Empty;

            var uri = new Uri(url);

            using (var client = new HttpClient())
            using (var content = new FormUrlEncodedContent(postData))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.PostAsync(uri, content);

                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }






        // POST api/user/login


        [HttpPost]
        [Route("api/login")]
        public async Task<IHttpActionResult> PostLogin([FromBody] OAuthGrantResourceOwnerCredentialsContext context)
        {


            //var isCustomer=  HttpContext.Current.Request.Params["IsCustomer"];
            if (context.ClientId == "User")
            {
                PasswordHasher pass = new PasswordHasher();
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                var user = db.Users.Where(x => x.UserName == context.UserName).FirstOrDefault();
                // password is correct 


                //var userManager = context.OwinContext.GetUserManager<Loader.UserManager>();
                // var user = await userManager.FindAsync(context.UserName, context.Password);

                //Loader.Models.ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
                if (user != null && user.IsActive == true && pass.VerifyHashedPassword(user.PasswordHash, context.Password) != PasswordVerificationResult.Failed)
                {
                    if (user.UserDesignationId == 11)
                    {
                        var locations = String.Format("select  locationid from  fgetlocationlistbycollector('" + user.UserId + "')");


                        List<int> returnData = db.Database.SqlQuery<int>(locations).ToList();
                        int[] myintlist = returnData.ToArray();




                        var sul = new LocationUser
                        {
                            EmployeeId = user.EmployeeId,
                            Email = user.Email,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            EffDate = user.EffDate,
                            TillDate = user.TillDate,
                            MTId = user.MTId,
                            IsUnlimited = user.IsUnlimited,
                            UserDesignationId = user.UserDesignationId,
                            Location = myintlist
                        };
                        return Ok(new { results = sul });

                    }
                    else
                    {
                        var sul = new User
                        {
                            EmployeeId = user.EmployeeId,
                            Email = user.Email,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            EffDate = user.EffDate,
                            TillDate = user.TillDate,
                            MTId = user.MTId,
                            IsUnlimited = user.IsUnlimited,
                            UserDesignationId = user.UserDesignationId,

                        };
                        return Ok(new { results = sul });

                    }

                }

                //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));



                else if (user != null && user.IsActive == false)
                {

                    return BadRequest("User Not Active");
                }
                else
                {
                    return NotFound();
                }




            }

            else if (context.ClientId == "Customer")
            {
                PasswordHasher pass = new PasswordHasher();
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                var user = db.CustomerUserTables.Where(x => x.UserName == context.UserName).FirstOrDefault();

                //CustomerUser user = db.CustomerUsers.Where(x => x.UserName == context.UserName).FirstOrDefault();

                if (user != null && user.IsActive == true && pass.VerifyHashedPassword(user.PasswordHash, context.Password) != PasswordVerificationResult.Failed)
                {


                    var sul = new CustomerUserTable
                    {
                        CustomerId = user.CustomerId,
                        Email = user.Email,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        EffDate = user.EffDate,
                        TillDate = user.TillDate,
                        MTId = user.MTId,
                        IsUnlimited = user.IsUnlimited,

                    };
                    //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));
                    return Ok(new { results = sul });



                }
                else if (user != null && user.IsActive == false)
                {

                    return BadRequest("Customer Not Active");
                }
                else
                {

                    return NotFound();
                }


            }
            else
            {
                return NotFound();
            }

        }


        [HttpPost]
        [Route("api/savecustomertoken")]
        public async Task<ReturnValue> SaveToken([FromBody]NotificationToken context)
        {
            ReturnValue retVal = new ReturnValue();
            using (TransactionScope scope = CollectorController.TransactionScopeUtils.CreateTransactionScope())
            {
                try
                {
                   


                    db.NotificationTokens.Add(new NotificationToken()
                    {

                        CustomerId = context.CustomerId,
                        RegistrationToken = context.RegistrationToken
                    });

                    db.SaveChanges();

                    scope.Complete();
                    retVal.Status = true;


                }


                catch (Exception ex)
                {
                    scope.Dispose();
                    retVal.Status = false;
                    throw;
                }
                return retVal;
            }
            


        }
    }
}
