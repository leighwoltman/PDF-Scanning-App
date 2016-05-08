using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Scanning;
using Defines;


namespace Model
{
  public class ScanSettings : Scanning.DataSourceSettings { }


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
  

  class Scanner
  {
    private InterfaceDataSourceManager fTwain;
    private InterfaceDataSourceManager fWia;
    private List<InterfaceDataSource> fDataSources;
    private InterfaceDataSource fActiveDataSource;
    private Document fDocument;


    public Scanner()
    {
      fTwain = new TwainDataSourceManager();
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

        foreach(InterfaceDataSource ds in fDataSources)
        {
          ds.OnNewPictureData += fActiveDataSource_OnNewPictureData;
          ds.OnScanningComplete += fActiveDataSource_OnScanningComplete;
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


    public delegate void AcquireCallback(bool success);

    public void Acquire(Document document, ScanSettings settings, AcquireCallback callback)
    {
      if(fActiveDataSource != null)
      {
        fDocument = document;

        if(fActiveDataSource.Acquire(settings) == false)
        {
          Raise_OnScanningComplete(false);
        }
      }
    }


    private void fActiveDataSource_OnNewPictureData(object sender, EventArgs e)
    {
      NewPictureEventArgs args = (NewPictureEventArgs)e;
      Page myPage = new PageFromScanner(args.TheImage, args.TheSettings.Resolution);
      fDocument.AddPage(myPage);
    }


    private void fActiveDataSource_OnScanningComplete(object sender, EventArgs e)
    {
      Raise_OnScanningComplete(true);
    }


    private AcquireCallback OnScanningComplete;

    private void Raise_OnScanningComplete(bool success)
    {
      if(OnScanningComplete != null)
      {
        OnScanningComplete(success);
      }
    }
  }
}
