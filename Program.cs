using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WIATest
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

          bool found = false;

          WIA.DeviceManager manager = new WIA.DeviceManager();
          WIA.Device device = null;
          foreach (WIA.DeviceInfo info in manager.DeviceInfos)
          {
            // get the name, look for the scanner
            string name = ((string)info.Properties["Name"].get_Value());

            if (name.Contains("HP Officejet J4500"))
            {
              // worth trying to connect to
              try
              {
                device = info.Connect();
                // we have a match
                settings.SetCurrentScanner(device.DeviceID);
                found = true;
              }
              catch
              {
                // not connected
                // continue around
              }
            }
          }

          if(!found)
          {
            MessageBox.Show("The HP Officejet J4500 scanner could not be found, the application will now close. Is the scanner plugged in and turned on? If not, fix and restart the application.");
          }
          else
          {
            Application.Run(new MainForm(settings));
          }
        }
    }
}
