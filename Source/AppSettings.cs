using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Defines;
using Utils;


namespace PDFScanningApp
{
  class AppSettings
  {
    static private SettingsTable fTable = null;


    static public void Initialize(string filename)
    {
      fTable = new SettingsTable(filename);
    }


    public string CurrentScanner
    {
      get { return fTable.Get("CurrentScanner", ""); }
      set { fTable.Set("CurrentScanner", value); }
    }


    public bool UseScannerNativeUI
    {
      get { return fTable.GetBool("UseScannerNativeUI", false); }
      set { fTable.SetBool("UseScannerNativeUI", value); }
    }


    public string LastDirectory
    {
      get { return fTable.Get("LastDirectory", ""); }
      set { fTable.Set("LastDirectory", value); }
    }


    public bool EnableFeeder
    {
      get { return fTable.GetBool("EnableFeeder", true); }
      set { fTable.SetBool("EnableFeeder", value); }
    }


    public ColorModeEnum ColorMode
    {
      get { return (ColorModeEnum)fTable.GetInteger("ColorMode", (int)ColorModeEnum.RGB); }
      set { fTable.SetInteger("ColorMode", (int)value); }
    }


    public PageTypeEnum PageType
    {
      get { return (PageTypeEnum)fTable.GetInteger("PageType", (int)PageTypeEnum.Letter); }
      set { fTable.SetInteger("PageType", (int)value); }
    }


    public int Resolution
    {
      get { return fTable.GetInteger("Resolution", 200); }
      set { fTable.SetInteger("Resolution", value); }
    }


    public double Threshold
    {
      get { return fTable.GetDouble("Threshold", 0.5); }
      set { fTable.SetDouble("Threshold", value); }
    }


    public double Brightness
    {
      get { return fTable.GetDouble("Brightness", 0.5); }
      set { fTable.SetDouble("Brightness", value); }
    }


    public double Contrast
    {
      get { return fTable.GetDouble("Contrast", 0.5); }
      set { fTable.SetDouble("Contrast", value); }
    }
  }
}
