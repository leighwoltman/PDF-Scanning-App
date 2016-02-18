using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


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

    List<ColorModeEnum> GetAvailableValuesForColorMode();

    List<PageTypeEnum> GetAvailableValuesForPageType();

    List<int> GetAvailableValuesForResolution();

    bool IsOpen { get; }

    bool Open();

    void Close();

    bool Acquire(ScanSettings settings, bool showSettingsUI, bool showTransferUI);

    event NewPictureEventHandler OnNewPictureData;

    event EventHandler OnScanningComplete;
  }


  public enum ColorModeEnum { BW, Gray, RGB };

  public enum PageTypeEnum { Letter, Legal };

  public class ScanSettings
  {
    public bool EnableFeeder;
    public ColorModeEnum ColorMode;
    public PageTypeEnum PageType;
    public int Resolution;
    public double Threshold;   // 0 to 1
    public double Brightness;  // 0 to 1
    public double Contrast;    // 0 to 1


    public ScanSettings()
    {
      EnableFeeder = false;
      ColorMode = ColorModeEnum.BW;
      PageType = PageTypeEnum.Letter;
      Resolution = 200;
      Threshold = 0.5;
      Brightness = 0.5;
      Contrast = 0.5;
    }
  }

  
  public delegate void NewPictureEventHandler(object sender, NewPictureEventArgs e);
  
  
  public class NewPictureEventArgs : EventArgs
  {
    public Image TheImage;
    public ScanSettings TheSettings;
  }
}
