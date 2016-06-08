using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Imports;


namespace PdfProcessing
{
  class PdfEngine
  {
    static PdfEngine fInstance = null;


    static public PdfEngine GetInstance()
    {
      if(fInstance == null)
      {
        fInstance = new PdfEngine();
      }

      return fInstance;
    }


    private PdfEngine()
    {
      LibPdfium.FPDF_AddRef();
    }


    public void Close()
    {
    }


    public IntPtr LoadDocument(string path)
    {
      FileStream stream = File.OpenRead(path);

      SafeHandle handle = stream.SafeFileHandle;

      if(handle == null)
      {
        throw new ArgumentNullException("handle");
      }

      int length = (int)stream.Length;

      SafeHandle mappedHandle = LibKernel32.CreateFileMapping(handle, IntPtr.Zero, LibKernel32.FileMapProtection.PageReadonly, 0, (uint)length, null);

      if(mappedHandle.IsInvalid)
      {
        throw new Exception();
      }

      SafeHandle buffer = LibKernel32.MapViewOfFile(mappedHandle, LibKernel32.FileMapAccess.FileMapRead, 0, 0, (uint)length);

      if(buffer.IsInvalid)
      {
        throw new Exception();
      }

      return LibPdfium.FPDF_LoadMemDocument(buffer, length, null);
    }


    public void CloseDocument(IntPtr document)
    {
      LibPdfium.FPDF_CloseDocument(document);
    }


    public IntPtr LoadPage(IntPtr document, int pageNumber)
    {
      return LibPdfium.FPDF_LoadPage(document, pageNumber);
    }


    public void ClosePage(IntPtr page)
    {
      LibPdfium.FPDF_ClosePage(page);
    }


    public int GetPageCount(IntPtr document)
    {
      return LibPdfium.FPDF_GetPageCount(document);
    }


    public bool CopyPage(IntPtr destDoc, IntPtr sourceDoc, int pageNumber)
    {
      return LibPdfium.FPDF_ImportPages(destDoc, sourceDoc, pageNumber.ToString(), GetPageCount(destDoc));
    }


    public IntPtr CreateNewDocument()
    {
      return LibPdfium.FPDF_CreateNewDocument();
    }


    public bool SaveDocument(IntPtr document, string filename)
    {
      return LibPdfium.FPDF_SaveAsFile(document, filename, 0, null, 0, null, 0);
    }


    public double GetPageWidth(IntPtr page)
    {
      // Convert points to inches
      return LibPdfium.FPDF_GetPageWidth(page) / 72;
    }


    public double GetPageHeight(IntPtr page)
    {
      // Convert points to inches
      return LibPdfium.FPDF_GetPageHeight(page) / 72;
    }


    public Bitmap Render(IntPtr page, int pixWidth, int pixHeight)
    {
      Bitmap bitmap = new Bitmap(pixWidth, pixHeight, PixelFormat.Format32bppArgb);
      //bitmap.SetResolution(dpiX, dpiY);

      BitmapData data = bitmap.LockBits(new Rectangle(0, 0, pixWidth, pixHeight), ImageLockMode.ReadWrite, bitmap.PixelFormat);

      try
      {
        IntPtr handle = LibPdfium.FPDFBitmap_CreateEx(pixWidth, pixHeight, 4, data.Scan0, pixWidth * 4);

        try
        {
          LibPdfium.FPDF_RenderPageBitmap(handle, page, 0, 0, pixWidth, pixHeight, 0, 0);
        }
        finally
        {
          LibPdfium.FPDFBitmap_Destroy(handle);
        }
      }
      finally
      {
        bitmap.UnlockBits(data);
      }

      return bitmap;
    }
  }
}
