using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Utils
{
  class TempFolder
  {
    static private string fTempFolder = null;
    

    static public string GetPath()
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
  }
}
