using System;
using System.Collections.Generic;
using System.Drawing;
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
        // Get the current page from document
        Page page = document.GetPage(i);

        if(page is PageFromPdf)
        {
          PageFromPdf pageFromPdf = page as PageFromPdf;

          if(pageFromPdf.SingleImageMode)
          {
            DrawPage(pdfDocument, pageFromPdf);
          }
          else
          {
            ImportPage(pdfDocument, pageFromPdf);
          }
        }
        else
        {
          DrawPage(pdfDocument, page);
        }
      }

      // Save the document...
      pdfDocument.Save(filename);
    }


    private void DrawPage(PdfDocument pdfDocument, Page page)
    {
      // Create an empty page
      PdfPage pdfPage = pdfDocument.AddPage();

      pdfPage.Width = page.Size.Width * 72; // Inches to Point
      pdfPage.Height = page.Size.Height * 72; // Inches to Point

      // Get an XGraphics object for drawing
      XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

      Rectangle imageRect = new Rectangle();
      BoundsInches imageBounds = page.ImageBoundsInches;

      // Convert the image boundaries to points
      imageRect.X = (int)(imageBounds.X * 72);
      imageRect.Y = (int)(imageBounds.Y * 72);
      imageRect.Width = (int)(imageBounds.Width * 72);
      imageRect.Height = (int)(imageBounds.Height * 72);

      Image image = page.GetImage();
      XImage ximage = XImage.FromGdiPlusImage(image);
      gfx.DrawImage(ximage, imageRect);
      image.Dispose();
    }


    public void ImportPage(PdfDocument pdfDocument, PageFromPdf page)
    {
      string filename = page.SourceFilename;
      int pageIndex = page.SourcePageIndex;

      PdfSharp.Pdf.PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(filename, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
      pdfDocument.AddPage(inputDocument.Pages[pageIndex]);
    }
  }
}
