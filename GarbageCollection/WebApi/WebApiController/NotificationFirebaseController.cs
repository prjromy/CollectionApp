using DataAccess.DatabaseModel;
using FCM.Net;
using GarbageCollection.WebApi.ApiViewModel;
using GarbageCollection.WebApi.Providers;
using GarbageCollection.WebApi.Security;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GarbageCollection.WebApi.WebApiController
{
    //[JWTAuthenticationFilter]
    [RoutePrefix("api/notification")]
    public class NotificationFirebaseController : ApiController
    {
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();



        //public static async Task<HttpResponseMessage> SendPushNotification(string[] deviceTokens)
        //{
        //    var applicationID = "AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK";

        //    var senderId = "920419562383";
        //    var messageInformation = new ApiViewModel.NotificationFirebase.Message()
        //    {
        //        notification = new ApiViewModel.NotificationFirebase.Notification()
        //        {
        //            title = "From waste",
        //            text = "test data",
        //        },
        //       // data = { 'title':'hello'},
        //        registration_ids = deviceTokens
        //    };
        //    //Object to JSON STRUCTURE => using Newtonsoft.Json;
        //    string jsonMessage = JsonConvert.SerializeObject(messageInformation);
        //    // Create request to Firebase API
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
        //    //request.Headers.TryAddWithoutValidation("Authorization", "key =" ,+AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK);


        //    request.Headers.TryAddWithoutValidation("Authorization", "key =" + applicationID);

        //    request.Headers.TryAddWithoutValidation("Sender","id=" +senderId);

        //    request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
        //    HttpResponseMessage result;
        //    using (var client = new HttpClient())
        //    {
        //        result = await client.SendAsync(request);
        //    }
        //    return result;
        //}






         
        [Route("sendnotification")]
        

        [HttpPost]
        public async Task<bool> NotifyAsync([FromBody] NotificationFirebase.NotificationTopic topic)
        {
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK");

                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", "920419562383");

                //var data = new
                //{
                //    to = "/topics/" + topic._topic,  // Recipient device token
                //    notification = new { topic.title, topic.body }
                //};
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
    {
        { "score", "850" },
        { "time", "2:45" },
    },
                    To = "/topics/"+topic._topic,
                };
                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(message);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);
                        //Console.ReadLine();
                        //Console.WriteLine();
                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            //Logger.writeLog($"Error sending notification. Status Code: {result.StatusCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

            return false;
        }




    }





    }
