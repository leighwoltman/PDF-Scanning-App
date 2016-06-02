using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Utils;
using Defines;


namespace Model
{
  public interface InterfaceImageCreator
  {
    Image CreateImage();
  }


  public class ImageHandler
  {
    private InterfaceImageCreator fImageCreator;
    private Image fSourceThumbnail;
    private Image fThumbnail;
    private SizePixels fSizePixels;
    private ResolutionDpi fResolutionDpi;
    private int fOrientation;
    private bool fIsMirrored;
    private string fTemporaryFilePath;


    public ImageHandler(InterfaceImageCreator imageCreator)
    {
      fImageCreator = imageCreator;
      fSourceThumbnail = null;
      fThumbnail = null;
      fSizePixels = null;
      fResolutionDpi = null;
      fOrientation = 0;
      fIsMirrored = false;
      fTemporaryFilePath = null;
    }


    public void Initialize(int horizontalDpi, int verticalDpi)
    {
      using(Image myImage = CreateImage())
      {
        fSizePixels = new SizePixels(myImage.Size.Width, myImage.Size.Height);
        fResolutionDpi = new ResolutionDpi(horizontalDpi, verticalDpi);
        fSourceThumbnail = Utils.Imaging.CreateThumbnail(myImage, 200);
      }
      RefreshImage();
    }


    private Image CreateImage()
    {
      return fImageCreator.CreateImage();
    }


    public Image GetImage()
    {
      Image result;

      if(SameAsSourceImage())
      {
        result = CreateImage();
      }
      else
      {
        result = Imaging.LoadImageFromFile(fTemporaryFilePath);
      }

      return result;
    }


    public Image GetImageInOriginalFormat()
    {
      Image image = CreateImage();

      if(SameAsSourceImage() == false)
      {
        System.Drawing.Imaging.ImageFormat originalFormat = image.RawFormat;

        Transform(image);

        byte[] byteArray = Imaging.EncodeImage(image, originalFormat, 80);

        image = Imaging.ImageFromByteArray(byteArray);
      }

      return image;
    }


    public Image GetImageFromMemory()
    {
      Image image = CreateImage();

      if(SameAsSourceImage() == false)
      {
        Transform(image);
      }

      return image;
    }


    public Image GetCompressedImage(int compressionFactor)
    {
      Image result;
      Image image = GetImageFromMemory();

      if(image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
      {
        // Do not compress a Monochome image. 
        // Because Jpeg format will make the file bigger and lower the quality.
        result = image;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        // The original picture is already compressed as Jpeg.
        result = image;
      }
      else
      {
        byte[] byteArray = Imaging.EncodeImageAsJpeg(image, compressionFactor);
        result = Imaging.ImageFromByteArray(byteArray);
      }

      return result;
    }


    public Image Thumbnail
    {
      get { return fThumbnail; }
    }


    private void RefreshImage()
    {
      if(SameAsSourceImage() == false)
      {
        Image image = CreateImage();

        Transform(image);

        if(String.IsNullOrEmpty(fTemporaryFilePath))
        {
          fTemporaryFilePath = TempFolder.GetFileName();
        }

        Utils.Imaging.SaveImageAsJpeg(image, fTemporaryFilePath, 90);
      }

      fThumbnail = (Image)fSourceThumbnail.Clone();
      Transform(fThumbnail);
    }


    public virtual void CleanUp()
    {
      if(String.IsNullOrEmpty(fTemporaryFilePath) == false)
      {
        File.Delete(fTemporaryFilePath);
      }

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


    protected void Transform(Image image)
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


    private bool SameAsSourceImage()
    {
      return ((fIsMirrored == false) && (fOrientation == 0));
    }


    private bool IsFlipped
    {
      get { return ((fOrientation & 1) == 1); }
    }


    public SizePixels SizePixels
    {
      get { return fSizePixels.Transform(IsFlipped); }
    }


    public ResolutionDpi ResolutionDpi
    {
      get { return fResolutionDpi.Transform(IsFlipped); }
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
  }
}
