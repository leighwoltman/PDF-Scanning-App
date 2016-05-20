using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Defines;


namespace Utils
{
  class Imaging
  {
    private static readonly ImageConverter fImageConverter = new ImageConverter();


    public static byte[] ImageToByteArray(Image image)
    {
      byte[] result = (byte[])fImageConverter.ConvertTo(image, typeof(byte[]));
      return result;
    }


    public static Image ByteArrayToImage(byte[] byteArray)
    {
      Image result = (Image)fImageConverter.ConvertFrom(byteArray);
      return result;
    }


    public static Image LoadImageFromFile(string fileName)
    {
      byte[] byteArray = File.ReadAllBytes(fileName);
      return ByteArrayToImage(byteArray);
    }


    public static void SaveImageToFile(Image image, string fileName)
    {
      byte[] byteArray = ImageToByteArray(image);
      File.WriteAllBytes(fileName, byteArray);
    }


    public static ImageFormatEnum GetImageFormat(Image image)
    {
      ImageFormatEnum result;

      if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        result = ImageFormatEnum.Jpeg;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
      {
        result = ImageFormatEnum.Bmp;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
      {
        result = ImageFormatEnum.Png;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
      {
        result = ImageFormatEnum.Emf;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
      {
        result = ImageFormatEnum.Exif;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
      {
        result = ImageFormatEnum.Gif;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
      {
        result = ImageFormatEnum.Icon;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
      {
        result = ImageFormatEnum.Bmp;
      }
      else if(image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
      {
        result = ImageFormatEnum.Tiff;
      }
      else
      {
        result = ImageFormatEnum.Unknown;
      }

      return result;
    }


    public static void EncodeSaveImageToFile(Image image, string fileName, ImageFormat format, EncoderParameters encoderParams)
    {
      byte[] byteArray = EncodeImage(image, format, encoderParams);
      File.WriteAllBytes(fileName, byteArray);
    }


    public static byte[] EncodeImage(Image image, ImageFormat format, EncoderParameters encoderParams)
    {
      byte[] result = null;

      ImageCodecInfo codecInfo = GetEncoderInfo(format);

      if(codecInfo != null)
      {
        using(MemoryStream stream = new MemoryStream())
        {
          image.Save(stream, codecInfo, encoderParams);
          result = stream.ToArray();
        }
      }

      return result;
    }


    private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
    {
      ImageCodecInfo result = null;
      ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

      for(int j = 0; (result == null) && (j < encoders.Length); j++)
      {
        if(encoders[j].FormatID == format.Guid)
        {
          result = encoders[j];
        }
      }
      return result;
    }


    public static void SaveImageAsJpeg(Image image, string fileName, int quality)
    {
      byte[] byteArray = EncodeImageAsJpeg(image, quality);
      File.WriteAllBytes(fileName, byteArray);
    }


    public static byte[] EncodeImageAsJpeg(Image image, int quality)
    {
      EncoderParameters encoderParams = new EncoderParameters(1);
      encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
      return EncodeImage(image, ImageFormat.Jpeg, encoderParams);
    }


    public static Image CreateThumbnail(Image image, int maxDimension)
    {
      int thumbnail_height;
      int thumbnail_width;

      if(image.Size.Height > image.Size.Width)
      {
        thumbnail_height = maxDimension;
        thumbnail_width = (int)(((double)image.Size.Width / (double)image.Size.Height) * thumbnail_height);
      }
      else
      {
        thumbnail_width = maxDimension;
        thumbnail_height = (int)(((double)image.Size.Height / (double)image.Size.Width) * thumbnail_width);
      }
      Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
      return image.GetThumbnailImage(thumbnail_width, thumbnail_height, myCallback, IntPtr.Zero);
    }


    private static bool ThumbnailCallback()
    {
      return false;
    }


    public static Rectangle FitToArea(int imageWidth, int imageHeight, Rectangle area)
    {
      double image_aspect_ratio = imageWidth / (double)imageHeight;
      double area_aspect_ratio = area.Width / (double)area.Height;

      int fittedWidth;
      int fittedHeight;

      if(image_aspect_ratio > area_aspect_ratio) // same as page.IsLandscape
      {
        // means our image has the width as the maximum dimension
        fittedWidth = area.Width;
        fittedHeight = (int)(area.Width / image_aspect_ratio);
      }
      else
      {
        // means our image has the height as the maximum dimension
        fittedWidth = (int)(area.Height * image_aspect_ratio);
        fittedHeight = area.Height;
      }

      Rectangle result = new Rectangle();

      result.Width = fittedWidth;
      result.Height = fittedHeight;
      result.X = area.X + ((area.Width - fittedWidth) / 2);
      result.Y = area.Y + ((area.Height - fittedHeight) / 2);

      return result;
    }


    public static bool ImageEquals(Image img1, Image img2)
    {
      bool result = img1.Size.Equals(img2.Size);

      if(result)
      {
        Bitmap bmp1 = new Bitmap(img1);
        Bitmap bmp2 = new Bitmap(img2);

        for(int x = 0; result && (x < bmp1.Width); x++)
        {
          for(int y = 0; result && (y < bmp1.Height); y++)
          {
            if(bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
            {
              result = false;
            }
          }
        }
      }

      return result;
    }
  }
}
