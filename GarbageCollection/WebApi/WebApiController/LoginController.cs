
using BuisnessObject.ViewModel;
using DataAccess.DatabaseModel;

using GarbageCollection.WebApi.Providers;
using GarbageCollection.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;

using System.Web.Script.Serialization;

namespace GarbageCollection.WebApi.WebApiController
{
    //[BasicAuthentication]
    [MyCorsPolicy]
    
    public class LoginController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
       
        [HttpPost]
        [Route("api/login")]
        public ApiViewModel.ApiControlViewModel.PostResponse PostLogin(bool IsCustomer,[FromBody] ApiViewModel.ApiControlViewModel.LoginViewModel usrObj)
        {
            //var isCustomer=  HttpContext.Current.Request.Params["IsCustomer"];
            if (!IsCustomer)
            {
                var hexString = HashPassword(usrObj.User.PasswordHash);
                User user = db.Users.Where(x => x.UserName == usrObj.User.UserName && x.PasswordHash == hexString).FirstOrDefault();

                if (user != null && user.IsActive == true)
                {
                    usrObj.User.PasswordHash = null;
                    if (user.UserDesignationId == 11)
                    {
                        var sul = new User
                        {
                            EmployeeId = Convert.ToInt32(user.EmployeeId),
                            UserId = user.UserId,
                            UserName = user.UserName
                            //UserDesignationId = user.UserDesignationId
                        };
                        Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(usrObj));
                        return new ApiViewModel.ApiControlViewModel.PostResponse()
                        {
                            IsSuccess = true,
                            Result = String.Format("{0} {1} {2}", sul.EmployeeId, sul.UserId, sul.UserName)
                        };
                    }
                    else
                    {
                        var error = new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sorry, user access was denied."));
                        Logger.writeLog(Request, "Sorry, user access was denied.", Logger.JsonDataResult(usrObj));
                        return new ApiViewModel.ApiControlViewModel.PostResponse()
                        {
                            IsSuccess = false,
                            Result = String.Format("{0}", error)
                        };
                    }
                }
                else if (user != null && user.IsActive == false)
                {
                    var error = new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username not Active."));
                    Logger.writeLog(Request, "Username not Active.", Logger.JsonDataResult(usrObj));
                    return new ApiViewModel.ApiControlViewModel.PostResponse()
                    {
                        IsSuccess = false,
                        Result = String.Format("{0}", error)
                    };
                }
                else
                {
                    var error = new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username or password was incorrect."));
                    Logger.writeLog(Request, "Username or password was incorrect.", Logger.JsonDataResult(usrObj));
                    return new ApiViewModel.ApiControlViewModel.PostResponse()
                    {
                        IsSuccess = false,
                        Result = String.Format("{0}", error)
                    };
                }

            }
            else
            {
                var hexString = HashPassword(usrObj.User.PasswordHash);
                CustomerUser user = db.CustomerUsers.Where(x => x.UserName == usrObj.User.UserName && x.PasswordHash == hexString).FirstOrDefault();

                if (user != null && user.IsActive == true)
                {
                    usrObj.User.PasswordHash = null;
                    
                        var sul = new CustomerUser
                        {
                            CustomerId = Convert.ToInt32(user.CustomerId),
                            CustomerUserId = user.CustomerUserId,
                            UserName = user.UserName
                            //UserDesignationId = user.UserDesignationId
                        };
                        Logger.writeLog(Request, Logger.JsonDataResult(sul), Logger.JsonDataResult(usrObj));
                        return new ApiViewModel.ApiControlViewModel.PostResponse()
                        {
                            IsSuccess = true,
                            Result = String.Format("{0} {1} {2}", sul.CustomerId, sul.CustomerId, sul.UserName)
                        };
                   
                 
                }
                else if(user != null && user.IsActive == false)
                {
                    var error = new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username not Active."));
                    Logger.writeLog(Request, "Username not Active.", Logger.JsonDataResult(usrObj));
                    return new ApiViewModel.ApiControlViewModel.PostResponse()
                    {
                        IsSuccess = false,
                        Result = String.Format("{0}", error)
                    };
                }
                else
                {
                    var error = new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username or password was incorrect."));
                    Logger.writeLog(Request, "Username or password was incorrect.", Logger.JsonDataResult(usrObj));
                    return new ApiViewModel.ApiControlViewModel.PostResponse()
                    {
                        IsSuccess = false,
                        Result = String.Format("{0}", error)
                    };
                }
            }       
        }
       

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        //        Verifying:

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        [Route("api/login/customer")]
        public MainViewModel.CustomerViewModel GetCustomer()
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {

               
                string query = String.Format("select COUNT(*) OVER() AS TotalCount, * from[dbo].[fgetCustomerTB]()");

                MainViewModel.CustomerViewModel returnData = db.Database.SqlQuery<MainViewModel.CustomerViewModel>(query).FirstOrDefault();
                if (returnData == null)
                {
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null));
                    throw new HttpResponseException(response);
                }

                Logger.writeLog(Request, Logger.JsonDataResult(returnData), Logger.JsonDataResult(null));

                return returnData;
            }
            catch (Exception ex)
            {
                resMsg.message = "Something went wrong. Please try again.";
                var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null), Logger.JsonDataResult(ex));
                throw new HttpResponseException(response);
            }

        }

        [Route("api/login/{id)}/customerdetail")]
        public MainViewModel.CustomerViewModel GetCustomerDetail(int id)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {


                string query = String.Format("SELECT  CustNo ,CustomerName ,CustomerTypeId,MobileNo, Address, Email, PanNo, PostedOn FROM Customer where Cid ={ 0} "id);

                MainViewModel.CustomerViewModel returnData = db.Database.SqlQuery<MainViewModel.CustomerViewModel>(query).FirstOrDefault();
                if (returnData == null)
                {
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null));
                    throw new HttpResponseException(response);
                }

                Logger.writeLog(Request, Logger.JsonDataResult(returnData), Logger.JsonDataResult(null));

                return returnData;
            }
            catch (Exception ex)
            {
                resMsg.message = "Something went wrong. Please try again.";
                var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null), Logger.JsonDataResult(ex));
                throw new HttpResponseException(response);
            }

        }
        [Route("api/login/{id}")]
        public MainViewModel.CollectionEntry Get(int id)
        {
             ResponseMessage resMsg = new ResponseMessage();
            try
            {
                
                if (id == 0)
                {
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(id));
                    throw new HttpResponseException(response);
                }
                string query = String.Format("select COUNT(*) OVER() AS TotalCount, subsid, Subsno as SubsNo, CustomerName, LocationName, DueBalance as MonthlyAmount, Debit, Status from fgetSubscriptionDueReport('" + DateTime.Now + "') where custid = " + id);

                MainViewModel.CollectionEntry returnData = db.Database.SqlQuery<MainViewModel.CollectionEntry>(query).FirstOrDefault();
                if (returnData == null)
                {
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(id));
                    throw new HttpResponseException(response);
                }

                Logger.writeLog(Request, Logger.JsonDataResult(returnData), Logger.JsonDataResult(id));

                return returnData;
            }
            catch (Exception ex)
            {
                resMsg.message = "Something went wrong. Please try again.";
                var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(id), Logger.JsonDataResult(ex));
                throw new HttpResponseException(response);
            }

        }
        public HttpResponseMessage getMessage(ResponseMessage resMsg, HttpStatusCode code, bool flag, string req = null, string ex = null)
        {
            var json = new JavaScriptSerializer().Serialize(resMsg);
            var response = new HttpResponseMessage(flag == true ? HttpStatusCode.OK : HttpStatusCode.NotFound)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = code
            };
            var message = Logger.JsonDataResult(resMsg);
            if (ex != null) message = String.Format("{0} \r\n {1}", message, ex);
            Logger.writeLog(Request, message, req);

            return response;
        }
    }
}
