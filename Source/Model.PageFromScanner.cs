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
    private string fFilename;


    public PageFromScanner(Image image, int dpi, int compressionFactor)
    {
      // get a temporary path
      fFilename = Path.GetTempFileName();

      Utils.Imaging.SaveImageAsJpeg(image, fFilename, compressionFactor);

      double pageWidth = image.Width / (double)dpi;
      double pageHeight = image.Height / (double)dpi;

      Initialize(new SizeInches(pageWidth, pageHeight), dpi, dpi);
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
