using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Drawing.BarCodes;
using CHPOUTSRCMES.Web.Models;
using PdfSharp.Pdf.IO;

namespace CHPOUTSRCMES.Web.Util
{
    public class PdfLableUtil
    {
        //直式
        public string GeneratePdfLabels(List<LabelModel> lables, ref string msg)
        {
            if (lables == null || lables.Count == 0)
            {
                msg = "沒有資料列印標籤";
                return "";
            }

            // Layout related
            XColor StrokeColor = XColors.Black;
            XColor FillColor = XColors.Black;
            XPen Pen = new XPen(StrokeColor, 0.1);
            XBrush Brush = new XSolidBrush(FillColor);
            const int lableWidth = 101;
            const int lableHeight = 152;
            string tempFileFullPath = Path.GetTempFileName();
            PdfDocument Doc = new PdfDocument();

            try
            {
                foreach (LabelModel lable in lables)
                {
                    if (Doc == null)
                    {
                        Doc = PdfReader.Open(tempFileFullPath, PdfDocumentOpenMode.Modify);
                    }

                    PdfPage page = null;
                    XGraphicsPath xPath = null;

                    page = Doc.AddPage();
                    page.Width = XUnit.FromMillimeter(lableWidth);
                    page.Height = XUnit.FromMillimeter(lableHeight);
                    using (XGraphics Gfx = XGraphics.FromPdfPage(page))
                    {
                        int y = 0;
                        xPath = new XGraphicsPath();
                        y += 3;
                        xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(60), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 5;
                        xPath.AddString("中文品名:" + lable.BarocdeName, new XFontFamily("Arial"), XFontStyle.Regular, 16, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 9;
                        xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 7;
                        xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("數量:" + lable.Qty, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        if (!string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 7;
                            xPath.AddString("工單號碼:" + lable.OspBatchNo, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        }
                        y += 7;
                        xPath.AddString("列印人:" + lable.PrintBy, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);

                        Gfx.DrawPath(Pen, Brush, xPath);

                        //主BAR CODE
                        y += 7;
                        PdfSharp.Drawing.BarCodes.Code3of9Standard BarCode39 = new PdfSharp.Drawing.BarCodes.Code3of9Standard();
                        BarCode39.TextLocation = new PdfSharp.Drawing.BarCodes.TextLocation();
                        BarCode39.Text = lable.Barocde;//value of code to draw on page
                        BarCode39.StartChar = Convert.ToChar("*");
                        BarCode39.EndChar = Convert.ToChar("*");
                        BarCode39.Direction = PdfSharp.Drawing.BarCodes.CodeDirection.LeftToRight;
                        XFont fontBARCODE = new XFont("Arial", 14, XFontStyle.Regular);
                        //XSize BARCODE_SIZE = new XSize(new XPoint(Convert.ToDouble(90), Convert.ToDouble(40)));
                        XSize BARCODE_SIZE = new XSize(XUnit.FromMillimeter(85), XUnit.FromMillimeter(12));
                        BarCode39.Size = BARCODE_SIZE;
                        Gfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)));


                        //副BAR CODE
                        if (string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 14 + 7; //補回工單號碼高度
                        }
                        else
                        {
                            y += 14;
                        }
                        int columnWidth = lableWidth / 4;
                        using (XForm form = new XForm(Doc, XUnit.FromMillimeter(columnWidth), XUnit.FromMillimeter(lableHeight - y)))
                        {
                            // Create an XGraphics object for drawing the contents of the form.
                            using (XGraphics formGfx = XGraphics.FromForm(form))
                            {
                                // Draw a large transparent rectangle to visualize the area the form occupies
                                //XColor back = XColors.Orange;
                                //back.A = 0.2;
                                //XSolidBrush brush = new XSolidBrush(back);
                                //formGfx.DrawRectangle(brush, -10000, -10000, 20000, 20000);

                                XGraphicsState state = formGfx.Save();
                                formGfx.RotateAtTransform(90, new XPoint(XUnit.FromMillimeter(0), XUnit.FromMillimeter(0)));
                                BARCODE_SIZE = new XSize(XUnit.FromMillimeter(85), XUnit.FromMillimeter(12));
                                BarCode39.Size = BARCODE_SIZE;
                                formGfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(-24)));
                                xPath = new XGraphicsPath();
                                xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(-11)), XStringFormats.TopLeft);
                                xPath.AddString("數量:" + lable.Qty + lable.Unit, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(75), XUnit.FromMillimeter(-11)), XStringFormats.TopLeft);
                                xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(-5)), XStringFormats.TopLeft);
                                xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(25), XUnit.FromMillimeter(-5)), XStringFormats.TopLeft);
                                xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(40), XUnit.FromMillimeter(-5)), XStringFormats.TopLeft);
                                formGfx.DrawPath(Pen, Brush, xPath);

                                formGfx.Restore(state);
                                Gfx.DrawImage(form, XUnit.FromMillimeter(columnWidth * 0), XUnit.FromMillimeter(y));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(columnWidth * 1), XUnit.FromMillimeter(y));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(columnWidth * 2), XUnit.FromMillimeter(y));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(columnWidth * 3), XUnit.FromMillimeter(y));
                            }
                        }
                    }
                    Doc.Save(tempFileFullPath);
                    Doc.Close();
                    Doc = null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return "";
            }
            finally
            {
                if (Doc != null)
                {
                    Doc.Close();
                    Doc = null;
                }
            }
            return tempFileFullPath;
        }

        //橫式
        public string GeneratePdfLabels2(List<LabelModel> lables, ref string msg)
        {
            if (lables == null || lables.Count == 0)
            {
                msg = "沒有資料列印標籤";
                return "";
            }

            // Layout related
            XColor StrokeColor = XColors.Black;
            XColor FillColor = XColors.Black;
            XPen Pen = new XPen(StrokeColor, 0.1);
            XBrush Brush = new XSolidBrush(FillColor);
            string tempFileFullPath = Path.GetTempFileName();
            PdfDocument Doc = new PdfDocument();

            const int lableWidth = 101;
            const int lableHeight = 152;

            try
            {                
                foreach (LabelModel lable in lables)
                {
                    if (Doc == null)
                    {
                        Doc = PdfReader.Open(tempFileFullPath, PdfDocumentOpenMode.Modify);
                    }

                    PdfPage page = null;
                    XGraphicsPath xPath = null;

                    page = Doc.AddPage();
                    page.Width = XUnit.FromMillimeter(lableWidth);
                    page.Height = XUnit.FromMillimeter(lableHeight);

                    using (XGraphics Gfx = XGraphics.FromPdfPage(page))
                    {
                        int y = 0;
                        xPath = new XGraphicsPath();
                        y += 3;
                        xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(60), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 5;
                        xPath.AddString("中文品名:" + lable.BarocdeName, new XFontFamily("Arial"), XFontStyle.Regular, 16, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 9;
                        xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 7;
                        xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("數量:" + lable.Qty, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        if (!string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 7;
                            xPath.AddString("工單號碼:" + lable.OspBatchNo, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        }
                        y += 7;
                        xPath.AddString("列印人:" + lable.PrintBy, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);

                        Gfx.DrawPath(Pen, Brush, xPath);

                        //主BAR CODE
                        y += 7;
                        PdfSharp.Drawing.BarCodes.Code3of9Standard BarCode39 = new PdfSharp.Drawing.BarCodes.Code3of9Standard();
                        //BarCode39.TextLocation = new PdfSharp.Drawing.BarCodes.TextLocation();
                        BarCode39.Text = lable.Barocde;//value of code to draw on page
                        BarCode39.StartChar = Convert.ToChar("*");
                        BarCode39.EndChar = Convert.ToChar("*");
                        BarCode39.Direction = PdfSharp.Drawing.BarCodes.CodeDirection.LeftToRight;
                        XFont fontBARCODE = new XFont("Arial", 14, XFontStyle.Regular);
                        //XSize BARCODE_SIZE = new XSize(new XPoint(Convert.ToDouble(90), Convert.ToDouble(40)));
                        XSize BARCODE_SIZE = new XSize(XUnit.FromMillimeter(90), XUnit.FromMillimeter(12));
                        BarCode39.Size = BARCODE_SIZE;
                        Gfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(y)));


                        //PdfSharp.Drawing.BarCodes.Code3of9Standard BarCode39 = new PdfSharp.Drawing.BarCodes.Code3of9Standard();
                        //BarCode39.TextLocation = new PdfSharp.Drawing.BarCodes.TextLocation();
                        //BarCode39.Text = lable.Barocde;//value of code to draw on page
                        //BarCode39.StartChar = Convert.ToChar("*");
                        //BarCode39.EndChar = Convert.ToChar("*");
                        //BarCode39.Direction = PdfSharp.Drawing.BarCodes.CodeDirection.LeftToRight;
                        //XFont fontBARCODE = new XFont("Arial", 14, XFontStyle.Regular);
                        ////XSize BARCODE_SIZE = new XSize(new XPoint(Convert.ToDouble(90), Convert.ToDouble(40)));
                        //XSize BARCODE_SIZE = new XSize(XUnit.FromMillimeter(85), XUnit.FromMillimeter(12));
                        //BarCode39.Size = BARCODE_SIZE;
                        //Gfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(y)));


                        //副BAR CODE
                        if (string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 15 + 7; //補回工單號碼高度
                        }
                        else
                        {
                            y += 15;
                        }

                        int columnHeight = (lableHeight - y) / 4;

                        using (XForm form = new XForm(Doc, XUnit.FromMillimeter(lableWidth), XUnit.FromMillimeter(columnHeight)))
                        {
                            // Create an XGraphics object for drawing the contents of the form.
                            using (XGraphics formGfx = XGraphics.FromForm(form))
                            {
                                // Draw a large transparent rectangle to visualize the area the form occupies
                                //XColor back = XColors.Orange;
                                //back.A = 0.2;
                                //XSolidBrush brush = new XSolidBrush(back);
                                //formGfx.DrawRectangle(brush, -10000, -10000, 20000, 20000);

                                XGraphicsState state = formGfx.Save();
                                //formGfx.RotateAtTransform(90, new XPoint(XUnit.FromMillimeter(0), XUnit.FromMillimeter(0)));
                                BARCODE_SIZE = new XSize(XUnit.FromMillimeter(90), XUnit.FromMillimeter(12));
                                BarCode39.Size = BARCODE_SIZE;
                                formGfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(2)));
                                xPath = new XGraphicsPath();
                                xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(15)), XStringFormats.TopLeft);
                                xPath.AddString("數量:" + lable.Qty + lable.Unit, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(80), XUnit.FromMillimeter(15)), XStringFormats.TopLeft);
                                xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(30), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(45), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                formGfx.DrawPath(Pen, Brush, xPath);
                                formGfx.Restore(state);


                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight * 2));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight * 3));             
                            }
                        }
                    }
                    Doc.Save(tempFileFullPath);
                    Doc.Close();
                    Doc = null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return "";
            }
            finally
            {
                if (Doc != null)
                {
                    Doc.Close();
                    Doc = null;
                }
            }
            return tempFileFullPath;
        }

        public ResultModel GeneratePdfLabels2(List<LabelModel> labels)
        {
            if (labels == null || labels.Count == 0)
            {
                return new ResultModel(false, "沒有資料列印標籤");
            }

            // Layout related
            XColor StrokeColor = XColors.Black;
            XColor FillColor = XColors.Black;
            XPen Pen = new XPen(StrokeColor, 0.1);
            XBrush Brush = new XSolidBrush(FillColor);
            string tempFileFullPath = Path.GetTempFileName();
            PdfDocument Doc = new PdfDocument();

            const int lableWidth = 101;
            const int lableHeight = 152;

            try
            {
                foreach (LabelModel lable in labels)
                {
                    if (Doc == null)
                    {
                        Doc = PdfReader.Open(tempFileFullPath, PdfDocumentOpenMode.Modify);
                    }

                    PdfPage page = null;
                    XGraphicsPath xPath = null;

                    page = Doc.AddPage();
                    page.Width = XUnit.FromMillimeter(lableWidth);
                    page.Height = XUnit.FromMillimeter(lableHeight);

                    using (XGraphics Gfx = XGraphics.FromPdfPage(page))
                    {
                        int y = 0;
                        xPath = new XGraphicsPath();
                        y += 0;
                        //xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(60), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        //y += 3;
                        xPath.AddString("中文品名:" + lable.BarocdeName, new XFontFamily("Arial"), XFontStyle.Regular, 16, new XPoint(XUnit.FromMillimeter(5), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 7;
                        xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        y += 5;
                        xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        xPath.AddString("數量:" + lable.Qty, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        if (!string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 5;
                            xPath.AddString("工單號碼:" + lable.OspBatchNo, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        }
                        y += 5;
                        xPath.AddString("列印人:" + lable.PrintBy, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);

                        Gfx.DrawPath(Pen, Brush, xPath);

                        //主BAR CODE
                        y += 6;
                        PdfSharp.Drawing.BarCodes.Code3of9Standard BarCode39 = new PdfSharp.Drawing.BarCodes.Code3of9Standard();
                        //BarCode39.TextLocation = new PdfSharp.Drawing.BarCodes.TextLocation();
                        BarCode39.Text = lable.Barocde;//value of code to draw on page
                        BarCode39.StartChar = Convert.ToChar("*");
                        BarCode39.EndChar = Convert.ToChar("*");
                        BarCode39.Direction = PdfSharp.Drawing.BarCodes.CodeDirection.LeftToRight;
                        XFont fontBARCODE = new XFont("Arial", 14, XFontStyle.Regular);
                        //XSize BARCODE_SIZE = new XSize(new XPoint(Convert.ToDouble(90), Convert.ToDouble(40)));
                        XSize BARCODE_SIZE = new XSize(XUnit.FromMillimeter(90), XUnit.FromMillimeter(12));
                        BarCode39.Size = BARCODE_SIZE;
                        Gfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(y)));

                        y += 13;
                        xPath = new XGraphicsPath();
                        xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(8), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        if (!string.IsNullOrEmpty(lable.LotNumber))
                        {
                            xPath.AddString("捲號:" + lable.LotNumber, new XFontFamily("Arial"), XFontStyle.Regular, 12, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(y)), XStringFormats.TopLeft);
                        }
                        Gfx.DrawPath(Pen, Brush, xPath);
                        //PdfSharp.Drawing.BarCodes.Code3of9Standard BarCode39 = new PdfSharp.Drawing.BarCodes.Code3of9Standard();
                        //BarCode39.TextLocation = new PdfSharp.Drawing.BarCodes.TextLocation();
                        //BarCode39.Text = lable.Barocde;//value of code to draw on page
                        //BarCode39.StartChar = Convert.ToChar("*");
                        //BarCode39.EndChar = Convert.ToChar("*");
                        //BarCode39.Direction = PdfSharp.Drawing.BarCodes.CodeDirection.LeftToRight;
                        //XFont fontBARCODE = new XFont("Arial", 14, XFontStyle.Regular);
                        ////XSize BARCODE_SIZE = new XSize(new XPoint(Convert.ToDouble(90), Convert.ToDouble(40)));
                        //XSize BARCODE_SIZE = new XSize(XUnit.FromMillimeter(85), XUnit.FromMillimeter(12));
                        //BarCode39.Size = BARCODE_SIZE;
                        //Gfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(y)));


                        //副BAR CODE
                        if (string.IsNullOrEmpty(lable.OspBatchNo))
                        {
                            y += 11 + 5; //補回工單號碼高度
                        }
                        else
                        {
                            y += 11;
                        }

                        int columnHeight = (lableHeight - y) / 4;

                        using (XForm form = new XForm(Doc, XUnit.FromMillimeter(lableWidth), XUnit.FromMillimeter(columnHeight)))
                        {
                            // Create an XGraphics object for drawing the contents of the form.
                            using (XGraphics formGfx = XGraphics.FromForm(form))
                            {
                                // Draw a large transparent rectangle to visualize the area the form occupies
                                //XColor back = XColors.Orange;
                                //back.A = 0.2;
                                //XSolidBrush brush = new XSolidBrush(back);
                                //formGfx.DrawRectangle(brush, -10000, -10000, 20000, 20000);

                                XGraphicsState state = formGfx.Save();
                                //formGfx.RotateAtTransform(90, new XPoint(XUnit.FromMillimeter(0), XUnit.FromMillimeter(0)));
                                BARCODE_SIZE = new XSize(XUnit.FromMillimeter(90), XUnit.FromMillimeter(12));
                                BarCode39.Size = BARCODE_SIZE;
                                formGfx.DrawBarCode(BarCode39, XBrushes.Black, fontBARCODE, new XPoint(XUnit.FromMillimeter(7.5), XUnit.FromMillimeter(2)));
                                xPath = new XGraphicsPath();
                                xPath.AddString("條碼:" + lable.Barocde, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(15)), XStringFormats.TopLeft);
                                xPath.AddString("數量:" + lable.Qty + " " + lable.Unit, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(15)), XStringFormats.TopLeft);
                                xPath.AddString("紙別:" + lable.PapaerType, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(10), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                xPath.AddString("基重:" + lable.BasicWeight, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(30), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                xPath.AddString("規格:" + lable.Specification, new XFontFamily("Arial"), XFontStyle.Regular, 8, new XPoint(XUnit.FromMillimeter(50), XUnit.FromMillimeter(18)), XStringFormats.TopLeft);
                                formGfx.DrawPath(Pen, Brush, xPath);
                                formGfx.Restore(state);


                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight * 2));
                                Gfx.DrawImage(form, XUnit.FromMillimeter(0), XUnit.FromMillimeter(y + columnHeight * 3));
                            }
                        }
                    }
                    Doc.Save(tempFileFullPath);
                    Doc.Close();
                    Doc = null;
                }
            }
            catch (Exception ex)
            {
                return new ResultModel(false, ex.Message);
            }
            finally
            {
                if (Doc != null)
                {
                    Doc.Close();
                    Doc = null;
                }
            }
            return new ResultModel(true, tempFileFullPath);
        }
    }
}