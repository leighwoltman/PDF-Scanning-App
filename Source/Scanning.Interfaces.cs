using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Defines;


namespace Scanning
{
  public interface InterfaceDataSourceManager
  {
    bool IsOpen { get; }

    bool Open();

    void Close();

    List<InterfaceDataSource> GetDataSources();
  }


  public interface InterfaceDataSource
  {
    string Name { get; }

    DataSourceCapabilities GetCapabilities();

    bool Acquire(DataSourceSettings settings);

    event EventHandler OnNewPictureData;

    event EventHandler OnScanningComplete;
  }


  public class DataSourceSettings
  {
    public bool ShowSettingsUI;
    public bool ShowTransferUI;
    public bool EnableFeeder;
    public ColorModeEnum ColorMode;
    public PageTypeEnum PageType;
    public int Resolution;
    public double Threshold;   // 0 to 1
    public double Brightness;  // 0 to 1
    public double Contrast;    // 0 to 1


    public DataSourceSettings()
    {
      ShowSettingsUI = false;
      ShowTransferUI = true;
      EnableFeeder = false;
      ColorMode = ColorModeEnum.BW;
      PageType = PageTypeEnum.Letter;
      Resolution = 200;
      Threshold = 0.5;
      Brightness = 0.5;
      Contrast = 0.5;
    }
  }


  public class DataSourceCapabilities
  {
    public List<ColorModeEnum> ColorModes;
    public List<PageTypeEnum> PageTypes;
    public List<int> Resolutions;
  }


  public class NewPictureEventArgs : EventArgs
  {
    public Image TheImage;
    public DataSourceSettings TheSettings;
  }
}
