using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfProcessing;
using Defines;
using Utils;


namespace Model
{
  public class PageFromPdf : Page
  {
    private bool fSingleImageMode;
    private string fFilename;
    private int fPageIndex;
    private ResolutionDpi fViewingResolution;


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


    public PageFromPdf(string filename, int pageIndex, SizeInches size, Image image, ResolutionDpi viewingResolution)
    {
      fFilename = filename;
      fPageIndex = pageIndex;
      fViewingResolution = viewingResolution;

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
      
      Initialize(size, 0, 0);
    }


    public override Image CreateImage()
    {
      Image result;

      IntPtr docPtr = LibPdfium.LoadDocument(fFilename);
      IntPtr pagePtr = LibPdfium.LoadPage(docPtr, fPageIndex);

      if(fSingleImageMode)
      {
        result = LibPdfium.GetSingleImageFromPdfDocument(docPtr, pagePtr);
      }
      else
      {
        // TODO: For thumbnail purpose rendering can be made to smaller size
        double width = LibPdfium.GetPageWidth(pagePtr);
        double height = LibPdfium.GetPageHeight(pagePtr);

        int pixWidth = (int)(width * fViewingResolution.Horizontal); // width * dpiX
        int pixHeight = (int)(height * fViewingResolution.Vertical); // height * dpiY

        result = LibPdfium.Render(pagePtr, pixWidth, pixHeight);
      }

      LibPdfium.ClosePage(pagePtr);
      LibPdfium.CloseDocument(docPtr);

      return result;
    }


    public override bool CanModify()
    {
      // can only modify if we are in image mode
      return fSingleImageMode;
    }
  }
}
