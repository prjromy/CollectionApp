using BussinessLogic.Repository;
using DataAccess.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace GarbageCollection.Controllers
{
    public class FCMPushNotification
    {
        public GenericUnitOfWork uow = null;
        public FCMPushNotification()
        {
            // TODO: Add constructor logic here
            uow = new GenericUnitOfWork();

        }

        public bool Successful
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }


        public FCMPushNotification SendNotification(DateTime? oldDate, DateTime? Date, string _topic, string Textarea,int? collectorid, string collectorname, int locationId, int notificationType)
        {
            FCMPushNotification result = new FCMPushNotification();
            try
            {
               
                ///To insert data in Tables

                if (notificationType == 4)
                {
                    WasteColldaysetupCustMannual(locationId, oldDate, Date, Textarea);
                        
                }
                if (notificationType == 2 || notificationType == 1)
                {
                    WasteColldaysetupCustAutoSave(oldDate, Date, _topic, Textarea, collectorid, collectorname, locationId, notificationType);
                }

                    ///to send Notification

                    result.Successful = true;
                result.Error = null;
                // var value = message;
                var requestUri = "https://fcm.googleapis.com/fcm/send";

                WebRequest webRequest = WebRequest.Create(requestUri);
                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK"));
                webRequest.Headers.Add(string.Format("Sender: id={0}", "920419562383"));
                webRequest.ContentType = "application/json";

                //if (collectorname == null)
                //{
                //    result.Successful = false;
                //    result.Response = "Please assign collector to location" + _topic+ "in Collection > Collector Location Map" ;
                //    result.Error = null;
                //    return result;

                //}

               

                var dataformat = "";
                if (Textarea == "")
                {
                    //DateTime dt = DateTime.ParseExact(Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    //string date = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    dataformat = collectorname + " will be coming for the collection on date"+ Date;
                }
                else
                {
                     dataformat = Textarea;
                }
                var data = new
                {
                    // to = YOUR_FCM_DEVICE_ID, // Uncoment this if you want to test for single device
                    to = "/topics/" + _topic, // this is for topic 

                    notification = new
                    {
                        title = "Waste Management Alert",
                        body = dataformat


                        //icon="myicon"
                    }
                };

       

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                webRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }
   
      public void WasteColldaysetupCustMannual(int locationId,DateTime? oldDate,DateTime? Date,string Textarea)
        {
            WasteColldaysetupCustMannual wasteColldaysetupCustMannual = new WasteColldaysetupCustMannual();
            wasteColldaysetupCustMannual.LocationId = locationId;
            wasteColldaysetupCustMannual.WasteFixedDay = oldDate;
            wasteColldaysetupCustMannual.WasteScheduleCollDay = Date;
            wasteColldaysetupCustMannual.Remarks = Textarea;
            uow.Repository<WasteColldaysetupCustMannual>().Add(wasteColldaysetupCustMannual);
            uow.Commit();
        }


        public void WasteColldaysetupCustAutoSave(DateTime? oldDate, DateTime? Date, string _topic, string Textarea, int? collectorid, string collectorname, int locationId, int notificationType)
        {

            if (notificationType == 2) //Collection Amount
            {
                uow.ExecWithStoreProcedure("[dbo].[PCreateCollSetup] @LocationId,@CollweekDay,@CollectionNotificationTypeId,@Message,@PostedBy,@CollectorArriveDate,@CollectorId",

                                             new SqlParameter("@LocationId", locationId),
                                             new SqlParameter("@CollweekDay", DBNull.Value),
                                             new SqlParameter("@CollectionNotificationTypeId", notificationType ),
                                                   new SqlParameter("@Message", Textarea),
                                                   new SqlParameter("@PostedBy", Global.UserId),
                                             new SqlParameter("@CollectorArriveDate", Date),
                                             new SqlParameter("@CollectorId", collectorid)


                                             );

            }
            if (notificationType == 1) //General Message
            {
                uow.ExecWithStoreProcedure("[dbo].[PCreateCollSetup] @LocationId,@CollweekDay,@CollectionNotificationTypeId,@Message,@PostedBy,@CollectorArriveDate,@CollectorId",

                                             new SqlParameter("@LocationId", locationId),
                                             new SqlParameter("@CollweekDay", DBNull.Value),
                                             new SqlParameter("@CollectionNotificationTypeId", notificationType ),
                                                   new SqlParameter("@Message", Textarea),
                                                   new SqlParameter("@PostedBy", Global.UserId),
                                             new SqlParameter("@CollectorArriveDate", DBNull.Value),
                                             new SqlParameter("@CollectorId", DBNull.Value)


                                             );
            }
          
        }

    }
}