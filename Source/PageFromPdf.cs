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
using PdfProcessing;


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
      Image result;

      if(myImageFile != null)
      {
        result = myImageFile;
      }
      else
      {
        if(fRasterize)
        {
          PdfEngine engine = PdfEngine.GetInstance();

          IntPtr docPtr = engine.LoadDocument(fFilename);
          IntPtr pagePtr = engine.LoadPage(docPtr, (int)fPageNumber);

          double width = engine.GetPageWidth(pagePtr);
          double height = engine.GetPageHeight(pagePtr);

          int pixWidth = (int)(width * 300);
          int pixHeight = (int)(height * 300);

          result = engine.Render(pagePtr, pixWidth, pixHeight);

          engine.ClosePage(pagePtr);
          engine.CloseDocument(docPtr);
        }
        else
        {
          result = PdfImporter.GetSingleImageFromPdfPage(PdfImporter.GetSinglePdfPageFromPdfDocument(fFilename, fPageNumber));
        }
      }

      return result;
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
