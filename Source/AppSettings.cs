using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Defines;
using Utils;


namespace PDFScanningApp
{
  public class AppSettings
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
    

    public int ScannerCompressionFactor
    {
      get { return fTable.GetInteger("ScannerCompressionFactor", 80); }
      set { fTable.SetInteger("ScannerCompressionFactor", value); }
    }
    

    public SizeInches DefaultPageSize
    {
      get 
      {
        return new SizeInches(fTable.GetDouble("DefaultPageSizeWidth", 8.5), fTable.GetDouble("DefaultPageSizeHeight", 11));
      }
      set
      {
        fTable.SetDouble("DefaultPageSizeWidth", value.Width);
        fTable.SetDouble("DefaultPageSizeHeight", value.Height);
      }
    }
    

    public SizeInches CustomPageSize
    {
      get
      {
        return new SizeInches(fTable.GetDouble("CustomPageSizeWidth", 7), fTable.GetDouble("CustomPageSizeHeight", 10));
      }
      set
      {
        fTable.SetDouble("CustomPageSizeWidth", value.Width);
        fTable.SetDouble("CustomPageSizeHeight", value.Height);
      }
    }
    

    public PageScalingEnum PageScaling
    {
      get { return (PageScalingEnum)fTable.GetInteger("PageScaling", (int)PageScalingEnum.StretchShrink); }
      set { fTable.SetInteger("PageScaling", (int)value); }
    }
    

    public int PdfViewingResolution
    {
      get { return fTable.GetInteger("PdfViewingResolution", 300); }
      set { fTable.SetInteger("PdfViewingResolution", value); }
    }
    

    public int PdfExportResolution
    {
      get { return fTable.GetInteger("PdfExportResolution", 300); }
      set { fTable.SetInteger("PdfExportResolution", value); }
    }
    

    public bool AlwaysNativePdfImport
    {
      get { return fTable.GetBool("AlwaysNativePdfImport", true); }
      set { fTable.SetBool("AlwaysNativePdfImport", value); }
    }
    

    public bool AttemptPdfSingleImageImport
    {
      get { return fTable.GetBool("AttemptPdfSingleImageImport", true); }
      set { fTable.SetBool("AttemptPdfSingleImageImport", value); }
    }
    

    public bool RemovePagesAfterPdfExport
    {
      get { return fTable.GetBool("RemovePagesAfterPdfExport", true); }
      set { fTable.SetBool("RemovePagesAfterPdfExport", value); }
    }
    

    public int ExportCompressionFactor
    {
      get { return fTable.GetInteger("ExportCompressionFactor", 80); }
      set { fTable.SetInteger("ExportCompressionFactor", value); }
    }

    public bool ShowPrintButton
    {
      get { return fTable.GetBool("ShowPrintButton", false); }
      set { fTable.SetBool("ShowPrintButton", value); }
    }
  }
}
