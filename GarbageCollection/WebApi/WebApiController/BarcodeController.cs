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
        public static string GenerateQRCode(string url,string alt= "QR Code",int height=500,int width=500,int margin =3)

        {
          
        var qrWriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions() { Height = height, Width = width, Margin = margin }

            };
            var filename= HttpContext.Current.Server.MapPath("~/Images/LogoBarcode2.png");
            Bitmap overlay = new Bitmap(filename);
            
            using (var q = qrWriter.Write(url))
            {
                //Bitmap overlay=new Bitmap()
                using (var ms=new MemoryStream())
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
    }

}
