﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
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
            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

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