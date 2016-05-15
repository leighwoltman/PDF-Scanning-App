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
    static private long fCounter = 0;
    

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


    static public string GetFileName()
    {
      fCounter++;
      return Path.Combine(GetPath(), "tmp" + fCounter.ToString() + ".tmp");
    }
  }
}
