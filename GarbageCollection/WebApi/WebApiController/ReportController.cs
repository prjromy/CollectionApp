using BuisnessObject.ViewModel;
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
    [JWTAuthenticationFilter]
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
        [HttpGet]
        [Route("subscriberreportlist")]
        public IEnumerable<MainViewModel.SubscriptionReport> GetSubscriberList([FromUri]  PagingParameterModel pagingparametermodel, int? subsid,DateTime? FromDate,DateTime? ToDate)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
                // var collectorid=HttpContext.Current.Session["CustomerUserId"];

                string query = String.Format("select COUNT(*) OVER () AS TotalCount, SubsNo,Custname,PostedOnAd,PostedOnBs,Debit,Credit,Balance,Sources from [dbo].[fgetSubscriberStmnt] (" + subsid + ",'" + FromDate + "','" + ToDate + "')");
                //string query = String.Format("select COUNT(*) OVER () AS TotalCount,SubsId,CollectorId,LocationID,Collectorname,CustId,Subscollid,subsno,LocationID,verifiedBy as SubsNo,CustomerName,LocationName,CollectionDate,CollectionAmt ,DiscountAmt,CollectionType,PostedBy   from fgetCollectionlist()  where verifiedby is null and CollectorId=" + collectorid + "and CollectionType=" + "'Mobile'");

                List<MainViewModel.SubscriptionReport> returnData = db.Database.SqlQuery<MainViewModel.SubscriptionReport>(query).ToList();

              
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

        [HttpGet]
        [Route("subscriptionlist")]
        public IEnumerable<MainViewModel.SubscriptionViewModel> GetSubscriptionList([FromUri]  PagingParameterModel pagingparametermodel, int? customerid)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
                // var collectorid=HttpContext.Current.Session["CustomerUserId"];

                string query = String.Format("select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetSubscriptionList]() where CustId= "+ customerid);
                //string query = String.Format("select COUNT(*) OVER () AS TotalCount,SubsId,CollectorId,LocationID,Collectorname,CustId,Subscollid,subsno,LocationID,verifiedBy as SubsNo,CustomerName,LocationName,CollectionDate,CollectionAmt ,DiscountAmt,CollectionType,PostedBy   from fgetCollectionlist()  where verifiedby is null and CollectorId=" + collectorid + "and CollectionType=" + "'Mobile'");

                List<MainViewModel.SubscriptionViewModel> returnData = db.Database.SqlQuery<MainViewModel.SubscriptionViewModel>(query).ToList();


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
                    resMsg.message = "No data found";
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
    }
}
