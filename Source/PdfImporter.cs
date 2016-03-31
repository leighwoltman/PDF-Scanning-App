using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using PdfProcessing;


namespace Model
{
  class PdfImporter
  {
    public void LoadDocument(Document document, string filename)
    {
      try
      {
        PdfDocument pdfDocument = PdfReader.Open(filename);

        for(int i = 0; i < pdfDocument.PageCount; i++)
        {
          PdfPage pdfPage = pdfDocument.Pages[i];
          double height = pdfPage.Height / 72; // PointsToInches
          double width = pdfPage.Width / 72; // PointsToInches
          PageSize pageSize = new PageSize(width, height);
          Image image = PdfImporter.GetSingleImageFromPdfPage(pdfPage);
          Page myPage = new PageFromPdf(filename, i, pageSize, image);
          document.AddPage(myPage);
        }
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }
    }


    static public Image RenderPage(string filename, int pageIndex, float dpiX, float dpiY)
    {
      PdfEngine engine = PdfEngine.GetInstance();

      IntPtr docPtr = engine.LoadDocument(filename);
      IntPtr pagePtr = engine.LoadPage(docPtr, pageIndex);

      double width = engine.GetPageWidth(pagePtr);
      double height = engine.GetPageHeight(pagePtr);

      int pixWidth = (int)(width * dpiX);
      int pixHeight = (int)(height * dpiY);

      Image result = engine.Render(pagePtr, pixWidth, pixHeight);

      engine.ClosePage(pagePtr);
      engine.CloseDocument(docPtr);

      return result;
    }


    static public Image GetSingleImageFromPdfDocument(string filename, int pageIndex)
    {
      PdfDocument pdfDocument = PdfReader.Open(filename);
      return GetSingleImageFromPdfPage(pdfDocument.Pages[pageIndex]);
    }
    

    static public Image GetSingleImageFromPdfPage(PdfPage page)
    {
      // determine what is on the page
      long imagesFound = 0;
      Image imageLargest = null;

      // Get resources dictionary
      PdfDictionary resources = page.Elements.GetDictionary("/Resources");
      if(resources != null)
      {
        // Get external objects dictionary
        PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
        if(xObjects != null)
        {
          ICollection<PdfItem> items = xObjects.Elements.Values;
          // Iterate references to external objects
          foreach(PdfItem item in items)
          {
            PdfReference reference = item as PdfReference;
            if(reference != null)
            {
              PdfDictionary xObject = reference.Value as PdfDictionary;
              // Is external object an image?
              if(xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
              {
                PdfArray pdfArray = null;

                try
                {
                  pdfArray = xObject.Elements.GetArray("/Filter");
                }
                catch(Exception)
                {
                  // do nothing
                }

                if(pdfArray != null && pdfArray.Elements.Count == 2)
                {
                  // the "/Filter" field was an array

                  // see if it had two values to indicate it is both JPEG and Deflate encoded
                  if((pdfArray.Elements[0].ToString() == "/DCTDecode" && pdfArray.Elements[1].ToString() == "/FlateDecode") ||
                      (pdfArray.Elements[1].ToString() == "/DCTDecode" && pdfArray.Elements[0].ToString() == "/FlateDecode"))
                  {
                    byte[] byteArray = xObject.Stream.Value;

                    FlateDecode fd = new FlateDecode();
                    byte[] byteArrayDecompressed = fd.Decode(byteArray);

                    Image image = Image.FromStream(new MemoryStream(byteArrayDecompressed));

                    imagesFound++;
                    imageLargest = image;
                  }
                }
                else
                {
                  string filter = xObject.Elements.GetString("/Filter");

                  switch(filter)
                  {
                    case "/DCTDecode":
                      {
                        // this is a directly encoded JPEG image
                        byte[] byteArray = xObject.Stream.Value;

                        Image image = Image.FromStream(new MemoryStream(byteArray));

                        imagesFound++;
                        imageLargest = image;
                      }
                      break;
                    case "/FlateDecode":
                      {
                        // potientially this is a BMP/PNG image
                        byte[] byteArray = xObject.Stream.Value;

                        FlateDecode fd = new FlateDecode();
                        byte[] byteArrayDecompressed = fd.Decode(byteArray);

                        try
                        {
                          Image image = Image.FromStream(new MemoryStream(byteArrayDecompressed));

                          imagesFound++;
                          imageLargest = image;
                        }
                        catch(Exception e)
                        {
                          // do nothing
                        }
                      }
                      break;
                  }
                }
              }
            }
          }
        }
      }

      if( imagesFound != 1 )
      {
        imageLargest = null;
      }

      return imageLargest;
    }
  }
}
