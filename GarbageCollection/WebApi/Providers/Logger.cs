using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace GarbageCollection.WebApi.Providers
{
    


        public static class Logger
        {
            public static void writeLog(HttpRequestMessage request, string response, string req = null)
            {
                // Set a variable to the Documents path.
                string dirPath = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "/");
                var file = "Logs/CollectionAppLog_" + DateTime.Now.ToString("MMMM dd, yyyy") + ".txt";
                string localPath = new Uri(dirPath + file).LocalPath;

                var message = "Log Entry: " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss") + "\r\n";
                message += "------------------------------------ \r\n";
                message += "Request Method:\r\n" + request.Method + " : " + request.RequestUri + "\r\n";
                message += "------------------------------------ \r\n";

                if (req == null)
                {
                    message += "Request:\r\n" + JsonDataResult(request.GetRouteData().Values) + "\r\n \r\n";
                }
                else
                {
                    message += "Request:\r\n" + req + "\r\n \r\n";
                }

                message += "Response:\r\n" + response + "\r\n";
                message += "-------------------------------- \r\n \r\n";

                try
                {
                    using (StreamWriter outputFile = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath(localPath)))
                    {
                        outputFile.WriteLine(message);
                    }
                }
                catch (Exception ex) { }
            }

            public static string JsonDataResult(object data)
            {
                return JsonConvert.SerializeObject(data);
            }
        }


    }

