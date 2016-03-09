using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Utils;


namespace Model
{
  class PdfExporter
  {
    public void SaveDocument(Document document, string filename)
    {
      PdfDocument pdfDocument = new PdfDocument();
      pdfDocument.Info.Title = "Created with PDFsharp";

      for(int i = 0; i < document.NumPages; i++)
      {
        // Create an empty page
        PdfPage pdfPage = pdfDocument.AddPage();
        // Get the current page from document
        Page page = document.GetPage(i);
        // Print page to pdf
        PageToPdfPage(page, pdfPage);
      }

      // Save the document...
      pdfDocument.Save(filename);
    }


    public void PageToPdfPage(Page page, PdfPage pdfPage)
    {
      if(page.GetScanPageSize() == PageSize.Legal)
      {
        pdfPage.Size = PdfSharp.PageSize.Legal;
      }
      else
      {
        pdfPage.Size = PdfSharp.PageSize.Letter;
      }

      // we need to swap the height and the width if page is a landscape image
      double aspect_ratio = ((double)page.GetWidth()) / ((double)page.GetHeight());

      if(page.IsLandscape())
      {
        pdfPage.Orientation = PageOrientation.Landscape;
      }
      else
      {
        pdfPage.Orientation = PageOrientation.Portrait;
      }

      // Get an XGraphics object for drawing
      XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
      // Create a font
      XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

      int draw_point_x = 0;
      int draw_point_y = 0;
      int draw_point_width = 0;
      int draw_point_height = 0;

      if(page.IsLandscape())
      {
        // these are swapped
        draw_point_width = (int)pdfPage.Height;
        draw_point_height = (int)pdfPage.Width;

        if(aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
        {
          // means our image has the width as the maximum dimension
          draw_point_height = (int)((double)draw_point_width / aspect_ratio);
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
        }
        else
        {
          // means our image has the height as the maximum dimension
          draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
        }
      }
      else
      {
        draw_point_width = (int)pdfPage.Width;
        draw_point_height = (int)pdfPage.Height;

        if(aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
        {
          // means our image has the width as the maximum dimension
          draw_point_height = (int)((double)draw_point_width / aspect_ratio);
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
        }
        else
        {
          // means our image has the height as the maximum dimension
          draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
        }
      }


      XImage image = XImage.FromGdiPlusImage(page.GetImage());

      //if(page.IsRotated())
      //{
      //  // rotate around the center of the pdfPage
      //  gfx.RotateAtTransform(-180, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      //}

      //if(page.IsLandscape())
      //{
      //  // rotate around the center of the pdfPage
      //  gfx.RotateAtTransform(90, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      //}

      gfx.DrawImage(image, draw_point_x, draw_point_y, draw_point_width, draw_point_height);
      image.Dispose();
    }
  }
}
