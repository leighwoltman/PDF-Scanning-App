using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;


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


    public static Image ImageFromFile2(string fileName)
    {
      Image result;

      // This is a recommended way of loading the file to make sure the file is released afterwords
      // http://stackoverflow.com/questions/4803935/free-file-locked-by-new-bitmapfilepath/8701748#8701748

      using(var bmpTemp = new Bitmap(fileName))
      {
        result = new Bitmap(bmpTemp);
      }

      return result;
    }


    public static Image ImageFromFile3(string fileName)
    {
      // This approach also seems to work. From
      // http://stackoverflow.com/questions/4803935/free-file-locked-by-new-bitmapfilepath
      
      var bytes = File.ReadAllBytes(fileName);
      var ms = new MemoryStream(bytes);
      var img = Image.FromStream(ms);
      return img;
    }


    public static void SaveImageAsJpeg(Image image, string fileName, int quality)
    {
      EncoderParameters encoderParams = new EncoderParameters(1);
      encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
      ImageCodecInfo codecInfo = GetEncoderInfo(ImageFormat.Jpeg);
      image.Save(fileName, codecInfo, encoderParams);
    }


    public static void SaveImageAsJpeg(Image image, Stream stream, int quality)
    {
      EncoderParameters encoderParams = new EncoderParameters(1);
      encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
      ImageCodecInfo codecInfo = GetEncoderInfo(ImageFormat.Jpeg);
      image.Save(stream, codecInfo, encoderParams);
    }


    public static Image EncodeImageAsJpeg(Image image, int quality)
    {
      MemoryStream stream = new MemoryStream();
      SaveImageAsJpeg(image, stream, quality);
      return Image.FromStream(stream);
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
  }
}
