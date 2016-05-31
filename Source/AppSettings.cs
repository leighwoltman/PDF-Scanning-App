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


    public bool ScannerUseNativeUI
    {
      get { return fTable.GetBool("UseScannerNativeUI", false); }
      set { fTable.SetBool("UseScannerNativeUI", value); }
    }


    public bool ScannerEnableFeeder
    {
      get { return fTable.GetBool("ScannerEnableFeeder", true); }
      set { fTable.SetBool("ScannerEnableFeeder", value); }
    }


    public ColorModeEnum ScannerColorMode
    {
      get { return (ColorModeEnum)fTable.GetInteger("ScannerColorMode", (int)ColorModeEnum.RGB); }
      set { fTable.SetInteger("ScannerColorMode", (int)value); }
    }


    public int ScannerResolution
    {
      get { return fTable.GetInteger("ScannerResolution", 200); }
      set { fTable.SetInteger("ScannerResolution", value); }
    }


    public double ScannerThreshold
    {
      get { return fTable.GetDouble("ScannerThreshold", 0.5); }
      set { fTable.SetDouble("ScannerThreshold", value); }
    }


    public double ScannerBrightness
    {
      get { return fTable.GetDouble("ScannerBrightness", 0.5); }
      set { fTable.SetDouble("ScannerBrightness", value); }
    }


    public double ScannerContrast
    {
      get { return fTable.GetDouble("ScannerContrast", 0.5); }
      set { fTable.SetDouble("ScannerContrast", value); }
    }
    

    public int ScannerCompressionFactor
    {
      get { return fTable.GetInteger("ScannerCompressionFactor", 80); }
      set { fTable.SetInteger("ScannerCompressionFactor", value); }
    }


    public SizeInches ScannerCustomPageSize
    {
      get
      {
        return new SizeInches(fTable.GetDouble("ScannerCustomPageSizeWidth", 7), fTable.GetDouble("ScannerCustomPageSizeHeight", 10));
      }
      set
      {
        fTable.SetDouble("ScannerCustomPageSizeWidth", value.Width);
        fTable.SetDouble("ScannerCustomPageSizeHeight", value.Height);
      }
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


    public bool ExportCompressImages
    {
      get { return fTable.GetBool("ExportCompressImages", true); }
      set { fTable.SetBool("ExportCompressImages", value); }
    }


    public int ExportCompressionFactor
    {
      get { return fTable.GetInteger("ExportCompressionFactor", 80); }
      set { fTable.SetInteger("ExportCompressionFactor", value); }
    }


    public string LastDirectoryForSaving
    {
      get { return fTable.Get("LastDirectoryForSaving", ""); }
      set { fTable.Set("LastDirectoryForSaving", value); }
    }


    public string LastDirectoryForLoading
    {
      get { return fTable.Get("LastDirectoryForLoading", ""); }
      set { fTable.Set("LastDirectoryForLoading", value); }
    }


    public bool ShowPrintButton
    {
      get { return fTable.GetBool("ShowPrintButton", false); }
      set { fTable.SetBool("ShowPrintButton", value); }
    }
  }
}
