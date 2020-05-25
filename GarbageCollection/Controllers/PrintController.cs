
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    public class PrintController : Controller
    {
        // GET: Print
        //public void PrintQRCode(HttpPostedFileBase file,int id)
        //{
        //    try
        //    {
             

        //        var allowedExtensions = new[] {
        //    ".Jpg", ".png", ".jpg", "jpeg"
        //};
        //    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
        //    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg) 

        //    if (allowedExtensions.Contains(ext)) //check what type of extension  
        //    {
        //        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
        //        string myfile = name + "_" + id + ext; //appending the name with id  
        //                                                   // store the file inside ~/project folder(Img)  
        //        var path = Path.Combine(Server.MapPath("~/BarCodeImage"), myfile);
               
        //        file.SaveAs(path);
        //        PrintDocument pd = new PrintDocument();
        //        pd.DefaultPageSettings.PrinterSettings.PrinterName = "EPSON LQ-310 ESC/P2";

        //        pd.DefaultPageSettings.Landscape = true; //or false!
        //        pd.PrintPage += (sender, args) =>
        //        {
        //            Image i = Image.FromFile(@"~/BarCodeImage" + myfile);
        //            Rectangle m = args.PageBounds;

        //            if ((double)i.Width / (double)i.Height > (double)m.Width / (double)m.Height) // image is wider
        //            {
        //                m.Height = (int)((double)i.Height / (double)i.Width * (double)m.Width);
        //            }
        //            else
        //            {
        //                m.Width = (int)((double)i.Width / (double)i.Height * (double)m.Height);
        //            }
        //            args.Graphics.DrawImage(i, m);
        //        };
        //        pd.Print();
        //    }
        //    else {
        //        ViewBag.message = "Please choose only Image file";
        //    }
        //    }
        //    catch(Exception e)
        //    {
        //        throw e;
        //    }
        //}

            [HttpPost]
        public ActionResult  PDF(string files,int? custNo,string custName)
        {
            try
            {
                var file = files.Split(',')[1];
                var bytes = Convert.FromBase64String(file);
                var fName = string.Format(DateTime.Now.ToString() +"-"+custNo+".pdf", DateTime.Now.ToString("s"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    //Initialize the PDF document object.
                    using (Document pdfDoc = new Document(PageSize.A4, 100f, 10f, 10f, 10f))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        //Add the Image file to the PDF document object.
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bytes);
                        img.ScaleAbsolute(200f, 200f);

                        pdfDoc.Add(img);
                        pdfDoc.Add(new Paragraph("Customer No :" + custNo));
                        pdfDoc.Add(new Paragraph("Customer Name :" + custName)); 
                        pdfDoc.Close();
                      
                        //Download the PDF file.
                        //return File(stream.ToArray(), "application/pdf", "ImageExport.pdf");
                      

                    }
                      var bytes1 = stream.ToArray();
                        Session[fName] = bytes1;

                }
                return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);
            }

            catch(Exception e)
            {
                throw e;
            }

        }

        [HttpPost]
        public ActionResult MultiplePDF(string[] files, int[] custNo, string[] custName)
        {
            try
            {
                //string[] files = collection["files"];
                //    , int[] custNo, string[] custName
                var fName = string.Format(DateTime.Now.ToString() + "- .pdf", DateTime.Now.ToString("s"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    //Initialize the PDF document object.
                    using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        PdfPTable table = new PdfPTable(1);

                        Paragraph p = new Paragraph();
                        PdfPCell cell = new PdfPCell();
                        for (int i = 0; i < files.Length; i++)
                        {
                            
                            var file = files[i].Split(',')[1];
                            var bytes = Convert.FromBase64String(file);
                          

                            //Add the Image file to the PDF document object.
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bytes);
                            img.ScalePercent(28);

                            //img.ScaleAbsolute(150f, 150f);
                            img.Border = iTextSharp.text.Rectangle.BOX;
                            img.BorderColor = BaseColor.BLACK;
                            img.BorderWidth = 2f;
                            img.PaddingTop = 4f;
                            p.Add(new Chunk(img, 0, 0, true));
                          
                            p.Add(" ");
                            p.Add(" ");
                            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);


                            //iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes,9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


                            //p.Add(new Paragraph(Convert.ToString(custNo[i]), times));
                            //p.Add(new Paragraph( custName[i], times));
                        }
                        //cell.Padding = ToPoints(spacingBetweenCells / 2);

                        cell.AddElement(p);
                        
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                        pdfDoc.Close();



                        //Download the PDF file.
                        //return File(stream.ToArray(), "application/pdf", "ImageExport.pdf");


                    }
                    var bytes1 = stream.ToArray();
                    Session[fName] = bytes1;

                }
                return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                throw e;
            }

        }
        public ActionResult DownloadInvoice(string fName)
        {
            var ms = Session[fName] as byte[];
            if (ms == null)
                return new EmptyResult();
            Session[fName] = null;
            return File(ms, "application/octet-stream", fName);
        }
    }
}
