using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using ZXing;

using System.Web.Http.Cors;
using WasteManagementApi.Providers;
using DataAccess.DatabaseModel;
using BuisnessObject.ViewModel;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace WasteManagementApi.Controllers
{
    //[MyCorsPolicy]

    public class BarcodeController : ApiController
    {
        private string imagePath = @"~/Images/LogoBarcode.png";
        //public IHttpActionResult QRCode()
        //{
        //    return View();
        //}
        GarbageCollectionDBEntities db = new GarbageCollectionDBEntities();
        ReturnBaseMessageModel returnMessage = null;
        public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionControllerHandler(RouteData routeData)
                : base(routeData)
            { }
        }

        public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new SessionControllerHandler(requestContext.RouteData);
            }
        }
        public BarcodeController()
        {
            returnMessage = new ReturnBaseMessageModel();
        }

        [Route("~/api/qrcode")]
        [HttpGet]

        public string GetQRCode(string content)
        {

            HttpResponseMessage result = new HttpResponseMessage();
            string alt = "QR Code";
            int height = 500;
            int width = 500;
            int margin = 0;

            string qrResult = GenerateQRCode(content, alt, height, width, 0);

            return qrResult;
        }
        public static string GenerateQRCode(string url, string alt = "QR Code", int height = 500, int width = 500, int margin = 3)

        {

            var qrWriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions() { Height = height, Width = width, Margin = margin }

            };
            var filename = HttpContext.Current.Server.MapPath("~/Images/LogoBarcode2.png");
            Bitmap overlay = new Bitmap(filename);

            using (var q = qrWriter.Write(url))
            {
                //Bitmap overlay=new Bitmap()
                using (var ms = new MemoryStream())
                {
                    int deltaHeigth = q.Height - overlay.Height;
                    int deltaWidth = q.Width - overlay.Width;
                    Graphics g = Graphics.FromImage(q);
                    g.DrawImage(overlay, new Point(deltaWidth / 2, deltaHeigth / 2));

                    q.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }


        [Route("~/api/multipleqrcode")]
        [HttpPost]

        public ReturnBaseMessageModel MultipleGetQRCode(List<MainQRCodeModel.MultipleQRCodeModel> content)
        {

            try
            {

                //var fName = string.Format(DateTime.Now.ToString() + "- .pdf", DateTime.Now.ToString("s"));

                // HttpResponseMessage result = new HttpResponseMessage();
                string alt = "QR Code";
            int height = 500;
            int width = 500;
            int margin = 0;



            var qrWriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions() { Height = height, Width = width, Margin = margin }

            };
            var filename = HttpContext.Current.Server.MapPath("~/Images/LogoBarcode2.png");
            Bitmap overlay = new Bitmap(filename);
                //List<string> list = new List<string>();
                List<MainQRCodeModel.MultipleQRCodeModel> list = new List<MainQRCodeModel.MultipleQRCodeModel>();
              

                //list.multipleQRCodeModel = new List<MainQRCodeModel.MultipleQRCodeModel>();
                foreach (var item in content)
                {
                    MainQRCodeModel.MultipleQRCodeModel singlelist = new MainQRCodeModel.MultipleQRCodeModel();
                    using (var q = qrWriter.Write(item.custid.Trim()))
                {
                    //Bitmap overlay=new Bitmap()
                    using (var ms = new MemoryStream())
                    {
                        int deltaHeigth = q.Height - overlay.Height;
                        int deltaWidth = q.Width - overlay.Width;
                        Graphics g = Graphics.FromImage(q);
                        g.DrawImage(overlay, new Point(deltaWidth / 2, deltaHeigth / 2));

                        q.Save(ms, ImageFormat.Png);
                        var qrcode = Convert.ToBase64String(ms.ToArray());
                            //list.Add(qrcode);
                            singlelist.generatedQRCode = qrcode;
                            singlelist.custid = item.custid.Trim();
                            singlelist.custname = item.custname.Trim();
                           
                        }
                        list.Add(singlelist);
                    }
                   
                }
               
                //string result = "";
                // return( list.ToArray());
                var fName = HttpContext.Current.Session;

                //var qrResult = list.ToArray();
                var qrResult = list;
                fName["fName"]= qrResult;
                returnMessage.Value = string.Format(DateTime.Now.ToString() + ".jpg");
            returnMessage.Success = true;

              

               
                return returnMessage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Route("api/getmultipleqrcode")]
        public List<MainQRCodeModel.MultipleQRCodeModel> GetMultipleQRCode(string fName)
        {
            var ms = HttpContext.Current.Session["fName"] as List<MainQRCodeModel.MultipleQRCodeModel>;
            if (ms == null)
                return null;
            HttpContext.Current.Session["result"] = null;
            return ms;
            //return Json(ms, JsonConvert.SerializeObject(ResultGroups, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //return ms;
        }

    }
}