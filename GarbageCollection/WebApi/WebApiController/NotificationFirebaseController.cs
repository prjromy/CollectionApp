using DataAccess.DatabaseModel;
using GarbageCollection.WebApi.ApiViewModel;
using GarbageCollection.WebApi.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace GarbageCollection.WebApi.WebApiController
{
    [BasicAuthentication]
    [RoutePrefix("api/notification")]
    public class NotificationFirebaseController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();

        [Route("sendnotification")]

        public static async Task<HttpResponseMessage> SendPushNotification(string[] deviceTokens,string title,string body,object data)
        {
            var applicationID = "AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK";

            var senderId = "920419562383";
            var messageInformation = new ApiViewModel.NotificationFirebase.Message()
            {
                notification = new ApiViewModel.NotificationFirebase.Notification()
                {
                    title = title,
                    text = body
                },
                data = data,
                registration_ids = deviceTokens
            };
            //Object to JSON STRUCTURE => using Newtonsoft.Json;
            string jsonMessage = JsonConvert.SerializeObject(messageInformation);
            // Create request to Firebase API
            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            //request.Headers.TryAddWithoutValidation("Authorization", "key =" ,+AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK);


            request.Headers.TryAddWithoutValidation("Authorization", "key =" + applicationID);

            request.Headers.TryAddWithoutValidation("Sender","id=" +senderId);

            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
            }
            return result;
        }
        [Route("tokensave")]
        private async Task<ReturnValue> ExecuteDataSync(NotificationFirebase.NotificationToken token)
        {
            using (TransactionScope scope = CollectorController.TransactionScopeUtils.CreateTransactionScope())
            {
                ReturnValue retVal = new ReturnValue();
                try
                {
                 
                    db.Database.ExecuteSqlCommand(String.Format(@"exec[dbo].[InsertTokenData] @RegistrationToken={0},@CustomerId={1}", token.RegistrationToken,token.CustomerId));



                    //});
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
                   // retVal.Data = new List<MainViewModel.CollectionEntry>();
                    throw;
                }
                return retVal;
            }

        }
    }



}
