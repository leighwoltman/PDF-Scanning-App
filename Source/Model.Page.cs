using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Utils;


namespace Model
{
  abstract public class Page : InterfaceImageCreator
  {
    private ImageHandler fImageHandler;
    private Image fLayoutThumbnail;
    private BoundsInches fImageBoundsInches;
    private SizeInches fSizeInch;
    private ResolutionDpi fResolutionDpi;


    public Page()
    {
      fImageHandler = new ImageHandler(this);
      fLayoutThumbnail = null;
      fImageBoundsInches = null;
      fSizeInch = null;
      fResolutionDpi = null;
    }


    public virtual string Name
    {
      get { return "Hello"; }
    }


    protected void Initialize(SizeInches pageSizeInches, int imageHorizontalDpi, int imageVerticalDpi)
    {
      fSizeInch = pageSizeInches;
      fImageHandler.Initialize(imageHorizontalDpi, imageVerticalDpi);
      RefreshLayout();
    }


    public abstract Image CreateImage();


    public Image GetImage()
    {
      return fImageHandler.GetImage();
    }


    public Image GetImageInOriginalFormat()
    {
      return fImageHandler.GetImageInOriginalFormat();
    }


    public Image GetImageFromMemory()
    {
      return fImageHandler.GetImageFromMemory();
    }


    public Image GetCompressedImage(int compressionFactor)
    {
      return fImageHandler.GetCompressedImage(compressionFactor);
    }

    
    public Image ImageThumbnail
    {
      get { return fImageHandler.Thumbnail; }
    }


    public Image GetLayoutImage()
    {
      return MakeLayoutImage(GetImage());
    }


    public Image LayoutThumbnail
    {
      get { return fLayoutThumbnail; }
    }


    private void RefreshLayout()
    {
      CalculateBounds();
      fLayoutThumbnail = MakeLayoutImage(fImageHandler.Thumbnail);
    }


    private void CalculateBounds()
    {
      SizePixels imageSizePixels = fImageHandler.SizePixels;
      SizeInches pageSizeInches = fSizeInch;

      fResolutionDpi = fImageHandler.ResolutionDpi;

      if(fResolutionDpi.IsDefined == false)
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


    public virtual void CleanUp()
    {
      fImageHandler.CleanUp();

      if(fLayoutThumbnail != null)
      {
        fLayoutThumbnail.Dispose();
        fLayoutThumbnail = null;
      }
    }


    public virtual bool CanModify()
    {
      // can normally modify 
      return true;
    }


    public void ImageRotateClockwise()
    {
      fImageHandler.ImageRotateClockwise();
      RefreshLayout();
    }


    public void ImageRotateCounterClockwise()
    {
      fImageHandler.ImageRotateCounterClockwise();
      RefreshLayout();
    }


    public void ImageMirrorHorizontally()
    {
      fImageHandler.ImageMirrorHorizontally();
      RefreshLayout();
    }


    public void ImageMirrorVertically()
    {
      fImageHandler.ImageMirrorVertically();
      RefreshLayout();
    }


    public void RotateSideways()
    {
      fSizeInch = fSizeInch.Transform(true);
      RefreshLayout();
    }


    public SizeInches Size
    {
      get { return fSizeInch; }
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
