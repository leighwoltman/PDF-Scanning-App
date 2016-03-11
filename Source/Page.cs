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
    private Image fSourceThumbnail;
    private Image fThumbnail;
    private int fImageHeightPixels;
    private int fImageWidthPixels;
    private int fImageResolutionDpi;
    private int fOrientation;
    private bool fMirrored;
    private bool fLandscape;
    private PageSize fSize;


    public Page()
    {
      fSourceThumbnail = null;
      fThumbnail = null;
      fImageHeightPixels = 0;
      fImageWidthPixels = 0;
      fImageResolutionDpi = 0;
      fOrientation = 0;
      fMirrored = false;
      fLandscape = false;
      fSize = null;
    }


    protected void InitializeImage()
    {
      using(Image myImage = CreateImage())
      {
        fImageHeightPixels = myImage.Size.Height;
        fImageWidthPixels = myImage.Size.Width;
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


    public int ImageHeightPixels
    {
      get { return fImageHeightPixels; }
    }


    public int ImageWidthPixels
    {
      get { return fImageWidthPixels; }
    }


    public int ImageResolutionDpi
    {
      get { return fImageResolutionDpi; }
      protected set { fImageResolutionDpi = value; }
    }


    public bool ImageResolutionIsDefined
    {
      get { return (bool)(fImageResolutionDpi != 0); }
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


    public int Orientation
    {
      get { return fOrientation * 90; }
    }


    public void Mirror()
    {
      fMirrored = !fMirrored;
      RefreshThumbnail();
    }


    public bool IsMirrored
    {
      get { return fMirrored; }
    }


    public void Landscape()
    {
      // TODO: Flip page Height/Width?
      fLandscape = !fLandscape;
    }


    public bool IsLandscape
    {
      get { return fLandscape; }
    }


    virtual public void CleanUp()
    {
      fThumbnail.Dispose();
    }
  }
}
