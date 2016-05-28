﻿using System;
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

      if(fSingleImageMode)
      {
        result = PdfImporter.GetSingleImageFromPdfDocument(fFilename, fPageIndex);
      }
      else
      {
        // TODO: For thumbnail purpose rendering can be made to smaller size
        result = PdfImporter.RenderPage(fFilename, fPageIndex, (float)fViewingResolution.Horizontal, (float)fViewingResolution.Vertical);
      }

      return result;
    }


    public override bool CanModify()
    {
      // can only modify if we are in image mode
      return fSingleImageMode;
    }
  }
}
