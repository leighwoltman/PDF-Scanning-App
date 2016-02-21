using System;
using System.Collections.Generic;
using System.Text;


namespace Utils
{
  class UtilApp
  {
    static public string GetUserAppDataFolder()
    {
      return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }

    static public string GetCommonAppDataFolder()
    {
      return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    }
  }
}
