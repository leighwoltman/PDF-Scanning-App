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
    private int fImageVerticalResolutionDpi;
    private int fImageHorizontalResolutionDpi;
    private int fOrientation;
    private bool fMirrored;
    private PageSize fSize;


    public Page()
    {
      fSourceThumbnail = null;
      fThumbnail = null;
      fImageHeightPixels = 0;
      fImageWidthPixels = 0;
      fImageVerticalResolutionDpi = 0;
      fImageHorizontalResolutionDpi = 0;
      fOrientation = 0;
      fMirrored = false;
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


    abstract protected Image CreateImage();


    public Image GetImage()
    {
      Image result = CreateImage();
      TransformImage(result);
      return result;
    }


    public Image Thumbnail
    {
      get { return fThumbnail; }
    }


    private void RefreshThumbnail()
    {
      fThumbnail = (Image)fSourceThumbnail.Clone();
      TransformImage(fThumbnail);
    }


    virtual public void CleanUp()
    {
      fThumbnail.Dispose();
    }


    readonly RotateFlipType[] rf_table =
    { 
      RotateFlipType.RotateNoneFlipNone,
      RotateFlipType.Rotate90FlipNone,
      RotateFlipType.Rotate180FlipNone,
      RotateFlipType.Rotate270FlipNone,
      RotateFlipType.Rotate270FlipX,
      RotateFlipType.Rotate180FlipX,
      RotateFlipType.Rotate90FlipX,
      RotateFlipType.RotateNoneFlipX,
    };


    protected void TransformImage(Image image)
    {
      int index = fOrientation + (fMirrored ? 4 : 0);
      image.RotateFlip(rf_table[index]);
    }


    // TODO: Make the following properties follow the orientation
    public int ImageHeightPixels
    {
      get { return fImageHeightPixels; }
      protected set { fImageHeightPixels = value; }
    }


    public int ImageWidthPixels
    {
      get { return fImageWidthPixels; }
      protected set { fImageWidthPixels = value; }
    }


    public int ImageVerticalResolutionDpi
    {
      get { return fImageVerticalResolutionDpi; }
      protected set { fImageVerticalResolutionDpi = value; }
    }


    public int ImageHorizontalResolutionDpi
    {
      get { return fImageHorizontalResolutionDpi; }
      protected set { fImageHorizontalResolutionDpi = value; }
    }


    public bool ImageResolutionIsDefined
    {
      get { return (bool)((fImageVerticalResolutionDpi != 0) && (fImageHorizontalResolutionDpi != 0)); }
    }


    public PageSize ImageSize
    {
      get 
      {
        PageSize result = null;

        if(ImageResolutionIsDefined)
        {
          double width = ImageWidthPixels / (double)ImageHorizontalResolutionDpi;
          double height = ImageHeightPixels / (double)ImageVerticalResolutionDpi;
          result = new PageSize(width, height);
        }

        return result; 
      }
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


    public PageSize Size
    {
      get { return fSize; }
      protected set { fSize = value; }
    }


    public void Landscape()
    {
      if(fSize != null)
      {
        double temp = fSize.Width;
        fSize.Width = fSize.Height;
        fSize.Height = temp;
      }
    }


    public bool IsLandscape
    {
      get 
      { 
        return (fSize != null) && (fSize.Width > fSize.Height); 
      }
    }
  }
}
