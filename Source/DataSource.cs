using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;


namespace PDFScanningApp
{
  public class ScanSettings
  { 
  }


  public class DataSourceNewPageEventArgs : EventArgs
  {
    public Page ThePage;
  }


  public class DataSource
  {
    public bool Acquire(ScanSettings settings)
    {
      bool success = false;

      return success;
    }


    public bool Scan(string scannerName, double width, double height, double resolution)
    {
      bool result = false;

      List<Image> images = WIAScanner.Scan(scannerName, width, height, resolution);

      if (images != null && images.Count != 0)
      {
        for(int i = 0; i < images.Count; i++)
        {
          // get a temporary path
          string fileName = Path.GetTempFileName();

          ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

          // Create an Encoder object based on the GUID 
          // for the Quality parameter category.
          System.Drawing.Imaging.Encoder myEncoder =
              System.Drawing.Imaging.Encoder.Quality;

          // Create an EncoderParameters object. 
          // An EncoderParameters object has an array of EncoderParameter 
          // objects. In this case, there is only one 
          // EncoderParameter object in the array.
          EncoderParameters myEncoderParameters = new EncoderParameters(1);

          EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
          myEncoderParameters.Param[0] = myEncoderParameter;

          // use max as the filename
          images[i].Save(fileName, jgpEncoder, myEncoderParameters);

          Page myPage = new Page(fileName, true, height == 14 ? ScanPageSize.Legal : ScanPageSize.Letter);

          Raise_OnNewPictureData(myPage);
        }

        // TODO: Do we really need this?
        // close all these images
        while(images.Count > 0)
        {
          Image img = images[0];
          images.RemoveAt(0);
          img.Dispose();
        }

        Raise_OnScanningComplete();

        result = true;
      }

      return result;
    }


    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
      ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

      foreach(ImageCodecInfo codec in codecs)
      {
        if(codec.FormatID == format.Guid)
        {
          return codec;
        }
      }
      return null;
    }


    public event EventHandler OnNewPage;

    private void Raise_OnNewPictureData(Page page)
    {
      if(OnNewPage != null)
      {
        DataSourceNewPageEventArgs args = new DataSourceNewPageEventArgs();
        args.ThePage = page;
        OnNewPage(this, args);
      }
    }


    public event EventHandler OnScanningComplete;

    private void Raise_OnScanningComplete()
    {
      if(OnScanningComplete != null)
      {
        OnScanningComplete(this, EventArgs.Empty);
      }
    }
  }
}
