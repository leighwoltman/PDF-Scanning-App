using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utils;


namespace Model
{
  abstract public class Page
  {
    protected Image myThumbnail;
    protected int heightPixels;
    protected int widthPixels;
    protected bool rotate_180 = false;
    protected bool landscape = false;
    protected PageSize size;


    protected void CreateThumbnail(Image myBitmap)
    {
      this.heightPixels = myBitmap.Size.Height;
      this.widthPixels = myBitmap.Size.Width;
      myThumbnail = UtilImaging.CreateThumbnail(myBitmap, 200);
    }
    

    public Image Thumbnail
    {
      get
      {
        return myThumbnail;
      }
    }


    public PageSize getScanPageSize()
    {
      return size;
    }


    public int getHeight()
    {
      return heightPixels;
    }


    public int getWidth()
    {
      return widthPixels;
    }


    abstract public Image getImage();


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

    abstract public void cleanUp();

    protected void pageCleanUp()
    {
      myThumbnail.Dispose();
    }
  }
}
