using DataAccess.DatabaseModel;
using GarbageCollection.WebApi.ApiViewModel;
using GarbageCollection.WebApi.Providers;
using GarbageCollection.WebApi.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GarbageCollection.WebApi.WebApiController
{
    //[BasicAuthentication]
    [JWTAuthenticationFilter]

    [RoutePrefix("api/location")]
    public class LocationController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
        [HttpGet]
        [Route("locationlist")]
        public IEnumerable<LocationViewModel> LocationList([FromUri]  PagingParameterModel pagingparametermodel)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
                //var collectorid = HttpContext.Current.Session["CustomerUserId"];


                string query = String.Format("select * from [dbo].[LocationMaster]");

                List<LocationViewModel> returnData = db.Database.SqlQuery<LocationViewModel>(query).ToList();
                //get's no of rows
                int count = returnData.Count();

                //parameter is passed from query strng if it is null then then it default value will be pagenumber 1
                int CurrentPage = pagingparametermodel.pageNumber;
                int PageSize = pagingparametermodel.pageSize;
                int TotalCount = count;
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
                var items = returnData.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                var prevousPage = CurrentPage > 1 ? "Yes" : "No";
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                var paginationMetaData = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    prevousPage,
                    nextPage

                };
                if (returnData == null)

                {
                    //resMsg.message = "No data found";
                    var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null));
                    throw new HttpResponseException(response);
                }

                Logger.writeLog(Request, Logger.JsonDataResult(returnData), Logger.JsonDataResult(null));
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetaData));
                return items;
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
            Logger.writeLog(Request, message, req);

            return response;
        }

        [HttpGet]
        [Route("singlelocationlist")]
        public IEnumerable<LocationViewModel> SingleLocation(int? locationId)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
                //var collectorid = HttpContext.Current.Session["CustomerUserId"];


                string query = String.Format("select * from [dbo].[LocationMaster] where Lid="+ locationId);

                List<LocationViewModel> returnData = db.Database.SqlQuery<LocationViewModel>(query).ToList();
               

               
                return returnData;
            }
            catch (Exception ex)
            {
                resMsg.message = "Something went wrong. Please try again.";
                var response = this.getMessage(resMsg, HttpStatusCode.PreconditionFailed, false, Logger.JsonDataResult(null), Logger.JsonDataResult(ex));
                throw new HttpResponseException(response);
            }

        }

    }
}
