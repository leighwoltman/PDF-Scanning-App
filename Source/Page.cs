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
    protected Image fSourceThumbnail;
    protected Image fThumbnail;
    protected int fHeightPixels;
    protected int fWidthPixels;
    protected int fOrientation;
    protected bool fMirrored;
    protected bool fLandscape;
    private PageSize fSize;


    public Page()
    {
      fOrientation = 0;
      fMirrored = false;
      fLandscape = false;
    }


    protected void InitializeImage()
    {
      using(Image myImage = CreateImage())
      {
        fHeightPixels = myImage.Size.Height;
        fWidthPixels = myImage.Size.Width;
        fSourceThumbnail = UtilImaging.CreateThumbnail(myImage, 200);
      }
      RefreshThumbnail();
    }


    private void RefreshThumbnail()
    {
      fThumbnail = (Image)fSourceThumbnail.Clone();
      TransformImage(fThumbnail);
    }


    public Image Thumbnail
    {
      get
      {
        return fThumbnail;
      }
    }


    public PageSize Size
    {
      get { return fSize; }
      protected set { fSize = value; }
    }


    public int GetHeight()
    {
      return fHeightPixels;
    }


    public int GetWidth()
    {
      return fWidthPixels;
    }


    readonly RotateFlipType[] rf_table =
    { 
      RotateFlipType.RotateNoneFlipNone,
      RotateFlipType.Rotate90FlipNone,
      RotateFlipType.Rotate180FlipNone,
      RotateFlipType.Rotate270FlipNone,
      RotateFlipType.RotateNoneFlipX,
      RotateFlipType.Rotate90FlipX,
      RotateFlipType.Rotate180FlipX,
      RotateFlipType.Rotate270FlipX
    };


    protected void TransformImage(Image image)
    {
      int index = fOrientation + (fMirrored ? 4 : 0);
      image.RotateFlip(rf_table[index]);
    }


    abstract protected Image CreateImage();


    public Image GetImage()
    {
      Image result = CreateImage();
      TransformImage(result);
      return result;
    }


    public void Rotate()
    {
      fOrientation = (fOrientation + 1) % 4;
      RefreshThumbnail();
    }


    public int GetOrientation()
    {
      return fOrientation * 90;
    }


    public void Mirror()
    {
      fMirrored = !fMirrored;
      RefreshThumbnail();
    }


    public bool IsMirrored()
    {
      return fMirrored;
    }


    public void Landscape()
    {
      //if(this.fLandscape)
      //{
      //  fThumbnail.RotateFlip(RotateFlipType.Rotate270FlipNone);
      //}
      //else
      //{
      //  fThumbnail.RotateFlip(RotateFlipType.Rotate90FlipNone);
      //}
      fLandscape = !fLandscape;
    }


    public bool IsLandscape()
    {
      return fLandscape;
    }


    virtual public void CleanUp()
    {
      fThumbnail.Dispose();
    }
  }
}
