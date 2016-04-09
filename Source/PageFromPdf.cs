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
    private bool fSingleImageMode;
    private string fFilename;
    private int fPageIndex;


    public string SourceFilename
    {
      get { return fFilename; }
    }


    public int SourcePageIndex
    {
      get { return fPageIndex; }
    }


    public bool SingleImageMode
    {
      get { return fSingleImageMode; }
    }


    public PageFromPdf(string filename, int pageIndex, SizeInches size, Image image)
    {
      fFilename = filename;
      fPageIndex = pageIndex;

      if(image == null)
      {
        fSingleImageMode = false;
      }
      else
      {
        fSingleImageMode = true;
        // we want to save memory
        image.Dispose();
        image = null;
      }
      
      this.Size = size;

      InitializeImage(0, 0);
    }


    protected override Image CreateImage()
    {
      Image result;

      if(fSingleImageMode)
      {
        result = PdfImporter.GetSingleImageFromPdfDocument(fFilename, fPageIndex);
      }
      else
      {
        // TODO: For thumbnail purpose rendering can be made to smaller size
        result = PdfImporter.RenderPage(fFilename, fPageIndex, 300, 300);
      }

      return result;
    }
  }
}
