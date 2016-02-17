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
  }
}
