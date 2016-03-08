using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;


namespace Utils
{
  class UtilImaging
  {
    public static void SaveImageAsJpeg(Image image, string fileName, long quality)
    {
      EncoderParameters encoderParams = new EncoderParameters(1);
      encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
      ImageCodecInfo codecInfo = GetEncoderInfo(ImageFormat.Jpeg);
      image.Save(fileName, codecInfo, encoderParams);
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
  }
}
