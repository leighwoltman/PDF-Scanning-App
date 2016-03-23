using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
    private PageSize fSizeInch;
    private PageSize fImageSizeInch;
    private Rectangle fBoundsPixel;
    private Rectangle fImageBoundsPixel;

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
        imageWidthInch = imageWidthPixels / (double)ImageHorizontalResolutionDpi;
        imageHeightInch = imageHeightPixels / (double)ImageVerticalResolutionDpi;
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
      }

      pageWidthPixels = (int)(pageWidthInch * imageWidthPixels / imageWidthInch);
      pageHeightPixels = (int)(pageHeightInch * imageHeightPixels / imageHeightInch);

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


    virtual public void AddPdfPage(PdfDocument pdfDocument)
    {
      // Create an empty page
      PdfPage pdfPage = pdfDocument.AddPage();

      if (this.Size == PageSize.Legal)
      {
        pdfPage.Size = PdfSharp.PageSize.Legal;
      }
      else
      {
        pdfPage.Size = PdfSharp.PageSize.Letter;
      }

      // we need to swap the height and the width if page is a landscape image
      double aspect_ratio = ((double)this.ImageWidthPixels) / ((double)this.ImageHeightPixels);

      if (this.IsLandscape)
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

      if (this.IsLandscape)
      {
        // these are swapped
        draw_point_width = (int)pdfPage.Height;
        draw_point_height = (int)pdfPage.Width;

        if (aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
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

        if (aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
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


      XImage image = XImage.FromGdiPlusImage(this.GetImage());

      //if(page.IsRotated())
      //{
      //  // rotate around the center of the pdfPage
      //  gfx.RotateAtTransform(-180, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      //}

      //if(page.IsLandscape())
      //{
      //  // rotate around the center of the pdfPage
      //  gfx.RotateAtTransform(90, new XPoint(pdfPage.Width / 2, pdfPage.Height / 2));
      //}

      gfx.DrawImage(image, draw_point_x, draw_point_y, draw_point_width, draw_point_height);
      image.Dispose();
    }
  }
}
