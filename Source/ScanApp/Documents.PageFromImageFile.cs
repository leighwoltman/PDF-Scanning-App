using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseUtils;
using HouseImaging;


namespace Documents
{
  class PageFromImageFile : Page
  {
    static public PageFromImageFile Create(Size2D pageSize, string filename)
    {
      return new PageFromImageFile(pageSize, filename);
    }


    private string fFilename;


    public PageFromImageFile(Size2D pageSize, string filename) : base(pageSize)
    {
      fFilename = filename;
    }


    public override ImageInfo GetSourceImage()
    {
      return ImageInfo.FromFile(fFilename);
    }


    public override List<KeyValuePair<string, string>> GetInfoTable()
    {
      List<KeyValuePair<string, string>> result = base.GetInfoTable();

      result.Add(new KeyValuePair<string, string>("Source", "File"));
      result.Add(new KeyValuePair<string, string>("File", System.IO.Path.GetFileName(fFilename)));

      return result;
    }


    public override string Name
    {
      get { return System.IO.Path.GetFileNameWithoutExtension(fFilename); }
    }
  }
}
