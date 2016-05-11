using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Utils;


namespace Model
{
  public class PageFromScanner : Page
  {
    static private int fScanNumber = 1;
    static private string fTempFolder = null;

    static private string GetTempFolder()
    {
      if(fTempFolder == null)
      {
        fTempFolder = Path.Combine(Path.GetTempPath(), AppInfo.GetApplicationName());

        if(Directory.Exists(fTempFolder))
        {
          Directory.Delete(fTempFolder, true);
        }

        Directory.CreateDirectory(fTempFolder);
      }

      return fTempFolder;
    }

    static private string GetTempFileName(int number)
    {
      return Path.Combine(GetTempFolder(), "Scan" + number.ToString() + ".tmp");
    }


    private string fFilename;


    public PageFromScanner(Image image, int dpi, int compressionFactor)
    {
      // get a temporary path
      fFilename = GetTempFileName(fScanNumber);
      fScanNumber++;

      Utils.Imaging.SaveImageAsJpeg(image, fFilename, compressionFactor);

      double pageWidth = image.Width / (double)dpi;
      double pageHeight = image.Height / (double)dpi;

      Initialize(new SizeInches(pageWidth, pageHeight), dpi, dpi);
    }


    public override string Name
    {
      get { return Path.GetFileNameWithoutExtension(fFilename); }
    }

    
    protected override Image CreateImage()
    {
      return Image.FromFile(fFilename);
    }


    public override void CleanUp()
    {
      try
      {
        File.Delete(fFilename);
      }
      catch(Exception)
      {
        // do nothing
      }
      base.CleanUp();
    }
  }
}
