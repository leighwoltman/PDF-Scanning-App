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
    private string fFilename;
    private int fPageIndex;
    private bool fSingleImageMode;
    private ResolutionDpi fViewingResolution;


    public bool SingleImageMode
    {
      get { return fSingleImageMode; }
    }


    public PageFromPdf(string filename, int pageIndex, SizeInches size, bool attemptSingleImageMode, ResolutionDpi viewingResolution)
    {
      fFilename = filename;
      fPageIndex = pageIndex;
      fSingleImageMode = attemptSingleImageMode;
      fViewingResolution = viewingResolution;
      Initialize(size, 0, 0);
    }


    public override Image CreateImage()
    {
      Image result = null;

      IntPtr docPtr = LibPdfium.LoadDocument(fFilename);
      IntPtr pagePtr = LibPdfium.LoadPage(docPtr, fPageIndex);

      if(fSingleImageMode)
      {
        result = LibPdfium.GetSingleImageFromPdfDocument(docPtr, pagePtr);
      }
      
      if(result == null)
      {
        // Was not single image mode, or wan not able to find a single image
        fSingleImageMode = false;

        // TODO: For thumbnail purpose rendering can be made to smaller size
        int pixWidth = (int)(Size.Width * fViewingResolution.Horizontal); // width * dpiX
        int pixHeight = (int)(Size.Height * fViewingResolution.Vertical); // height * dpiY

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


    public void ExportToPdfDocument(IntPtr destDoc)
    {
      IntPtr sourceDoc = LibPdfium.LoadDocument(fFilename);
      LibPdfium.CopyPage(destDoc, sourceDoc, fPageIndex + 1);
      LibPdfium.CloseDocument(sourceDoc);
    }
  }
}
