using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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


    public void SaveDocument(Document document, string filename, List<int> pageNumbers, bool append, 
      bool compressImage, 
      int compressionFactor, 
      bool importNativePages,
      ResolutionDpi exportResolution)
    {
      IntPtr pdfDocument = LibPdfium.CreateNewDocument();
      
      if(append && File.Exists(filename))
      {
        IntPtr sourceDoc = LibPdfium.LoadDocument(filename);

        for(int i = 0; i < LibPdfium.GetPageCount(sourceDoc); i++)
        {
          LibPdfium.CopyPage(pdfDocument, sourceDoc, i + 1);
        }

        LibPdfium.CloseDocument(sourceDoc);
      }
      
      foreach(int num in pageNumbers)
      {
        // Get the current page from document
        Page page = document.GetPage(num);

        Image image = null;

        if(page is PageFromPdf)
        {
          PageFromPdf pageFromPdf = page as PageFromPdf;

          if(pageFromPdf.SingleImageMode)
          {
            image = pageFromPdf.GetSingleImage();
          }
          else if(importNativePages)
          {
            pageFromPdf.ExportToPdfDocument(pdfDocument);
          }
          else
          {
            image = pageFromPdf.Render(exportResolution);
          }
        }
        else
        {
          image = page.GetSourceImage();
        }

        if(image != null)
        {
          if(compressImage)
          {
            image = CompressImage(image, compressionFactor, exportResolution);
          }

          DrawPage(pdfDocument, page, image);
        }
      }

      // Save the document...
      LibPdfium.SaveDocument(pdfDocument, filename);
    }


    private Image CompressImage(Image image, int compressionFactor, ResolutionDpi maxResolution)
    {
      Image result;

      // TODO: Limit to maxResolution
      if(image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
      {
        // Jpeg format will make a Monochome image file bigger and lower the quality; Use PNG instead
        result = Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Png, 0);
      }
      else
      {
        result = Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Jpeg, compressionFactor);
      }

      return result;
    }


    private void DrawPage(IntPtr pdfDocument, Page page, Image image)
    {
      IntPtr pdfPage = LibPdfium.CreatePage(pdfDocument, page.Size);

      int transformationIndex = page.ImageTransformationIndex;

      if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        LibPdfium.AddJpegToPage(pdfDocument, pdfPage, image, page.ImageBoundsInches, transformationIndex);
      }
      else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
      {
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, image, page.ImageBoundsInches, transformationIndex);
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
      {
        // Keep PNG because:
        // -It seems PNG and BMP are both stored the same way.
        // -Converting to BMP loses the Alpha component of ARGB
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, image, page.ImageBoundsInches, transformationIndex);
      }
      else
      {
        // All other types convery to PNG because:
        // It seems PNG and BMP are both stored the same way.
        Image storedImage = Utils.Imaging.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Png, 0);
        LibPdfium.AddBitmapToPage(pdfDocument, pdfPage, storedImage, page.ImageBoundsInches, transformationIndex);
      }
    }
  }
}
