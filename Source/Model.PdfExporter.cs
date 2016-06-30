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
            ImportPage(pdfDocument, pageFromPdf);
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

      Image image;

      if(compressImage)
      {
        image = page.GetCompressedImage(compressionFactor);
      }
      else
      {
        image = page.GetImageInOriginalFormat(compressionFactor);
      }

      if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        LibPdfium.AddJpegToPage(pdfDocument, pdfPage, image, page.ImageBoundsInches);
      }
      else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
      {
        Bitmap bmp = (Bitmap)image;
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, bmp, page.ImageBoundsInches);
      }
      else
      {
        Bitmap bmp = (Bitmap)Utils.Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Bmp, 0);
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, bmp, page.ImageBoundsInches);
      }
    }


    public void ImportPage(IntPtr destDoc, PageFromPdf page)
    {
      string filename = page.SourceFilename;
      int pageIndex = page.SourcePageIndex;
      IntPtr sourceDoc = LibPdfium.LoadDocument(filename);
      LibPdfium.CopyPage(destDoc, sourceDoc, pageIndex + 1);
    }
  }
}
