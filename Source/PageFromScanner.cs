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


    public PageFromScanner(Image image, int dpi)
    {
      // get a temporary path
      fFilename = Path.GetTempFileName();

      UtilImaging.SaveImageAsJpeg(image, fFilename, 75L);

      double pageWidth = image.Width / (double)dpi;
      double pageHeight = image.Height / (double)dpi;

      this.Size = new PageSize(pageWidth, pageHeight);

      InitializeImage(dpi, dpi);
    }


    protected override Image CreateImage()
    {
      return Image.FromFile(fFilename);
    }


    public override void CleanUp()
    {
      File.Delete(fFilename);
      base.CleanUp();
    }
  }
}
