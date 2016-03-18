using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using BitMiracle.LibTiff.Classic;
using System.Runtime.InteropServices;


namespace Model
{
  class PdfImporter
  {
    public void LoadDocument(Document document, string filename)
    {
      try
      {
        PdfDocument pdfDocument = PdfReader.Open(filename);

        long pageNumber = 0;

        // Iterate pages
        foreach(PdfPage page in pdfDocument.Pages)
        {
          Image image = PdfImporter.GetSingleImageFromPdfPage(page);
          Page myPage = new PageFromPdf(filename, pageNumber, image);
          document.AddPage(myPage);
          
          pageNumber++;
        }
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }
    }


    static public PdfPage GetSinglePdfPageFromPdfDocument(string filename, long pageNumber)
    {
      PdfDocument pdfDocument = PdfReader.Open(filename);

      return pdfDocument.Pages[(int)pageNumber];
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
                    case "/CCITTFaxDecode":
                      {
                        byte[] data = xObject.Stream.Value;

                        string fFilename = Path.GetTempFileName();

                        Tiff tiff = Tiff.Open(fFilename, "w");

                        tiff.SetField(TiffTag.IMAGEWIDTH, xObject.Elements.GetInteger("/Width"));
                        tiff.SetField(TiffTag.IMAGELENGTH, xObject.Elements.GetInteger("/Height"));
                        tiff.SetField(TiffTag.COMPRESSION, Compression.CCITTFAX4);
                        tiff.SetField(TiffTag.BITSPERSAMPLE, xObject.Elements.GetInteger("/BitsPerComponent"));
                        tiff.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                        tiff.WriteRawStrip(0, data, data.Length);
                        tiff.Close();

                        Image image = Image.FromFile(fFilename);

                        // once loaded, try and delete the temp file
                        try
                        {
                          File.Delete(fFilename);
                        }
                        catch(Exception e)
                        {
                          // do nothing
                        }

                        imagesFound++;
                        imageLargest = image;
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
