
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


                    var sul = new User
                    {
                        EmployeeId = user.EmployeeId,
                        Email=user.Email,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        EffDate = user.EffDate,
                        TillDate = user.TillDate,
                        MTId = user.MTId,
                        IsUnlimited = user.IsUnlimited,
                        UserDesignationId = user.UserDesignationId
                    };

                    //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));
                    return Ok(new { results = sul });

                }
                else if (user != null && user.IsActive == false)
                {

                    return NotFound();
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

                    return NotFound();
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

        //public static string HashPassword(string password)
        //{
        //    byte[] salt;
        //    byte[] buffer2;
        //    if (password == null)
        //    {
        //        throw new ArgumentNullException("password");
        //    }
        //    using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        //    {
        //        salt = bytes.Salt;
        //        buffer2 = bytes.GetBytes(0x20);
        //    }
        //    byte[] dst = new byte[0x31];
        //    Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        //    Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        //    return Convert.ToBase64String(dst);
        //}
        //        Verifying:




    }
}
