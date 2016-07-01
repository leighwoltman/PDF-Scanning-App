using System;
using System.Collections.Generic;
using System.Drawing;
using PdfProcessing;
using Utils;


namespace Model
{
  class PdfImporter
  {
    public PdfImporter()
    {
      LibPdfium.Initialize();
    }


    public void LoadPagesFromFiles(Document document, string[] filenames, bool attemptPdfSingleImageImport, ResolutionDpi viewingResolution)
    {
      foreach(string filename in filenames)
      {
        LoadDocument(document, filename, attemptPdfSingleImageImport, viewingResolution);
      }
    }


    public void LoadDocument(Document document, string filename, bool attemptPdfSingleImageImport, ResolutionDpi viewingResolution)
    {
      try
      {
        IntPtr docPtr = LibPdfium.LoadDocument(filename);

        for(int i = 0; i < LibPdfium.GetPageCount(docPtr); i++)
        {
          IntPtr pagePtr = LibPdfium.LoadPage(docPtr, i);

          double height = LibPdfium.GetPageHeight(pagePtr);
          double width = LibPdfium.GetPageWidth(pagePtr);
          SizeInches pageSize = new SizeInches(width, height);

          Page myPage = new PageFromPdf(filename, i, pageSize, attemptPdfSingleImageImport, viewingResolution);
          document.AddPage(myPage);

          LibPdfium.ClosePage(pagePtr);
        }

        LibPdfium.CloseDocument(docPtr);
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }
    }
  }
}
