using System;
using System.Collections.Generic;
using System.Text;
using Twain;


namespace Scanning
{
  partial class TwainDataSourceManager : InterfaceDataSourceManager
  {
    private Portal fTwain;

    public TwainDataSourceManager(IntPtr windowHandle)
    {
      fTwain = new Portal(windowHandle);
    }

    public bool IsOpen { get; private set; }

    public bool Open()
    {
      if(IsOpen == false)
      {
        IsOpen = fTwain.OpenDataSourceManager();
      }
      return IsOpen;
    }

    public void Close()
    {
      if(IsOpen)
      {
        fTwain.CloseDataSourceManager();
        IsOpen = false;
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
