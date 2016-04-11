using System;
using System.Collections.Generic;
using System.Text;


namespace Scanning
{
  partial class TwainDataSourceManager : InterfaceDataSourceManager
  {
    private TwainDevice fTwain;
    private bool fIsOpen;

    public TwainDataSourceManager(IntPtr windowHandle)
    {
      fTwain = new TwainDevice(windowHandle);
    }

    public bool IsOpen
    {
      get { return fIsOpen; }
    }

    public bool Open()
    {
      if(IsOpen == false)
      {
        fTwain.OpenDataSourceManager();
        fIsOpen = true;
      }
      return IsOpen;
    }

    public void Close()
    {
      if(IsOpen)
      {
        fTwain.CloseDataSourceManager();
        fIsOpen = false;
      }
    }

    public List<InterfaceDataSource> GetDataSources()
    {
      List<InterfaceDataSource> result = new List<InterfaceDataSource>();

      if(IsOpen)
      {
        foreach(TwIdentity id in fTwain.GetDataSourceList())
        {
          DataSource ds = new DataSource(fTwain, id);
          result.Add(ds);
        }
      }

      return result;
    }
  }
}
