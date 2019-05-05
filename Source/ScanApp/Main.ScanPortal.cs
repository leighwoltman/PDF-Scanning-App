using System.Collections.Generic;
using System.Drawing;
using Scanning;
using HouseUtils;
using HouseUtils.Threading;


namespace ScanApp
{
  public class ScanSettings
  {
    public ColorModeEnum ColorMode { get; set; }
    public int Resolution { get; set; }  // dpi
    public int Threshold { get; set; }   // %
    public int Brightness { get; set; }  // %
    public int Contrast { get; set; }    // %

    public ScanSettings Copy()
    {
      return new ScanSettings()
      {
        ColorMode = this.ColorMode,
        Resolution = this.Resolution,
        Brightness = this.Brightness,
        Contrast = this.Contrast,
        Threshold = this.Threshold
      };
    }

    static public ScanSettings Default
    {
      get
      {
        return new ScanSettings()
        {
          ColorMode = ColorModeEnum.BW,
          Resolution = 200,
          Brightness = 50,
          Contrast = 50,
          Threshold = 50
        };
      }
    }
  }


  public class ScanCapabilities
  {
    public List<ColorModeEnum> ColorModes { get; set; }
    public List<string> PageTypes { get; set; }
    public List<int> Resolutions { get; set; }
  }


  class ScanPortal
  {
    private DispatcherUI fUI;
    private DispatcherScanner fThread;
    private DataSourceManager fDataSourceManager;


    public ScanPortal()
    {
      fUI = new DispatcherUI();
      fThread = new DispatcherScanner();
    }


    public delegate void OpenCallback(List<string> dataSourceList);
    public delegate void AcquireCallback(Image image);
    public delegate void CapabilitiesCallback(ScanCapabilities result);


    public void Open(OpenCallback callback)
    {
      fThread.Post(()=> 
      {
        fDataSourceManager = new DataSourceManager(fThread.GetWindowHandle());

        fDataSourceManager.Open((success) =>
        {
          List<string> list = null;

          if (success)
          {
            list = fDataSourceManager.GetDataSourceNames();
          }

          fUI.Post(callback, list);
        });
      });
    }


    public void Terminate()
    {
      fThread.Stop();
    }


    public void GetCapabilities(string dataSource, CapabilitiesCallback callback)
    {
      fThread.Post(() =>
      {
        fDataSourceManager.SetActiveDataSource(dataSource);
        Scanning.ScanCapabilities cap = fDataSourceManager.GetActiveDataSourceCapabilities();

        List<ColorModeEnum> colorModes = new List<ColorModeEnum>();

        foreach (ColorModeEnum e in cap.ColorModes)
        {
          colorModes.Add(e);
        }

        List<string> pageTypes = new List<string>();

        foreach (PageTypeEnum e in cap.PageTypes)
        {
          pageTypes.Add(e.ToString());
        }

        fUI.Post(callback, new ScanCapabilities() { ColorModes = colorModes, PageTypes = pageTypes, Resolutions = cap.Resolutions });
      });
    }


    public void Acquire(string dataSource, Bounds scanArea, bool enableDuplex, ScanSettings settings, AcquireCallback callback)
    {
      DataSourceSettings dsSettings = new DataSourceSettings()
      {
        ShowSettingsUI = false,
        ShowTransferUI = false,
        EnableFeeder = true,
        EnableDuplex = enableDuplex,
        ScanArea = scanArea,
        ColorMode = settings.ColorMode,
        Resolution = settings.Resolution,
        Threshold = settings.Threshold,
        Brightness = settings.Brightness,
        Contrast = settings.Contrast
      };


      fThread.Post(() => 
      {
        fDataSourceManager.SetActiveDataSource(dataSource);
        fDataSourceManager.Acquire(dsSettings, (image) => 
        {
          fUI.Post(callback, image);
        });
      });
    }
  }
}
