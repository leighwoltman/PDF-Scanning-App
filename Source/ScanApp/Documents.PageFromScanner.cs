using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using HouseUtils;
using HouseImaging;


namespace Documents
{
  class PageFromScanner : Page
  {
    static private int fScanNumber = 1;


    static private string GetTempFileName()
    {
      int number = fScanNumber++;
      string filename = "Scan" + number + ".tmp";
      return AppInfo.GetFullPathToUserApplicationData(filename);
    }


    static public PageFromScanner Create(Image image, int dpi)
    {
      // get a temporary path
      string filename = GetTempFileName();

      ImageTools.SaveImageToFile(image, filename, System.Drawing.Imaging.ImageFormat.Png);

      double pageWidth = image.Width / (double)dpi;
      double pageHeight = image.Height / (double)dpi;

      return new PageFromScanner(new Size2D(pageWidth, pageHeight), filename, dpi);
    }


    private string fFilename;


    public PageFromScanner(Size2D pageSize, string filename, int dpi) : base(pageSize, dpi)
    {
      fFilename = filename;
    }


    public override ImageInfo GetSourceImage()
    {
      return ImageInfo.CreateImageInfo(fFilename);
    }


    public override List<KeyValuePair<string, string>> GetInfoTable()
    {
      List<KeyValuePair<string, string>> result = base.GetInfoTable();

      result.Add(new KeyValuePair<string, string>("Source", "Scanner"));
      result.Add(new KeyValuePair<string, string>("DPI", ResolutionDpi.ToString()));

      return result;
    }


    public override string Name
    {
      get { return System.IO.Path.GetFileNameWithoutExtension(fFilename); }
    }
  }
}
