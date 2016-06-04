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


    public static byte[] ByteArrayFromImage(Image image)
    {
      byte[] result = (byte[])fImageConverter.ConvertTo(image, typeof(byte[]));
      return result;
    }


    public static Image ImageFromByteArray(byte[] byteArray)
    {
      Image result = (Image)fImageConverter.ConvertFrom(byteArray);
      return result;
    }


    public static byte[] LoadImageByteArrayFromFile(string fileName)
    {
      return File.ReadAllBytes(fileName);
    }


    public static void SaveImageByteArrayToFile(string fileName, byte[] byteArray)
    {
      File.WriteAllBytes(fileName, byteArray);
    }


    public static Image LoadImageFromFile(string fileName)
    {
      byte[] byteArray = LoadImageByteArrayFromFile(fileName);
      return ImageFromByteArray(byteArray);
    }


    public static void SaveImageToFile(Image image, string fileName)
    {
      byte[] byteArray = ByteArrayFromImage(image);
      SaveImageByteArrayToFile(fileName, byteArray);
    }


    public static string GetImageFormatDescription(Image image)
    {
      return GetFormatDescription(image.RawFormat);
    }


    public static string GetFormatDescription(System.Drawing.Imaging.ImageFormat rawFormat)
    {
      string[] result = GetImageRawFormatDescription(rawFormat);
      return result[0];
    }


    public static string GetImageExtension(Image image)
    {
      string[] result = GetImageRawFormatDescription(image.RawFormat);
      return result[1];
    }


    public static string GetImageExtensionFromByteArray(byte[] byteArray)
    {
      Image image = ImageFromByteArray(byteArray);
      return GetImageExtension(image);
    }


    private static string[] GetImageRawFormatDescription(System.Drawing.Imaging.ImageFormat rawFormat)
    {
      string[] result = new string[2];

      if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        result[0] = "Jpeg";
        result[1] = ".jpg";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
      {
        result[0] = "Bmp";
        result[1] = ".bmp";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
      {
        result[0] = "Png";
        result[1] = ".png";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
      {
        result[0] = "Gif";
        result[1] = ".gif";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
      {
        result[0] = "Emf";
        result[1] = ".image";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
      {
        result[0] = "Exif";
        result[1] = ".image";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
      {
        result[0] = "Icon";
        result[1] = ".image";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
      {
        result[0] = "MemoryBmp";
        result[1] = ".image";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
      {
        result[0] = "Tiff";
        result[1] = ".image";
      }
      else if(rawFormat.Equals(System.Drawing.Imaging.ImageFormat.Wmf))
      {
        result[0] = "Wmf";
        result[1] = ".image";
      }
      else
      {
        result[0] = "Unknown";
        result[1] = ".image";
      }

      return result;
    }


    public static void EncodeSaveImageToFile(Image image, string fileName, ImageFormat format, int quality)
    {
      byte[] byteArray = EncodeImage(image, format, quality);
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


    public static byte[] EncodeImage(Image image, ImageFormat format, int quality)
    {
      EncoderParameters encoderParams = null;

      if(format.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
      {
        // quality is used by Jpeg and probably ignored by other formats
        encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
      }

      return EncodeImage(image, format, encoderParams);
    }


    public static Image ConvertImage(Image image, ImageFormat format, int quality)
    {
      Image result;

      if(image.RawFormat.Equals(format))
      {
        // No need to do conversion since the image is already in desired format
        result = image;
      }
      else
      {
        byte[] byteArray = Imaging.EncodeImage(image, format, quality);
        result = Imaging.ImageFromByteArray(byteArray);
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


    public static Image ImageClone(Image source)
    {
      byte[] byteArray = ByteArrayFromImage(source);
      return ImageFromByteArray(byteArray);
    }
  }
}
