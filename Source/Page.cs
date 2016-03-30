using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
    private PageSize fSizeInch;
    private PageSize fImageSizeInch;
    private Rectangle fBoundsPixel;
    private Rectangle fImageBoundsPixel;
    private double fResolutionDpiX;
    private double fResolutionDpiY;


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
      fSizeInch = null;
      fImageSizeInch = null;
      fBoundsPixel = new Rectangle();
      fImageBoundsPixel = new Rectangle();
      fResolutionDpiX = 0;
      fResolutionDpiY = 0;
    }


    protected void InitializeImage(int verticalDpi, int horizontalDpi)
    {
      using(Image myImage = CreateImage())
      {
        fImageHeightPixels = myImage.Size.Height;
        fImageWidthPixels = myImage.Size.Width;
        fImageVerticalResolutionDpi = verticalDpi;
        fImageHorizontalResolutionDpi = horizontalDpi;
        fSourceThumbnail = UtilImaging.CreateThumbnail(myImage, 200);
      }
      RefreshImage();
    }


    private void RefreshImage()
    {
      CalculateBounds();
      fThumbnail = (Image)fSourceThumbnail.Clone();
      TransformImage(fThumbnail);
    }


    private void CalculateBounds()
    {
      double pageWidthInch = this.Size.Width;
      double pageHeightInch = this.Size.Height;
      double imageWidthInch;
      double imageHeightInch;
      int pageWidthPixels;
      int pageHeightPixels;
      int imageWidthPixels = this.ImageWidthPixels;
      int imageHeightPixels = this.ImageHeightPixels;

      if(ImageResolutionIsDefined)
      {
        fResolutionDpiX = ImageHorizontalResolutionDpi;
        fResolutionDpiY = ImageVerticalResolutionDpi;
        imageWidthInch = imageWidthPixels / fResolutionDpiX;
        imageHeightInch = imageHeightPixels / fResolutionDpiY;
      }
      else
      {
        double image_aspect_ratio = imageWidthPixels / (double)imageHeightPixels;
        double page_aspect_ratio = pageWidthInch / pageHeightInch;

        if(image_aspect_ratio > page_aspect_ratio)
        {
          // means our image has the width as the maximum dimension
          imageWidthInch = pageWidthInch;
          imageHeightInch = pageWidthInch / image_aspect_ratio;
        }
        else
        {
          // means our image has the height as the maximum dimension
          imageWidthInch = pageHeightInch * image_aspect_ratio;
          imageHeightInch = pageHeightInch;
        }

        fResolutionDpiX = imageWidthPixels / imageWidthInch;
        fResolutionDpiY = imageHeightPixels / imageHeightInch;
      }

      pageWidthPixels = (int)(pageWidthInch * fResolutionDpiX);
      pageHeightPixels = (int)(pageHeightInch * fResolutionDpiY);

      fImageSizeInch = new PageSize(imageWidthInch, imageHeightInch);

      fBoundsPixel.Width = pageWidthPixels;
      fBoundsPixel.Height = pageHeightPixels;
      fBoundsPixel.X = 0;
      fBoundsPixel.Y = 0;

      fImageBoundsPixel.Width = imageWidthPixels;
      fImageBoundsPixel.Height = imageHeightPixels;
      fImageBoundsPixel.X = (pageWidthPixels - imageWidthPixels) / 2;
      fImageBoundsPixel.Y = (pageHeightPixels - imageHeightPixels) / 2;
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
    };


    readonly RotateFlipType[] rf_mirrored_table =
    { 
      RotateFlipType.RotateNoneFlipX,
      RotateFlipType.Rotate90FlipY,
      RotateFlipType.Rotate180FlipX,
      RotateFlipType.Rotate270FlipY,
    };


    protected void TransformImage(Image image)
    {
      if(fMirrored)
      {
        image.RotateFlip(rf_mirrored_table[fOrientation]);
      }
      else
      {
        image.RotateFlip(rf_table[fOrientation]);
      }
    }


    private bool IsFlipped
    {
      get { return (bool)((fOrientation & 1) == 0); }
    }


    public int ImageHeightPixels
    {
      get { return IsFlipped ? fImageHeightPixels : fImageWidthPixels; }
    }


    public int ImageWidthPixels
    {
      get { return IsFlipped ? fImageWidthPixels : fImageHeightPixels; }
    }


    public int ImageVerticalResolutionDpi
    {
      get { return IsFlipped ? fImageVerticalResolutionDpi : fImageHorizontalResolutionDpi; }
    }


    public int ImageHorizontalResolutionDpi
    {
      get { return IsFlipped ? fImageHorizontalResolutionDpi : fImageVerticalResolutionDpi; }
    }


    public bool ImageResolutionIsDefined
    {
      get { return (bool)((fImageVerticalResolutionDpi != 0) && (fImageHorizontalResolutionDpi != 0)); }
    }


    public void RotateClockwise()
    {
      fOrientation = (fOrientation + 1) % 4;
      RefreshImage();
    }


    public void RotateCounterClockwise()
    {
      fOrientation = (fOrientation + 3) % 4;
      RefreshImage();
    }


    public void MirrorHorizontally()
    {
      fMirrored = !fMirrored;

      if(IsFlipped == false)
      {
        fOrientation = (fOrientation + 2) % 4;
      }

      RefreshImage();
    }


    public void MirrorVertically()
    {
      fMirrored = !fMirrored;

      if(IsFlipped == true)
      {
        fOrientation = (fOrientation + 2) % 4;
      }

      RefreshImage();
    }


    public int Orientation
    {
      get { return fOrientation * 90; }
    }


    public bool IsMirrored
    {
      get { return fMirrored; }
    }


    public void Landscape()
    {
      if(fSizeInch != null)
      {
        double temp = fSizeInch.Width;
        fSizeInch.Width = fSizeInch.Height;
        fSizeInch.Height = temp;
        RefreshImage();
      }
    }


    public bool IsLandscape
    {
      get 
      { 
        return (fSizeInch != null) && (fSizeInch.Width > fSizeInch.Height); 
      }
    }


    public PageSize Size
    {
      get { return fSizeInch; }
      protected set { fSizeInch = value; }
    }


    public PageSize ImageSize
    {
      get { return fImageSizeInch; }
    }


    public Rectangle Bounds
    {
      get { return fBoundsPixel; }
    }


    public Rectangle ImageBounds
    {
      get { return fImageBoundsPixel; }
    }


    public double ResolutionDpiX
    {
      get { return fResolutionDpiX; }
    }


    public double ResolutionDpiY
    {
      get { return fResolutionDpiY; }
    }
  }
}
