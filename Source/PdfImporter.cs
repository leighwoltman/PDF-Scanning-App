using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;


namespace Model
{
  class PdfImporter
  {
    public void LoadDocument(Document document, string filename)
    {
      try
      {
        PdfDocument pdfDocument = PdfReader.Open(filename);

        // Iterate pages
        foreach(PdfPage page in pdfDocument.Pages)
        {
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

                        Page myPage = new Page(image);
                        document.AddPage(myPage);
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

                            Page myPage = new Page(image);
                            document.AddPage(myPage);
                          }
                          break;
                        case "/FlateDecode":
                          {
                            // potientially this is a BMP/PNG image
                          }
                          break;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }
    }
  }
}
