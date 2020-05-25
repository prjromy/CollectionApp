using GarbageCollection.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GarbageCollection.WebApi.WebApiController
{
    [BasicAuthentication]
    [RoutePrefix("api/notification")]
    public class NotificationFirebaseController : ApiController
    {

    }
}
