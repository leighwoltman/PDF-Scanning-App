using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;


namespace PDFScanningApp
{
  public enum ScanPageSize { Letter, Legal };

  public class Page
  {
    private string fileName;
    private Image myThumbnail;
    private int height;
    private int width;
    private bool tempFile;
    private bool rotate_180 = false;
    private bool landscape = false;
    private ScanPageSize size;

    public Page(string fileName)
      : this(fileName, false, ScanPageSize.Letter)
    {
      // nothing extra
    }

    public Page(string fileName, bool temporary_file, ScanPageSize size)
    {
      this.tempFile = temporary_file;
      this.fileName = fileName;
      this.size = size;
      // create a thumbnail
      int thumbnail_height;
      int thumbnail_width;
      using (Bitmap myBitmap = new Bitmap(fileName))
      {
        this.height = myBitmap.Size.Height;
        this.width = myBitmap.Size.Width;

        if( myBitmap.Size.Height > myBitmap.Size.Width )
        {
          thumbnail_height = 200;
          thumbnail_width = (int)(((double)myBitmap.Size.Width / (double)myBitmap.Size.Height) * thumbnail_height);
        }
        else
        {
          thumbnail_width = 200;
          thumbnail_height = (int)(((double)myBitmap.Size.Height / (double)myBitmap.Size.Width) * thumbnail_width);
        }
        Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
        myThumbnail = myBitmap.GetThumbnailImage(thumbnail_width, thumbnail_height, myCallback, IntPtr.Zero);
      }
    }

    public Image Thumbnail
    {
      get
      {
        return myThumbnail;
      }
    }

    public ScanPageSize getScanPageSize()
    {
      return size;
    }

    public int getHeight()
    {
      return height;
    }

    public int getWidth()
    {
      return width;
    }

    public string getFileName()
    {
      return fileName;
    }

    public bool ThumbnailCallback()
    {
      return false;
    }

    public void rotate()
    {
      rotate_180 = !rotate_180;
      myThumbnail.RotateFlip(RotateFlipType.Rotate180FlipNone);
    }

    public void makeLandscape()
    {
      if(this.landscape)
      {
        myThumbnail.RotateFlip(RotateFlipType.Rotate270FlipNone);
      }
      else
      {
        myThumbnail.RotateFlip(RotateFlipType.Rotate90FlipNone);
      }
      this.landscape = !this.landscape;
    }

    public bool isLandscape()
    {
      return this.landscape;
    }

    public bool isRotated()
    {
      return rotate_180;
    }

    public void cleanUp()
    {
      myThumbnail.Dispose();
      if(this.tempFile)
      {
        File.Delete(fileName);
      }
    }

    public void ToPdf(PdfPage pdfPage)
    {
      if(this.getScanPageSize() == ScanPageSize.Legal)
      {
        pdfPage.Size = PageSize.Legal;
      }
      else
      {
        pdfPage.Size = PageSize.Letter;
      }

      // we need to swap the height and the width if this is a landscape image
      double aspect_ratio = ((double)this.getWidth()) / ((double)this.getHeight());

      if(this.isLandscape())
      {
        pdfPage.Orientation = PageOrientation.Landscape;
      }
      else
      {
        pdfPage.Orientation = PageOrientation.Portrait;
      }

      // Get an XGraphics object for drawing
      XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
      // Create a font
      XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

      int draw_point_x = 0;
      int draw_point_y = 0;
      int draw_point_width = 0;
      int draw_point_height = 0;

      if(this.isLandscape())
      {
        // these are swapped
        draw_point_width = (int)pdfPage.Height;
        draw_point_height = (int)pdfPage.Width;

        if(aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
        {
          // means our image has the width as the maximum dimension
          draw_point_height = (int)((double)draw_point_width / aspect_ratio);
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
        }
        else
        {
          // means our image has the height as the maximum dimension
          draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
        }
      }
      else
      {
        draw_point_width = (int)pdfPage.Width;
        draw_point_height = (int)pdfPage.Height;

        if(aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
        {
          // means our image has the width as the maximum dimension
          draw_point_height = (int)((double)draw_point_width / aspect_ratio);
          draw_point_y = ((int)pdfPage.Height - draw_point_height) / 2;
        }
        else
        {
          // means our image has the height as the maximum dimension
          draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
          draw_point_x = ((int)pdfPage.Width - draw_point_width) / 2;
        }
      }


      XImage image = XImage.FromFile(this.getFileName());

      if(this.isRotated())
      {
        // rotate around the center of the pdfPage
        gfx.RotateAtTransform(-180, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      }

      if(this.isLandscape())
      {
        // rotate around the center of the pdfPage
        gfx.RotateAtTransform(90, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      }

      gfx.DrawImage(image, draw_point_x, draw_point_y, draw_point_width, draw_point_height);
      image.Dispose();
    }
  }
}
