using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseUtils;


namespace PdfProcessing
{
  public class PdfDocument
  {
    public IntPtr Ptr { get; private set; }

    public string Filename { get; private set; }

    private int fAccessCounter = 0;


    public PdfDocument(string filename = null)
    {
      Filename = filename;
      Ptr = IntPtr.Zero;
    }


    public bool IsOpen
    {
      get { return Ptr != IntPtr.Zero; }
    }


    public bool Open()
    {
      if (fAccessCounter == 0)
      {
        if (string.IsNullOrEmpty(this.Filename))
        {
          Ptr = LibPdfium.CreateNewDocument();
        }
        else
        {
          Ptr = LibPdfium.LoadDocument(this.Filename);
        }
      }
      fAccessCounter++;
      return IsOpen;
    }


    public void Close()
    {
      if (fAccessCounter > 0)
      {
        fAccessCounter--;

        if (fAccessCounter == 0)
        {
          LibPdfium.CloseDocument(Ptr);
          Ptr = IntPtr.Zero;
        }
      }
    }


    public bool Save(string filename)
    {
      this.Filename = filename;
      return IsOpen ? LibPdfium.SaveDocument(Ptr, this.Filename) : false;
    }


    public int PageCount
    {
      get { return IsOpen ? LibPdfium.GetPageCount(Ptr) : 0; }
    }


    public PdfPage GetPage(int index)
    {
      PdfPage result = null;

      if (IsOpen)
      {
        result = new PdfPage(this, index);
      }

      return result;
    }


    public bool CopyNativePage(PdfPage page)
    {
      bool result = false;

      if (this.IsOpen)
      {
        if (page.Open())
        {
          result = LibPdfium.CopyPage(this.Ptr, page.Document.Ptr, page.Index + 1);
          page.Close();
        }
      }

      return result;
    }
  }
}
