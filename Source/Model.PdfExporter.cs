using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using PdfProcessing;
using Imports;
using Utils;


namespace Model
{
  class PdfExporter
  {
    public PdfExporter()
    {
      LibPdfium.Initialize();
    }


    public void SaveDocument(Document document, string filename, List<int> pageNumbers, bool compressImage, int compressionFactor)
    {
      IntPtr pdfDocument = LibPdfium.CreateNewDocument();

      foreach(int num in pageNumbers)
      {
        // Get the current page from document
        Page page = document.GetPage(num);

        if(page is PageFromPdf)
        {
          PageFromPdf pageFromPdf = page as PageFromPdf;

          if(pageFromPdf.SingleImageMode)
          {
            DrawPage(pdfDocument, pageFromPdf, compressImage, compressionFactor);
          }
          else
          {
            pageFromPdf.ExportToPdfDocument(pdfDocument);
          }
        }
        else
        {
          DrawPage(pdfDocument, page, compressImage, compressionFactor);
        }
      }

      // Save the document...
      LibPdfium.SaveDocument(pdfDocument, filename);
    }


    private void DrawPage(IntPtr pdfDocument, Page page, bool compressImage, int compressionFactor)
    {
      IntPtr pdfPage = LibPdfium.CreatePage(pdfDocument, page.Size);

      Image image = page.GetSourceImage();

      int transformationIndex = page.ImageTransformationIndex;

      if(compressImage)
      {
        if(image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
        {
          // Jpeg format will make a Monochome image file bigger and lower the quality; Use PNG instead
          image = Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Png, 0);
        }
        else
        {
          image = Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Jpeg, compressionFactor);
        }
      }

      if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        LibPdfium.AddJpegToPage(pdfDocument, pdfPage, image, page.ImageBoundsInches, transformationIndex);
      }
      else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
      {
        Bitmap bmp = (Bitmap)image;
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, bmp, page.ImageBoundsInches, transformationIndex);
      }
      else
      {
        Bitmap bmp = (Bitmap)Utils.Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Bmp, 0);
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, bmp, page.ImageBoundsInches, transformationIndex);
      }
    }
  }
}
