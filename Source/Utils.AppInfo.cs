using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace Utils
{
  class AppInfo
  {
    static public string GetApplicationName()
    {
      return Application.ProductName;
    }


    static public string GetApplicationVersion()
    {
      return Application.ProductVersion;
    }


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
