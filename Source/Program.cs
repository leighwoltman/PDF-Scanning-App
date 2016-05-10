using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Utils;


namespace PDFScanningApp
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      using(Mutex mutex = new Mutex(false, AppInfo.GetApplicationGuid()))
      {
        if(!mutex.WaitOne(0, false))
        {
          MessageBox.Show("An instance of " + AppInfo.GetApplicationName() + " is already running, only one instance is allowed at a time.");
          return;
        }

        string settingsFilename;

        if(args.Length > 0)
        {
          settingsFilename = args[0];
        }
        else
        {
          settingsFilename = Path.Combine(AppInfo.GetUserAppDataFolder(), AppInfo.GetApplicationName(), "Settings.xml");
        }

        AppSettings.Initialize(settingsFilename);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FormMain());
      }
    }
  }
}
