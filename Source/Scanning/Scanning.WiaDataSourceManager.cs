using System;
using System.Collections.Generic;
using System.Text;


namespace Scanning
{
  partial class WiaDataSourceManager : InterfaceDataSourceManager
  {
    private WIA.DeviceManager fWia;

    public WiaDataSourceManager()
    {
      fWia = null;
    }

    public bool IsOpen
    {
      get { return (bool)(fWia != null); }
    }

    public bool Open()
    {
      if(IsOpen == false)
      {
        fWia = new WIA.DeviceManager();
      }
      return IsOpen;
    }

    public void Close()
    {
      fWia = null;
    }

    public List<InterfaceDataSource> GetDataSources()
    {
      List<InterfaceDataSource> result = new List<InterfaceDataSource>();

      if(IsOpen)
      {
        foreach(WIA.DeviceInfo info in fWia.DeviceInfos)
        {
          if(info.Type == WIA.WiaDeviceType.ScannerDeviceType)
          {
            WiaDataSource ds = new WiaDataSource(info);
            result.Add(ds);
          }
        }
      }

      return result;
    }
  }
}
