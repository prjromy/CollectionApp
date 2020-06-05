using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class NotificationFirebase
    {
        public class Message
        {
            public string[] registration_ids { get; set; }
            public Notification notification { get; set; }
            public object data { get; set; }
        }
        public class Notification
        {
            public string title { get; set; }
            public string text { get; set; }
        }
        public class NotificationTokenViewModel
        {
            public string RegistrationToken { get; set; }
            public int CustomerId { get; set; }
            public int Id { get; set; }
        }
    }
}