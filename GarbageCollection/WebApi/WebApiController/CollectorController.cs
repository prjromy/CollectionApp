using BuisnessObject.ViewModel;
using DataAccess.DatabaseModel;
using GarbageCollection.WebApi.ApiViewModel;
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

    [BasicAuthentication]
    [RoutePrefix("api/collectors")]
    public class CollectorController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
        [Route("customer")]
        //[HttpGet]
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

        [Route("{id}/customerdetail")]
        public MainViewModel.CustomerViewModel GetCustomerDetail(int id)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {


                string query = String.Format("SELECT  CustNo ,CustomerName ,CustomerTypeId,MobileNo, Address, Email, PanNo, PostedOn FROM Customer where Cid ={0}", id);

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

        [Route("{id}")]
        public IEnumerable<MainViewModel.CollectionEntry> Get(int id)
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

                List<MainViewModel.CollectionEntry> returnData = db.Database.SqlQuery<MainViewModel.CollectionEntry>(query).ToList();
                if (returnData == null)

                {
                    //resMsg.message = "No data found";
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

        [HttpGet]
        [Route("collectionentrylist")]
        public IEnumerable<MainViewModel.CollectionVerificationEntry> GetCollectionVerify([FromUri]  PagingParameterModel pagingparametermodel,int? collectorid)
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
               // var collectorid=HttpContext.Current.Session["CustomerUserId"];


                string query = String.Format("select COUNT(*) OVER () AS TotalCount,SubsId,CollectorId,LocationID,Collectorname,CustId,Subscollid,subsno,LocationID,verifiedBy as SubsNo,CustomerName,LocationName,CollectionDate,CollectionAmt ,DiscountAmt,CollectionType,PostedBy   from fgetCollectionlist()  where verifiedby is null and CollectorId=" + collectorid/*+ "and CollectionType="+"'Mobile'"*/);

                List<MainViewModel.CollectionVerificationEntry> returnData = db.Database.SqlQuery<MainViewModel.CollectionVerificationEntry>(query).ToList();
               
                foreach (var item in returnData)
                {
                    if (item.verifiedBy == null)
                    {
                        item.Status = "Pending";

                    }
                    else
                    {
                        item.Status = "Verified";
                    }
                    returnData.Add(item);
                }
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
                currentPage=CurrentPage,
                totalPages=TotalPages,
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
        [Route("defaultlocationsubscriptiondue")]
        public IEnumerable<SubscriptionDueModel> DefaultMonthlyDueForCollector([FromUri]  PagingParameterModel pagingparametermodel,int? locationid,int? collectorid,string searchterm="")
        {
            ResponseMessage resMsg = new ResponseMessage();
            try
            {
                //var collectorid = HttpContext.Current.Session["CustomerUserId"];


                string query = String.Format("select  COUNT(*) OVER () AS TotalCount,* from  FgetNotificationlocationwise('" + locationid + "','" +collectorid+ "')");
                
                if (!string.IsNullOrEmpty(searchterm.Trim(new Char[] { '*', ' ', '\'' })))
                {
                    query += "where CustomerName Like '"+ searchterm.ToLower().Trim() + "%'";
                }

                List<SubscriptionDueModel> returnData = db.Database.SqlQuery<SubscriptionDueModel>(query).ToList();
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

        [HttpPost]
        [Route("collectionentry")]
       
        public async Task<ReturnValue> ExecuteDataSync([FromBody] List<CollectorViewModel> collListObj)
        {
            using (TransactionScope scope = TransactionScopeUtils.CreateTransactionScope())
            {
                ReturnValue retVal = new ReturnValue();
                try
                {
                   collListObj.ToList().ForEach(collObj =>

                    {
                        if (collObj.Discount == null)
                        {
                            collObj.Discount = 0;
                        }
                    db.Database.ExecuteSqlCommand(String.Format(@"exec[dbo].[PsetCollectionDueMobile]  @custId={0},@Subsid={1},@CollectorId={2}, @CollectionAmt={3},@CollDiscount={4},@CollectionDate='{5}',@PostedBy={6}",
                      collObj.custid, collObj.SubsId, collObj.CollectorId, collObj.ReceivedAmount, collObj.Discount, collObj.CollectionDate, collObj.UserId));
                      
                    
                    
                    });
                    //var data = String.Format("select COUNT(*) OVER() AS TotalCount, subsid, Subsno as SubsNo, CustomerName, LocationName, DueBalance as MonthlyAmount, Debit, Status from fgetSubscriptionDueReport('" + DateTime.Now + "') where custid = " + collListObj.SingleOrDefault().custid);
                    //List<MainViewModel.CollectionEntry> returnData = db.Database.SqlQuery<MainViewModel.CollectionEntry>(data).ToList();
                    scope.Complete();
                    retVal.Status = true;
                    //retVal.Data = returnData;

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

        [Route("collectionentryverify")]
        private async Task<ReturnValue> CollectionEntryVerify(MainViewModel.CollectionVerificationEntry[] collListObj)
        {
            using (TransactionScope scope = TransactionScopeUtils.CreateTransactionScope())
            {
                ReturnValue retVal = new ReturnValue();
                try
                {
                    collListObj.ToList().ForEach(collObj =>

                    {
                         
                        db.Database.ExecuteSqlCommand(String.Format(@"exec[dbo].[PsetCollectionDue]  @SubsCollId={0},@custId={1},@Subsid={2},@CollectorId={3}, @CollectionAmt={4},@CollDiscount={5},@CollectionDate='{6}',@PostedBy={7}",
                         collObj.Subscollid, collObj.CustId, collObj.subsid, collObj.CollectorId, collObj.CollectionAmt, collObj.DiscountAmt, collObj.CollectionDate, Global.UserId));



                    });
                    var data = String.Format("select COUNT(*) OVER() AS TotalCount, subsid, Subsno as SubsNo, CustomerName, LocationName, DueBalance as MonthlyAmount, Debit, Status from fgetSubscriptionDueReport('" + DateTime.Now + "') where custid = " + collListObj.SingleOrDefault().CustId);
                    List<MainViewModel.CollectionVerificationEntry> returnData = db.Database.SqlQuery<MainViewModel.CollectionVerificationEntry>(data).ToList();
                    scope.Complete();
                    retVal.Status = true;
                    retVal.ColVerData = returnData;

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    retVal.Status = false;
                    retVal.ColVerData = new List<MainViewModel.CollectionVerificationEntry>();
                    throw;
                }
                return retVal;
            }

        }
        

        public class TransactionScopeUtils
        {
            public static TransactionScope CreateTransactionScope()
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
                transactionOptions.Timeout = TransactionManager.MaximumTimeout;

                return new TransactionScope(TransactionScopeOption.RequiresNew, TransactionManager.MaximumTimeout, TransactionScopeAsyncFlowOption.Enabled);
            }
        }


    }
}
