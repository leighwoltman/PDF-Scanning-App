using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Utils;


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

          UtilImaging.SaveImageAsJpeg(images[i], fileName, 75L);

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
