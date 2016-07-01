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
      else if(String.IsNullOrEmpty(fTemporaryFilePath))
      {
        result = CreateImage();
        Transform(result);
      }
      else
      {
        // Transformed image is already stored in a temporary file.
        result = Imaging.LoadImageFromFile(fTemporaryFilePath);
      }

      return result;
    }


    public Image GetSourceImage()
    {
      return CreateImage();
    }


    public Image Thumbnail
    {
      get { return fThumbnail; }
    }


    private void RefreshImage()
    {
#if USE_TEMPORARY_FILE_FOR_TRANSFORMED_IMAGE
      if(SameAsSourceImage() == false)
      {
        Image image = CreateImage();

        Transform(image);

        if(String.IsNullOrEmpty(fTemporaryFilePath))
        {
          fTemporaryFilePath = TempFolder.GetFileName();
        }

        Utils.Imaging.EncodeSaveImageToFile(image, fTemporaryFilePath, System.Drawing.Imaging.ImageFormat.Png, 0);
      }
#endif
      fThumbnail = Imaging.ImageClone(fSourceThumbnail);
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


    protected void Transform(Image image)
    {
      Imaging.TransformImage(image, TransformationIndex);
    }


    private bool SameAsSourceImage()
    {
      return (TransformationIndex == 0);
    }


    private bool IsFlipped
    {
      get { return ((fOrientation & 1) == 1); }
    }


    public int TransformationIndex
    {
      get { return fOrientation + (fIsMirrored ? 4 : 0);  }
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
