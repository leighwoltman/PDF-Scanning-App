using System;
using System.Collections.Generic;
using System.IO;
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
      string settingsFilename;

      if(args.Length > 0)
      {
        settingsFilename = args[0];
      }
      else
      {
        settingsFilename = Path.Combine(AppInfo.GetUserAppDataFolder(), Application.ProductName, "Settings.xml");
      }

      AppSettings.Initialize(settingsFilename);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new FormMain());
    }
  }
}
