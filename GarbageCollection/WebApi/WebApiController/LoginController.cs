﻿
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
using System.Data.Entity.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
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
        [Route("api/basiclogin")]
        public async Task<IHttpActionResult> PostLogin([FromBody] OAuthGrantResourceOwnerCredentialsContext context)
        {


            //var isCustomer=  HttpContext.Current.Request.Params["IsCustomer"];
            if (context.ClientId == "User")
            {
                PasswordHasher pass = new PasswordHasher();
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                var user = db.Users.Where(x => x.UserName == context.UserName.Trim()).FirstOrDefault();
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


        /// <summary>
        /// // For JWT Login
        /// </summary>
        /// <param name="context"></param>
        /// <returns>token and user details</returns>

        [HttpPost]
        [Route("api/login")]
        public HttpResponseMessage LoginDemo([FromBody] ApiControlViewModel.LoginViewModel context)
        {


            //var isCustomer=  HttpContext.Current.Request.Params["IsCustomer"];
            if (context.ClientId == "User")
            {
                PasswordHasher pass = new PasswordHasher();
              
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                User user = new User();
                if (context.Email != null)
                {
                    
                    user = db.Users.Where(x => x.Email == context.Email).FirstOrDefault();
                }
                if (context.UserName != null)
                {
                    user = db.Users.Where(x => x.UserName == context.UserName.Trim()).FirstOrDefault();
                }
               
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



                        AuthenticationModule authentication = new AuthenticationModule();
                        string tokens = authentication.GenerateTokenForUser(user.UserName, user.UserId);
                        //var sul = new LocationUser
                        //{
                        //EmployeeId = user.EmployeeId,
                        //Email = user.Email,
                        //UserId = user.UserId,
                        //UserName = user.UserName,
                        //EffDate = user.EffDate,
                        //TillDate = user.TillDate,
                        //MTId = user.MTId,
                        //IsUnlimited = user.IsUnlimited,
                        //UserDesignationId = user.UserDesignationId,
                        //Location = myintlist,
                        var token = tokens;
                        //};
                       
                       return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
                        //return Ok(new { results = sul });
                      
                    }
                    //else
                    //{
                    //    AuthenticationModule authentication = new AuthenticationModule();
                    //    string tokens = authentication.GenerateTokenForUser(user.UserName, user.UserId);
                    //    //var sul = new LocationUser
                    //    //{
                    //    //EmployeeId = user.EmployeeId,
                    //    //Email = user.Email,
                    //    //UserId = user.UserId,
                    //    //UserName = user.UserName,
                    //    //EffDate = user.EffDate,
                    //    //TillDate = user.TillDate,
                    //    //MTId = user.MTId,
                    //    //IsUnlimited = user.IsUnlimited,
                    //    //UserDesignationId = user.UserDesignationId,
                    //    var token = tokens;

                    //    //};
                    //    //return Ok(new { results = sul });

                    //    return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
                    //}
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "User is not collector ", Configuration.Formatters.JsonFormatter);
                    }

                }

                //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));



                else if (user != null && user.IsActive == false)
                {

                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User", Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found ", Configuration.Formatters.JsonFormatter);
                }




            }

            else if (context.ClientId == "Customer")
            {
                PasswordHasher pass = new PasswordHasher();
                CustomerUserTable user = new CustomerUserTable();
                if (context.UserName != null)
                {
                     user = db.CustomerUserTables.Where(x => x.UserName == context.UserName).FirstOrDefault();
                }
                if(context.Email!=null)
                {
                    user = db.CustomerUserTables.Where(x => x.Email == context.Email).FirstOrDefault();
                }
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
               // var user = db.CustomerUserTables.Where(x => x.UserName == context.UserName).FirstOrDefault();

                //CustomerUser user = db.CustomerUsers.Where(x => x.UserName == context.UserName).FirstOrDefault();

                if (user != null && user.IsActive == true && pass.VerifyHashedPassword(user.PasswordHash, context.Password) != PasswordVerificationResult.Failed)
                {
                    AuthenticationModule authentication = new AuthenticationModule();
                    string tokens = authentication.GenerateTokenForUser(user.UserName, user.UserId);

                    //var sul = new customerUser
                    //{
                    //CustomerId = user.CustomerId,
                    //Email = user.Email,
                    //UserId = user.UserId,
                    //UserName = user.UserName,
                    //EffDate = user.EffDate,
                    //TillDate = user.TillDate,
                    //MTId = user.MTId,
                    //IsUnlimited = user.IsUnlimited,
                    var token = tokens;
                    //};
                    //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));
                    //return Ok(new { results = sul });
                   
                    return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);


                }
                else if (user != null && user.IsActive == false)
                {

                    //return BadRequest("Customer Not Active");
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "User Not Active", Configuration.Formatters.JsonFormatter);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found", Configuration.Formatters.JsonFormatter);
                }


            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found", Configuration.Formatters.JsonFormatter);
            }
          

        }




        [JWTAuthenticationFilter]

        [HttpGet]
        [Route("api/userdetails")]
        public HttpResponseMessage UserDetails(int userId,string ClientId="")
        {


            //var isCustomer=  HttpContext.Current.Request.Params["IsCustomer"];
            if (ClientId == "User")
            {
                //PasswordHasher pass = new PasswordHasher();
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                var user = db.Users.Where(x => x.UserId == userId).FirstOrDefault();
                // password is correct 


                //var userManager = context.OwinContext.GetUserManager<Loader.UserManager>();
                // var user = await userManager.FindAsync(context.UserName, context.Password);

                //Loader.Models.ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
                if (user != null && user.IsActive == true )
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
                            Location = myintlist,
                            
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, sul, Configuration.Formatters.JsonFormatter);
                        //return Ok(new { results = sul });

                    }
                    else
                    {
                       
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
                            UserDesignationId = user.UserDesignationId

                    };
                    //return Ok(new { results = sul });

                    return Request.CreateResponse(HttpStatusCode.OK, sul, Configuration.Formatters.JsonFormatter);
                    }

                }

                //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));



                else if (user != null && user.IsActive == false)
                {

                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User", Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found ", Configuration.Formatters.JsonFormatter);
                }




            }

            else if (ClientId == "Customer")
            {
                //var hashedPassword = EncodePassword(context.Password, MembershipPasswordFormat.Hashed, "MAKV2SPBNI99212");
                var user = db.CustomerUserTables.Where(x => x.UserId == userId).FirstOrDefault();

                //CustomerUser user = db.CustomerUsers.Where(x => x.UserName == context.UserName).FirstOrDefault();

                if (user != null && user.IsActive == true )
                {


                    var sul = new customerUser
                    {
                        CustomerId = user.CustomerId,
                        Email = user.Email,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        EffDate = user.EffDate,
                        TillDate = user.TillDate,
                        MTId = user.MTId,
                        IsUnlimited = user.IsUnlimited
                };
                //Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(context));
                //return Ok(new { results = sul });

                return Request.CreateResponse(HttpStatusCode.OK, sul, Configuration.Formatters.JsonFormatter);


                }
                else if (user != null && user.IsActive == false)
                {

                    //return BadRequest("Customer Not Active");
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "User Not Active", Configuration.Formatters.JsonFormatter);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found", Configuration.Formatters.JsonFormatter);
                }


            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found", Configuration.Formatters.JsonFormatter);
            }


        }
        [JWTAuthenticationFilter]
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


                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                    //scope.Dispose();
                    //retVal.Status = false;
                    //throw;
                }
                return retVal;
            }
            


        }
    }
}
