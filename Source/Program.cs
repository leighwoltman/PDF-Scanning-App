using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PDFScanningApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          // first get the settings
          Settings settings = Settings.GetSettings();

          Application.Run(new MainForm(settings));
        }
    }
}
