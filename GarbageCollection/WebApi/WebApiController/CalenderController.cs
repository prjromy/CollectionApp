using BuisnessObject.ViewModel;
using DataAccess.DatabaseModel;
using GarbageCollection.WebApi.Providers;
using GarbageCollection.WebApi.Security;
using Loader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Script.Serialization;
namespace GarbageCollection.WebApi.WebApiController
{
    [RoutePrefix("api/calender")]
    [JWTAuthenticationFilter]
    public class CalenderController : ApiController
    {


        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
        [HttpGet]
        [Route("List")]
        public IEnumerable<MainViewModel.CalenderMainViewModel> List(int? year, int? month, int? location)
            {
                ResponseMessage resMsg = new ResponseMessage();
                try
                {

                    string query = String.Format(" select COUNT(*) OVER() AS TotalCount, * from FgetAssignLocationfoCalander(" + year + "," + month + "," + location + ")");


                    List<MainViewModel.CalenderMainViewModel> returnData = db.Database.SqlQuery<MainViewModel.CalenderMainViewModel>(query).ToList();

                    if (returnData == null)

                    {
                        resMsg.message = "No data found";
                        var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null));
                        throw new HttpResponseException(response);
                    }

                    return returnData;
                }
                catch (Exception ex)
                {
                    resMsg.message = "Something went wrong. Please try again.";
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null), Logger.JsonDataResult(ex));
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

                return response;
            }
        }
    }

