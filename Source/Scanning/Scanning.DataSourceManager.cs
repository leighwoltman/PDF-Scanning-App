using System;
using System.Collections.Generic;


namespace Scanning
{
  public class ScanCapabilities : Scanning.DataSourceCapabilities 
  {
    public ScanCapabilities(InterfaceDataSource ds)
    {
      DataSourceCapabilities cap = ds.GetCapabilities();
      this.ColorModes = cap.ColorModes;
      this.PageTypes = cap.PageTypes;
      this.Resolutions = cap.Resolutions;
    }
  }
  

  public class DataSourceManager
  {
    private InterfaceDataSourceManager fTwain;
    private InterfaceDataSourceManager fWia;
    private List<InterfaceDataSource> fDataSources;
    private InterfaceDataSource fActiveDataSource;


    public DataSourceManager(IntPtr windowHandle)
    {
      fTwain = new TwainDataSourceManager(windowHandle);
      fWia = new WiaDataSourceManager();
      fDataSources = null;
      fActiveDataSource = null;
    }


    public bool IsOpen
    {
      get { return (bool)(fDataSources != null); }
    }


    public delegate void OpenCallback(bool success);

    public void Open(OpenCallback callback)
    {
      if(IsOpen == false)
      {
        fDataSources = new List<InterfaceDataSource>();

        if(fTwain.Open())
        {
          fDataSources.AddRange(fTwain.GetDataSources());
        }
        if(fWia.Open())
        {
          fDataSources.AddRange(fWia.GetDataSources());
        }
      }

      if(callback != null)
      {
        callback(IsOpen);
      }
    }


    public delegate void CloseCallback();

    public void Close(CloseCallback callback)
    {
      if(IsOpen)
      {
        fTwain.Close();
        fWia.Close();
        fDataSources = null;
      }

      if(callback != null)
      {
        callback();
      }
    }


    public List<string> GetDataSourceNames()
    {
      List<string> result = new List<string>();

      if(IsOpen)
      {
        foreach(InterfaceDataSource ds in fDataSources)
        {
          result.Add(ds.Name);
        }
      }

      return result;
    }

    
    public string GetActiveDataSourceName()
    {
      string result = null;

      if(fActiveDataSource != null)
      {
        result = fActiveDataSource.Name;
      }

      return result;
    }


    public void SetActiveDataSource(string name)
    {
      InterfaceDataSource ds = GetDataSourceByName(name);

      if(ds != null)
      {
        fActiveDataSource = ds;
      }
    }


    private InterfaceDataSource GetDataSourceByName(string name)
    {
      InterfaceDataSource result = null;

      if(IsOpen)
      {
        foreach(InterfaceDataSource ds in fDataSources)
        {
          if(ds.Name == name)
          {
            result = ds;
          }
        }
      }

      return result;
    }


    public ScanCapabilities GetActiveDataSourceCapabilities()
    {
      ScanCapabilities result = null;

      if(fActiveDataSource != null)
      {
        result = new ScanCapabilities(fActiveDataSource);
      }

      return result;
    }


    public void Acquire(DataSourceSettings settings, AcquireCallback callback)
    {
      if(fActiveDataSource != null)
      {
        if(fActiveDataSource.Acquire(settings, callback) == false)
        {
          callback(null);
        }
      }
    }
  }
}
