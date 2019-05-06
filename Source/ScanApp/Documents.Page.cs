using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseUtils;
using HouseImaging;
using PdfProcessing;


namespace Documents
{
  public abstract class Page
  {
    private ImageTransformer fTransform = new ImageTransformer();


    public Page(Size2D pageSize, int dpi = 0)
    {
      Size = pageSize;
      ResolutionDpi = dpi;
    }


    public virtual bool CanModify()
    {
      // can normally modify 
      return true;
    }


    public virtual List<KeyValuePair<string, string>> GetInfoTable()
    {
      List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

      ImageInfo imageInfo = this.GetSourceImage();

      result.Add(new KeyValuePair<string, string>("Name", this.Name));
      result.Add(new KeyValuePair<string, string>("Image Type", imageInfo.GetImageFormatDescription()));
      result.Add(new KeyValuePair<string, string>("Height", imageInfo.SizePixels.Height.ToString()));
      result.Add(new KeyValuePair<string, string>("Width", imageInfo.SizePixels.Width.ToString()));
      result.Add(new KeyValuePair<string, string>("Pixel Format", imageInfo.GetPixelFormat()));

      return result;
    }


    public abstract string Name { get; }

    public int ResolutionDpi { get; private set; }

    public Size2D Size { get; private set; }


    public abstract ImageInfo GetSourceImage();


    public ImageInfo GetTransformedImage()
    {
      ImageInfo result = GetSourceImage();
      result.Transform(fTransform);
      return result;
    }


    public ImageInfo GetLayout()
    {
      ImageInfo result;

      using (ImageInfo myImage = GetTransformedImage())
      {
        result = MakeLayout(myImage);
      }

      return result;
    }


    public virtual ImageInfo GetSourceThumbnail(int width, int height)
    {
      ImageInfo result = null;

      using (ImageInfo myImage = GetSourceImage())
      {
        result = myImage.CreateThumbnail(width, height);
      }

      return result;
    }


    public ImageInfo CreateThumbnail(int width, int height)
    {
      ImageInfo result = null;

      using (ImageInfo miniImage = GetSourceThumbnail(width, height))
      {
        miniImage.Transform(fTransform);
        result = MakeLayout(miniImage);
      }

      return result;
    }


    public void ImageRotateClockwise()
    {
      fTransform.RotateClockwise();
    }


    public void ImageRotateCounterClockwise()
    {
      fTransform.RotateCounterClockwise();
    }


    public void ImageMirrorHorizontally()
    {
      fTransform.MirrorHorizontally();
    }


    public void ImageMirrorVertically()
    {
      fTransform.MirrorVertically();
    }


    public void RotateSideways()
    {
      Size = new Size2D(Size.Height, Size.Width);
    }


    public virtual void ExportToPdf(PdfDocument pdf, ExportSettings exportSettings)
    {
      using (ImageInfo sourceImage = GetSourceImage())
      {
        PdfPage page = new PdfPage(pdf, Size);
        SizePixels imageSizePixels = fTransform.IsFlipped ? sourceImage.SizePixels.Flip() : sourceImage.SizePixels;

        if (exportSettings.CompressImagesForPdfExport)
        {
          if (sourceImage.GetImageFormat() == ImageFormatEnum.Jpeg)
          {
            // Skip compression if it is already Jpeg
          }
          else if (sourceImage.GetPixelFormatBitsPerPixel() == 1)
          {
            // Don't use Jpeg compression for monochrome pictures, because it makes them bigger
          }
          else
          {
            sourceImage.Convert(ImageFormatEnum.Jpeg, exportSettings.JpegCompressionFactor);
          }
        }

        page.DrawImage(sourceImage, fTransform, GetImageBounds(imageSizePixels));
      }
    }


    public void SaveImageToFile(string path, ExportSettings exportSettings)
    {
      ImageInfo image = this.GetTransformedImage();
      image.SaveImageToFile(path, exportSettings.JpegCompressionFactor);
    }


    // Boundaries of the transformed image within the page in inches
    public Bounds GetImageBounds(SizePixels imageSizePixels)
    {
      double image_aspect_ratio = imageSizePixels.Width / (double)imageSizePixels.Height;
      double page_aspect_ratio = this.Size.Width / this.Size.Height;

      double imageWidthInch;
      double imageHeightInch;

      if (image_aspect_ratio > page_aspect_ratio)
      {
        // means our image has the width as the maximum dimension
        imageWidthInch = this.Size.Width;
        imageHeightInch = this.Size.Width / image_aspect_ratio;
      }
      else
      {
        // means our image has the height as the maximum dimension
        imageWidthInch = this.Size.Height * image_aspect_ratio;
        imageHeightInch = this.Size.Height;
      }

      // Calculate Image Layout
      double x = (this.Size.Width - imageWidthInch) / 2;
      double y = (this.Size.Height - imageHeightInch) / 2;

      return new Bounds(x, y, imageWidthInch, imageHeightInch);
    }


    private ImageInfo MakeLayout(ImageInfo transformedImage)
    {
      System.Drawing.Image img = transformedImage.SystemImage; 

      int width;
      int height;

      double image_aspect_ratio = img.Width / (double)img.Height;
      double page_aspect_ratio = this.Size.Width / this.Size.Height;

      if (image_aspect_ratio > page_aspect_ratio)
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

      System.Drawing.Bitmap pageBitmap = new System.Drawing.Bitmap(width, height);

      System.Drawing.Rectangle pageBounds = new System.Drawing.Rectangle(0, 0, pageBitmap.Width, pageBitmap.Height);
      System.Drawing.Rectangle imageBounds = HouseImaging.ImageTools.FitToArea(img.Width, img.Height, pageBounds);

      using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pageBitmap))
      {
        g.FillRectangle(System.Drawing.Brushes.White, pageBounds);
        g.DrawImage(img, imageBounds);
      }

      return new ImageInfo(pageBitmap);
    }
  }


  public class ExportSettings
  {
    public bool CompressImagesForPdfExport { get; set; } = false;

    public bool RemovePagesAfterPdfExport { get; set; } = false;

    public int JpegCompressionFactor { get; set; } = 70;

    public ExportSettings Copy()
    {
      return new ExportSettings()
      {
        CompressImagesForPdfExport = this.CompressImagesForPdfExport,
        RemovePagesAfterPdfExport = this.RemovePagesAfterPdfExport,
        JpegCompressionFactor = this.JpegCompressionFactor
      };
    }
  }
}
