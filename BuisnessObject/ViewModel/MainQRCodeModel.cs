using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuisnessObject.ViewModel
{
   public class MainQRCodeModel
    {
        public class QRCodeModel
        {
            [Display(Name = "QRCode Text")]
            public string QRCodeText { get; set; }
            [Display(Name = "QRCode Image")]
            public string QRCodeImagePath { get; set; }


        }
        public class MultipleQRCodeModel
        {
            [Display(Name = "QRCode Text")]
            public string QRCodeText { get; set; }
            [Display(Name = "QRCode Image")]
            public string QRCodeImagePath { get; set; }
            public int LocationId { get; set; }
            public string custid { get; set; }
            public string custname { get; set; }
            public string generatedQRCode { get; set; }
            //public List<MultipleQRCodeModel> multipleQRCodeModel { get; set; }
        }
    }
       
   
}