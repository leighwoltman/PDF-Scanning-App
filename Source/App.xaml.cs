using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Utils;


namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      string settingsFilename;

      if(e.Args.Length > 0)
      {
        settingsFilename = e.Args[0];
      }
      else
      {
        settingsFilename = System.IO.Path.Combine(AppInfo.GetUserAppDataFolder(), AppInfo.GetApplicationName(), "Settings.xml");
      }

      AppSettings.Initialize(settingsFilename);

      Window wnd = new WindowMain();
      wnd.Show();
    }
  }
}
