using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.Loggers
{
    public class MessageLogging
    {
        public void IncomingMessageAsync(API_Log apiLog)
        {
            apiLog.RequestType = "Request";
            var sqlErrorLogging = new ApiLogging();
            sqlErrorLogging.InsertLog(apiLog);
        }

        public void OutgoingMessageAsync(API_Log apiLog)
        {
            apiLog.RequestType = "Response";
            var sqlErrorLogging = new ApiLogging();
            sqlErrorLogging.InsertLog(apiLog);
        }
    }
}