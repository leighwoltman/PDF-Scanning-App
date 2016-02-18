using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace TwainInterface
{
  public partial class TwainDataSourceManager
  {
    private class TwainDevice
    {
      private IntPtr fWindowHandle;
      private TwIdentity fApplicationId;


      public TwainDevice(IntPtr windowHandle)
      {
        fWindowHandle = windowHandle;

        fApplicationId = new TwIdentity();
        fApplicationId.Id = IntPtr.Zero;
        fApplicationId.Version.MajorNum = 1;
        fApplicationId.Version.MinorNum = 1;
        fApplicationId.Version.Language = Language.USA;
        fApplicationId.Version.Country = Country.USA;
        fApplicationId.Version.Info = "TwainLibrary";
        fApplicationId.ProtocolMajor = TwProtocol.Major;
        fApplicationId.ProtocolMinor = TwProtocol.Minor;
        fApplicationId.SupportedGroups = (int)(TwDG.Image | TwDG.Control);
        fApplicationId.Manufacturer = "TwainLibrary";
        fApplicationId.ProductFamily = "TwainLibrary";
        fApplicationId.ProductName = "TwainLibrary";
      }


      public bool OpenDataSourceManager()
      {
        TwRC rc = LibTwain32.DSMparent(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.OpenDSM, ref fWindowHandle);
        return (bool)(rc == TwRC.Success);
      }


      public bool CloseDataSourceManager()
      {
        TwRC rc = LibTwain32.DSMparent(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref fWindowHandle);
        return (bool)(rc == TwRC.Success);
      }


      public bool GetDataSourceManagerStatus(TwStatus result)
      {
        TwRC rc = LibTwain32.DSMstatus(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Status, TwMSG.Get, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwStatus GetDataSourceManagerStatus()
      {
        TwStatus result = new TwStatus();
        TwRC rc = LibTwain32.DSMstatus(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Status, TwMSG.Get, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool GetDefaultDataSource(TwIdentity result)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwIdentity GetDefaultDataSource()
      {
        TwIdentity result = new TwIdentity();
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool GetFirstDataSource(TwIdentity result)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetFirst, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwIdentity GetFirstDataSource()
      {
        TwIdentity result = new TwIdentity();
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetFirst, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool GetNextDataSource(TwIdentity result)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetNext, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwIdentity GetNextDataSource()
      {
        TwIdentity result = new TwIdentity();
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetNext, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public List<TwIdentity> GetDataSourceList()
      {
        List<TwIdentity> result = new List<TwIdentity>();

        TwIdentity ds = GetFirstDataSource();

        while(ds != null)
        {
          result.Add(ds);
          ds = GetNextDataSource();
        }

        return result;
      }


      public bool UserSelectDataSource(TwIdentity result)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwIdentity UserSelectDataSource()
      {
        TwIdentity result = new TwIdentity();
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool OpenDataSource(TwIdentity dataSourceId)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.OpenDS, dataSourceId);
        return (bool)(rc == TwRC.Success);
      }


      public bool CloseDataSource(TwIdentity dataSourceId)
      {
        TwRC rc = LibTwain32.DSMident(fApplicationId, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDS, dataSourceId);
        return (bool)(rc == TwRC.Success);
      }


      public bool SetDataSourceCapability(TwIdentity dataSourceId, TwCapability cap)
      {
        TwRC rc = LibTwain32.DScap(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Capability, TwMSG.Set, cap);
        return (bool)(rc == TwRC.Success);
      }


      public bool SetDataSourceCapability(TwIdentity dataSourceId, TwCap capType, TwType valueType, object value)
      {
        TwCapability cap = new TwCapability(capType, valueType, value);
        return SetDataSourceCapability(dataSourceId, cap);
      }


      public bool GetDataSourceCapability(TwIdentity dataSourceId, TwCapability result)
      {
        TwRC rc = LibTwain32.DScap(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Capability, TwMSG.GetCurrent, result);
        return (bool)(rc == TwRC.Success);
      }


      public object GetDataSourceCapability(TwIdentity dataSourceId, TwCap capType)
      {
        object result = null;
        TwCapability cap = new TwCapability(capType);

        if(GetDataSourceCapability(dataSourceId, cap))
        {
          result = cap.GetCurrentValue();
        }
        return result;
      }


      public bool GetDataSourceAvailableCapabilityValues(TwIdentity dataSourceId, TwCapability result)
      {
        TwRC rc = LibTwain32.DScap(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Capability, TwMSG.Get, result);
        return (bool)(rc == TwRC.Success);
      }


      public bool GetDataSourceStatus(TwIdentity dataSourceId, TwStatus result)
      {
        TwRC rc = LibTwain32.DSstatus(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Status, TwMSG.Get, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwStatus GetDataSourceStatus(TwIdentity dataSourceId)
      {
        TwStatus result = new TwStatus();
        TwRC rc = LibTwain32.DSstatus(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Status, TwMSG.Get, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public TwCustomDsData GetDataSourceCustomData(TwIdentity dataSourceId)
      {
        TwCustomDsData result = new TwCustomDsData();
        TwRC rc = LibTwain32.DScustomData(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Status, TwMSG.Get, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool StartDataSession(TwIdentity dataSourceId, bool showDialog, bool showTransfer)
      {
        TwUserInterface guif = new TwUserInterface();
        guif.ShowUI = (short)(showDialog ? 1 : 0);
        guif.ModalUI = 1;
        guif.ParentHand = showTransfer ? fWindowHandle : IntPtr.Zero;
        TwRC rc = LibTwain32.DSuserif(fApplicationId, dataSourceId, TwDG.Control, TwDAT.UserInterface, TwMSG.EnableDS, guif);
        return (bool)(rc == TwRC.Success);
      }


      public bool FinishDataSession(TwIdentity dataSourceId)
      {
        TwUserInterface guif = new TwUserInterface(); // Contents are not used during DisableDS
        TwRC rc = LibTwain32.DSuserif(fApplicationId, dataSourceId, TwDG.Control, TwDAT.UserInterface, TwMSG.DisableDS, guif);
        return (bool)(rc == TwRC.Success);
      }


      public bool GetImageInfo(TwIdentity dataSourceId, TwImageInfo result)
      {
        TwRC rc = LibTwain32.DSiinf(fApplicationId, dataSourceId, TwDG.Image, TwDAT.ImageInfo, TwMSG.Get, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwImageInfo GetImageInfo(TwIdentity dataSourceId)
      {
        TwImageInfo result = new TwImageInfo();
        TwRC rc = LibTwain32.DSiinf(fApplicationId, dataSourceId, TwDG.Image, TwDAT.ImageInfo, TwMSG.Get, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool TransferImage(TwIdentity dataSourceId, ref IntPtr hresultBitmap)
      {
        TwRC rc = LibTwain32.DSixfer(fApplicationId, dataSourceId, TwDG.Image, TwDAT.ImageNativeXfer, TwMSG.Get, ref hresultBitmap);
        return (bool)(rc == TwRC.XferDone);
      }


      public bool GetNumberOfPendingTransfers(TwIdentity dataSourceId, TwPendingXfers result)
      {
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.Get, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwPendingXfers GetNumberOfPendingTransfers(TwIdentity dataSourceId)
      {
        TwPendingXfers result = new TwPendingXfers();
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.Get, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool TransferEnd(TwIdentity dataSourceId, TwPendingXfers result)
      {
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwPendingXfers TransferEnd(TwIdentity dataSourceId)
      {
        TwPendingXfers result = new TwPendingXfers();
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool DiscardPendingTransfers(TwIdentity dataSourceId, TwPendingXfers result)
      {
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, result);
        return (bool)(rc == TwRC.Success);
      }


      public TwPendingXfers DiscardPendingTransfers(TwIdentity dataSourceId)
      {
        TwPendingXfers result = new TwPendingXfers();
        TwRC rc = LibTwain32.DSpxfer(fApplicationId, dataSourceId, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, result);
        return (rc == TwRC.Success) ? result : null;
      }


      public bool ProcessEvent(TwIdentity dataSourceId, ref TwEvent result)
      {
        TwRC rc = LibTwain32.DSevent(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Event, TwMSG.ProcessEvent, ref result);
        return (bool)(rc != TwRC.NotDSEvent);
      }


      public bool ProcessEvent(TwIdentity dataSourceId, ref WINMSG message, ref TwMSG result)
      {
        TwEvent evtmsg;
        evtmsg.Message = 0;
        evtmsg.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WINMSG)));
        Marshal.StructureToPtr(message, evtmsg.EventPtr, false);

        TwRC rc = LibTwain32.DSevent(fApplicationId, dataSourceId, TwDG.Control, TwDAT.Event, TwMSG.ProcessEvent, ref evtmsg);

        Marshal.FreeHGlobal(evtmsg.EventPtr);
        result = evtmsg.Message;

        return (bool)(rc == TwRC.DSEvent);
      }
    }
  }
}