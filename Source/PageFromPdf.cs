using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utils;


namespace Model
{
  public class PageFromPdf : Page
  {
    private Image myImageFile;
    private bool fRasterize;
    private string fFilename;
    private long fPageNumber;

    public PageFromPdf(string filename, long pageNumber, Image image)
    {
      fFilename = filename;
      fPageNumber = pageNumber;

      if(image == null)
      {
        fRasterize = true;
        this.myImageFile = CreateImage();
      }
      else
      {
        fRasterize = false;
        this.myImageFile = image;
      }
      
      this.Size = PageSize.Letter;

      InitializeImage(0, 0);

      // we want to save memory
      this.myImageFile.Dispose();
      this.myImageFile = null;
    }


    protected override Image CreateImage()
    {
      if(myImageFile != null)
      {
        return myImageFile;
      }
      else
      {
        if(fRasterize)
        {
          Image image = null;

          try
          {
            using (var rasterizer = new GhostscriptRasterizer())
            {
              rasterizer.Open(fFilename);
              // Ghostscript uses 1 based page indexing
              image = rasterizer.GetPage(300, 300, (int)fPageNumber + 1);
            }
          }
          catch (Exception e)
          {
            // we need to load it from the file
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("PDF_Scanner_App_WPF.PdfNotAvailableBanner.png");
            image = Image.FromStream(myStream);
          }

          return image;
        }
        else
        {
          return PdfImporter.GetSingleImageFromPdfPage(PdfImporter.GetSinglePdfPageFromPdfDocument(fFilename, fPageNumber));
        }
      }
    }


    public override void AddPdfPage(PdfDocument pdfDocument)
    {
      if (fRasterize)
      {
        PdfDocument inputDocument = PdfReader.Open(fFilename, PdfDocumentOpenMode.Import);

        // otherwise we just want to add the document from the original document
        pdfDocument.AddPage(inputDocument.Pages[(int)fPageNumber]);
      }
      else
      {
        base.AddPdfPage(pdfDocument);
      }
    }

    public override void CleanUp()
    {
      if (this.myImageFile != null)
      {
        this.myImageFile.Dispose();
        this.myImageFile = null;
      }

      base.CleanUp();
    }
  }
}
