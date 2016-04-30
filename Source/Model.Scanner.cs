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
  public class ScanCapabilities : Scanning.DataSourceCapabilities { }
  

  class Scanner
  {
    private InterfaceDataSourceManager fTwain;
    private InterfaceDataSourceManager fWia;
    private List<InterfaceDataSource> fDataSources;
    private InterfaceDataSource fActiveDataSource;
    private DataSourceCapabilities fScanCapabilities;
    private Document fDocument;


    public Scanner()
    {
      fTwain = new TwainDataSourceManager();
      fWia = new WiaDataSourceManager();
      fDataSources = null;
      fActiveDataSource = null;
      fScanCapabilities = null;
    }


    public bool IsOpen
    {
      get { return (bool)(fDataSources != null); }
    }


    public bool Open()
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
      return IsOpen;
    }


    public void Close()
    {
      if(IsOpen)
      {
        fTwain.Close();
        fWia.Close();
        fDataSources = null;
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


    public bool SelectActiveDataSource(string name)
    {
      bool success = false;

      if(GetActiveDataSourceName() == name)
      {
        success = true; // Already selected
      }
      else
      {
        InterfaceDataSource ds = GetDataSourceByName(name);

        if(ds != null)
        {
          fScanCapabilities = ds.GetCapabilities();

          if(fScanCapabilities != null)
          {
            fActiveDataSource = ds;
            success = true;
          }
        }
      }

      return success;
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
      return (ScanCapabilities)fScanCapabilities;
    }


    public bool Acquire(Document document, ScanSettings settings)
    {
      bool result = false;

      if(fActiveDataSource != null)
      {
        fDocument = document;

        fActiveDataSource.OnNewPictureData += fActiveDataSource_OnNewPictureData;
        fActiveDataSource.OnScanningComplete += fActiveDataSource_OnScanningComplete;

        result = fActiveDataSource.Acquire(settings);
      }

      return result;
    }


    private void fActiveDataSource_OnNewPictureData(object sender, EventArgs e)
    {
      NewPictureEventArgs args = (NewPictureEventArgs)e;
      Page myPage = new PageFromScanner(args.TheImage, args.TheSettings.Resolution);
      fDocument.AddPage(myPage);
    }


    private void fActiveDataSource_OnScanningComplete(object sender, EventArgs e)
    {
      Raise_OnScanningComplete();
    }


    public event EventHandler OnScanningComplete;

    private void Raise_OnScanningComplete()
    {
      if(OnScanningComplete != null)
      {
        OnScanningComplete(this, EventArgs.Empty);
      }
    }
  }
}
