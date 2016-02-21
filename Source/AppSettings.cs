using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utils;


namespace PDFScanningApp
{
  class AppSettings
  {
    static private UtilSettings fUtilSettings = null;


    static public void Initialize(string filename)
    {
      fUtilSettings = new UtilSettings(filename);
    }


    public string CurrentScanner
    {
      get { return fUtilSettings.Get("CurrentScanner", ""); }
      set { fUtilSettings.Set("CurrentScanner", value); }
    }


    public bool UseScannerNativeUI
    {
      get { return fUtilSettings.GetBool("UseScannerNativeUI", false); }
      set { fUtilSettings.SetBool("UseScannerNativeUI", value); }
    }


    public string LastDirectory
    {
      get { return fUtilSettings.Get("LastDirectory", ""); }
      set { fUtilSettings.Set("LastDirectory", value); }
    }
  }
}
