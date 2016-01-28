using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WIATest
{
  public enum ScanPageSize { Letter, Legal };

  class ImageContainer
  {
    private string fileName;
    private Image myThumbnail;
    private PictureBox myPictureBox;
    private int height;
    private int width;
    private bool tempFile;
    private bool rotate_180 = false;
    private bool landscape = false;
    private ScanPageSize size;

    public ImageContainer(string fileName)
      : this(fileName, false, ScanPageSize.Letter)
    {
      // nothing extra
    }

    public ImageContainer(string fileName, bool temporary_file, ScanPageSize size)
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
      myPictureBox = new PictureBox();
      myPictureBox.Image = myThumbnail;
      myPictureBox.Height = 180;
      myPictureBox.Width = 180;
      myPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
      myPictureBox.Tag = this;
    }

    public PictureBox getPictureBox()
    {
      return myPictureBox;
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
      myPictureBox.Refresh();
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
      
      myPictureBox.Refresh();
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
      myPictureBox.Dispose();
      myThumbnail.Dispose();
      if(this.tempFile)
      {
        File.Delete(fileName);
      }
    }
  }
}
