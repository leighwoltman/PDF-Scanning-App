using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using Utils;


namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private Mutex fMutex = null;

    protected override void OnStartup(StartupEventArgs e)
    {
      fMutex = new Mutex(false, AppInfo.GetApplicationGuid());

      if(fMutex.WaitOne(0, false))
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

        base.OnStartup(e);
      }
      else
      {
        MessageBox.Show("An instance of " + AppInfo.GetApplicationName() + " is already running, only one instance is allowed at a time.");
        fMutex = null;
        this.Shutdown();
      }
    }


    protected override void OnExit(ExitEventArgs e)
    {
      if(fMutex != null)
      {
        fMutex.ReleaseMutex();
      }

      base.OnExit(e);
    }
  }
}
