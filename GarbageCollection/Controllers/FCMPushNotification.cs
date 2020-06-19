﻿using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
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
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();

        public FCMPushNotification()
        {
            // TODO: Add constructor logic here
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


        public FCMPushNotification SendNotification( string _topic,int locationid)
        {
            FCMPushNotification result = new FCMPushNotification();
            try
            {
                result.Successful = true;
                result.Error = null;
                // var value = message;
                var requestUri = "https://fcm.googleapis.com/fcm/send";

                WebRequest webRequest = WebRequest.Create(requestUri);
                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA1k1H848:APA91bFsW-DqHhhnQXTblzGyr9Gx-nT-VUud50ckUTvhCnkolzkFp81yUeHWI7A2ZUUiTgMWrhBDw1QLdFvnVx1Nwtl-JmyaZq0C4UzsAHfNyDuQ9OGeG5ZYHPioek8gQlD_gvzi1FOK"));
                webRequest.Headers.Add(string.Format("Sender: id={0}", "920419562383"));
                webRequest.ContentType = "application/json";
               string query = String.Format("select collectorname from dbo.fgetcollectorinfo() as c inner join [dbo].[LocationVsCollector]  as l on c.CollectorID=l.CollectorId where locationid=" + locationid);

                string collectorname = db.Database.SqlQuery<string>(query).SingleOrDefault();
                if (collectorname == null)
                {
                    result.Successful = false;
                    result.Response = "Please assign collector to location" + _topic+ "in Collection > Collector Location Map" ;
                    result.Error = null;
                    return result;

                }
                var data = new
                {
                    // to = YOUR_FCM_DEVICE_ID, // Uncoment this if you want to test for single device
                    to = "/topics/" + _topic, // this is for topic 
                    notification = new
                    {
                        title = "Waste Management Alert",
                        body =collectorname+ " will be coming for the collection."
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
    }
}