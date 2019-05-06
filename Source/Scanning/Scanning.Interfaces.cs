using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HouseUtils;


namespace Scanning
{
  public enum ColorModeEnum { BW, Gray, RGB };

  public enum PageTypeEnum { Custom, Letter, Legal };

  public class EnumUtils
  {
    public static ColorModeEnum ColorModeEnumFromString(string name)
    {
      ColorModeEnum result = ColorModeEnum.BW;

      List<ColorModeEnum> elist = Enum.GetValues(typeof(ColorModeEnum)).Cast<ColorModeEnum>().ToList();

      foreach (ColorModeEnum e in elist)
      {
        if (e.ToString() == name)
        {
          result = e;
        }
      }

      return result;
    }

    public static PageTypeEnum PageTypeEnumFromString(string name)
    {
      PageTypeEnum result = PageTypeEnum.Letter;

      List<PageTypeEnum> elist = Enum.GetValues(typeof(PageTypeEnum)).Cast<PageTypeEnum>().ToList();

      foreach (PageTypeEnum e in elist)
      {
        if (e.ToString() == name)
        {
          result = e;
        }
      }

      return result;
    }
  }
  public interface InterfaceDataSourceManager
  {
    bool IsOpen { get; }
    bool Open();
    void Close();
    List<InterfaceDataSource> GetDataSources();
  }


  public delegate void AcquireCallback(Image image);


  public interface InterfaceDataSource
  {
    string Name { get; }
    DataSourceCapabilities GetCapabilities();
    bool Acquire(DataSourceSettings settings, AcquireCallback callback);
  }


  public class DataSourceSettings
  {
    public bool ShowSettingsUI { get; set; }
    public bool ShowTransferUI { get; set; }
    public bool EnableFeeder { get; set; }
    public bool EnableDuplex { get; set; }
    public ColorModeEnum ColorMode { get; set; }
    public Bounds ScanArea { get; set; }
    public int Resolution { get; set; }  // dpi
    public int Threshold { get; set; }   // % 0 black, 100 white
    public int Brightness { get; set; }  // %
    public int Contrast { get; set; }    // %
  }


  public class DataSourceCapabilities
  {
    public List<ColorModeEnum> ColorModes;
    public List<PageTypeEnum> PageTypes;
    public List<int> Resolutions;
  }
}
