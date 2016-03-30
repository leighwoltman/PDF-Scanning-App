using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using PdfiumViewer;


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
      NativeMethods.FPDF_AddRef();
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

      SafeHandle mappedHandle = NativeMethods.CreateFileMapping(handle, IntPtr.Zero, NativeMethods.FileMapProtection.PageReadonly, 0, (uint)length, null);

      if(mappedHandle.IsInvalid)
      {
        throw new Exception();
      }

      SafeHandle buffer = NativeMethods.MapViewOfFile(mappedHandle, NativeMethods.FileMapAccess.FileMapRead, 0, 0, (uint)length);

      if(buffer.IsInvalid)
      {
        throw new Exception();
      }

      return NativeMethods.FPDF_LoadMemDocument(buffer, length, null);
    }


    public void CloseDocument(IntPtr document)
    {
      NativeMethods.FPDF_CloseDocument(document);
    }


    public IntPtr LoadPage(IntPtr document, int pageNumber)
    {
      return NativeMethods.FPDF_LoadPage(document, pageNumber);
    }


    public void ClosePage(IntPtr page)
    {
      NativeMethods.FPDF_ClosePage(page);
    }


    public int GetPageCount(IntPtr document)
    {
      return NativeMethods.FPDF_GetPageCount(document);
    }


    public bool CopyPage(IntPtr destDoc, IntPtr sourceDoc, int pageNumber)
    {
      return NativeMethods.FPDF_ImportPages(destDoc, sourceDoc, pageNumber.ToString(), GetPageCount(destDoc));
    }


    public IntPtr CreateNewDocument()
    {
      return NativeMethods.FPDF_CreateNewDocument();
    }


    public bool SaveDocument(IntPtr document, string filename)
    {
      return NativeMethods.FPDF_SaveAsFile(document, filename, 0, null, 0, null, 0);
    }


    public double GetPageWidth(IntPtr page)
    {
      // Convert points to inches
      return NativeMethods.FPDF_GetPageWidth(page) / 72;
    }


    public double GetPageHeight(IntPtr page)
    {
      // Convert points to inches
      return NativeMethods.FPDF_GetPageHeight(page) / 72;
    }


    public Bitmap Render(IntPtr page, int pixWidth, int pixHeight)
    {
      Bitmap bitmap = new Bitmap(pixWidth, pixHeight, PixelFormat.Format32bppArgb);
      //bitmap.SetResolution(dpiX, dpiY);

      BitmapData data = bitmap.LockBits(new Rectangle(0, 0, pixWidth, pixHeight), ImageLockMode.ReadWrite, bitmap.PixelFormat);

      try
      {
        IntPtr handle = NativeMethods.FPDFBitmap_CreateEx(pixWidth, pixHeight, 4, data.Scan0, pixWidth * 4);

        try
        {
          NativeMethods.FPDF_RenderPageBitmap(handle, page, 0, 0, pixWidth, pixHeight, 0, 0);
        }
        finally
        {
          NativeMethods.FPDFBitmap_Destroy(handle);
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
