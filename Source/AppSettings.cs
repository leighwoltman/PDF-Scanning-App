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


    public bool EnableFeeder
    {
      get { return fUtilSettings.GetBool("EnableFeeder", true); }
      set { fUtilSettings.SetBool("EnableFeeder", value); }
    }


    public ColorModeEnum ColorMode
    {
      get { return (ColorModeEnum)fUtilSettings.GetInteger("ColorMode", (int)ColorModeEnum.RGB); }
      set { fUtilSettings.SetInteger("ColorMode", (int)value); }
    }


    public PageTypeEnum PageType
    {
      get { return (PageTypeEnum)fUtilSettings.GetInteger("PageType", (int)PageTypeEnum.Letter); }
      set { fUtilSettings.SetInteger("PageType", (int)value); }
    }


    public int Resolution
    {
      get { return fUtilSettings.GetInteger("Resolution", 200); }
      set { fUtilSettings.SetInteger("Resolution", value); }
    }


    public double Threshold
    {
      get { return fUtilSettings.GetDouble("Threshold", 0.5); }
      set { fUtilSettings.SetDouble("Threshold", value); }
    }


    public double Brightness
    {
      get { return fUtilSettings.GetDouble("Brightness", 0.5); }
      set { fUtilSettings.SetDouble("Brightness", value); }
    }


    public double Contrast
    {
      get { return fUtilSettings.GetDouble("Contrast", 0.5); }
      set { fUtilSettings.SetDouble("Contrast", value); }
    }
  }
}
