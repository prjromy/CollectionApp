using GarbageCollection.WebApi.Loggers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.WebHost;

namespace GarbageCollection.App_Start
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            config.MessageHandlers.Add(new RequestResponseHandler());

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new WasteManagementApi.Controllers.BarcodeController.SessionHttpControllerRouteHandler(), true));
            }
            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional }, null);
            JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}