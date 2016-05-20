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
    private Image fLayoutThumbnail;
    private SizePixels fImageSizePixels;
    private ResolutionDpi fImageResolutionDpi;
    private BoundsInches fImageBoundsInches;
    private SizeInches fSizeInch;
    private ResolutionDpi fResolutionDpi;
    private int fOrientation;
    private bool fIsMirrored;
    private string fTransformedImagePath;


    public Page()
    {
      fSourceThumbnail = null;
      fThumbnail = null;
      fLayoutThumbnail = null;
      fImageSizePixels = null;
      fImageResolutionDpi = null;
      fImageBoundsInches = null;
      fSizeInch = null;
      fResolutionDpi = null;
      fOrientation = 0;
      fIsMirrored = false;
      fTransformedImagePath = null;
    }


    public virtual string Name
    {
      get { return "Hello"; }
    }


    protected void Initialize(SizeInches pageSizeInches, int imageHorizontalDpi, int imageVerticalDpi)
    {
      using(Image myImage = CreateImage())
      {
        fSizeInch = pageSizeInches;
        fImageSizePixels = new SizePixels(myImage.Size.Width, myImage.Size.Height);
        fImageResolutionDpi = new ResolutionDpi(imageHorizontalDpi, imageVerticalDpi);
        fSourceThumbnail = Utils.Imaging.CreateThumbnail(myImage, 200);
      }
      RefreshImage();
    }


    abstract protected Image CreateImage();


    public Image GetImage()
    {
      Image result;

      if(SameAsSourceImage())
      {
        result = CreateImage();
      }
      else
      {
        result = Imaging.LoadImageFromFile(fTransformedImagePath);
      }

      return result;
    }


    public Image ImageThumbnail
    {
      get { return fThumbnail; }
    }


    public Image GetLayoutImage()
    {
      return MakeLayoutImage(GetImage());
    }


    public Image LayoutThumbnail
    {
      get { return fLayoutThumbnail; }
    }


    private void RefreshImage()
    {
      if(SameAsSourceImage() == false)
      {
        Image image = CreateImage();

        TransformImage(image);

        if(String.IsNullOrEmpty(fTransformedImagePath))
        {
          fTransformedImagePath = TempFolder.GetFileName();
        }

        Utils.Imaging.SaveImageAsJpeg(image, fTransformedImagePath, 90);
      }

      fThumbnail = (Image)fSourceThumbnail.Clone();
      TransformImage(fThumbnail);
      CalculateBounds();
      fLayoutThumbnail = MakeLayoutImage(fThumbnail);
    }


    private bool SameAsSourceImage()
    {
      return ((fIsMirrored == false) && (fOrientation == 0));
    }


    private void CalculateBounds()
    {
      SizePixels imageSizePixels = fImageSizePixels.Transform(IsFlipped);
      SizeInches pageSizeInches = fSizeInch;

      if(ImageResolutionIsDefined)
      {
        fResolutionDpi = fImageResolutionDpi.Transform(IsFlipped);
      }
      else
      {
        double image_aspect_ratio = imageSizePixels.Width / (double)imageSizePixels.Height;
        double page_aspect_ratio = pageSizeInches.Width / pageSizeInches.Height;

        double res;

        if(image_aspect_ratio > page_aspect_ratio)
        {
          // means our image has the width as the maximum dimension
          res = imageSizePixels.Width / pageSizeInches.Width;
        }
        else
        {
          // means our image has the height as the maximum dimension
          res = imageSizePixels.Height / pageSizeInches.Height;
        }

        fResolutionDpi = new ResolutionDpi(res, res);
      }

      // Calculate Image Layout
      double imageWidthInch = imageSizePixels.Width / fResolutionDpi.Horizontal;
      double imageHeightInch = imageSizePixels.Height / fResolutionDpi.Vertical;

      double x = (pageSizeInches.Width - imageWidthInch) / 2;
      double y = (pageSizeInches.Height - imageHeightInch) / 2;

      fImageBoundsInches = new BoundsInches(x, y, imageWidthInch, imageHeightInch);
    }


    private Image MakeLayoutImage(Image img)
    {
      int width;
      int height;

      double image_aspect_ratio = img.Width / (double)img.Height;
      double page_aspect_ratio = this.Size.Width / this.Size.Height;

      if(image_aspect_ratio > page_aspect_ratio)
      {
        // means our image has the width as the maximum dimension
        width = (int)img.Width;
        height = (int)(img.Width / page_aspect_ratio);
      }
      else
      {
        // means our image has the height as the maximum dimension
        width = (int)(img.Height * page_aspect_ratio);
        height = (int)img.Height;
      }

      Bitmap pageBitmap = new Bitmap(width, height);

      Rectangle pageBounds = new Rectangle(0, 0, pageBitmap.Width, pageBitmap.Height);
      Rectangle imageBounds = Utils.Imaging.FitToArea(img.Width, img.Height, pageBounds);

      using(Graphics g = Graphics.FromImage(pageBitmap))
      {
        g.FillRectangle(Brushes.White, pageBounds);
        g.DrawImage(img, imageBounds);
      }

      return pageBitmap;
    }


    virtual public void CleanUp()
    {
      if(fSourceThumbnail != null)
      {
        fSourceThumbnail.Dispose();
        fSourceThumbnail = null;
      }

      if(fThumbnail != null)
      {
        fThumbnail.Dispose();
        fThumbnail = null;
      }

      if(fLayoutThumbnail != null)
      {
        fLayoutThumbnail.Dispose();
        fLayoutThumbnail = null;
      }
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


    public virtual bool CanModify()
    {
      // can normally modify 
      return true;
    }


    protected void TransformImage(Image image)
    {
      if(fIsMirrored)
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
      get { return ((fOrientation & 1) == 1); }
    }


    private bool ImageResolutionIsDefined
    {
      get { return ((fImageResolutionDpi.Horizontal != 0) && (fImageResolutionDpi.Vertical != 0)); }
    }


    public void ImageRotateClockwise()
    {
      fOrientation = (fOrientation + 1) % 4;
      RefreshImage();
    }


    public void ImageRotateCounterClockwise()
    {
      fOrientation = (fOrientation + 3) % 4;
      RefreshImage();
    }


    public void ImageMirrorHorizontally()
    {
      fIsMirrored = !fIsMirrored;

      if(IsFlipped == true)
      {
        fOrientation = (fOrientation + 2) % 4;
      }

      RefreshImage();
    }


    public void ImageMirrorVertically()
    {
      fIsMirrored = !fIsMirrored;

      if(IsFlipped == false)
      {
        fOrientation = (fOrientation + 2) % 4;
      }

      RefreshImage();
    }


    public void RotateSideways()
    {
      Size = Size.Transform(true);
    }


    public SizeInches Size
    {
      get { return fSizeInch; }
      set { fSizeInch = value; RefreshImage(); }
    }


    public BoundsInches ImageBoundsInches
    {
      get { return fImageBoundsInches; }
    }


    public ResolutionDpi ResolutionDpi
    {
      get { return fResolutionDpi; }
    }
  }
}
