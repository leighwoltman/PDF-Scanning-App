using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Interop;
using System.Threading.Tasks;
using Twain;
using NativeLibs;
using HouseUtils;
using WINMSG = NativeLibs.LibDefines.WINMSG;


namespace Scanning
{
  partial class TwainDataSourceManager
  {
    private class DataSource : InterfaceDataSource
    {
      private enum StateType { Closed = 0, Idle, Active };

      private Portal fTwain;
      private TwIdentity fIdent;
      private StateType fState;
      private DataSourceSettings fSettings;
      private DataSourceCapabilities fCapabilities;


      public DataSource(Portal twain, TwIdentity identity)
      {
        fTwain = twain;
        fIdent = identity;
        fState = StateType.Closed;
        fSettings = null;
        fCapabilities = null;
      }


      public string Name
      {
        get { return "TWAIN I/F: " + fIdent.ProductName; }
      }

      
      private TwainCapability fCapabilitySupportedTypes;
      private TwainCapability fCapabilityPixelType;
      private TwainCapability fCapabilityPageSize;
      private TwainCapability fCapabilityResolutionX;
      private TwainCapability fCapabilityResolutionY;
      private TwainCapability fCapabilityFeederEnabled;
      private TwainCapability fCapabilityFeederLoaded;
      private TwainCapability fCapabilityDuplex;
      private TwainCapability fCapabilityDuplexEnabled;
      private TwainCapability fCapabilityBitDepthReduction;
      private TwainCapability fCapabilityThreshold;
      private TwainCapability fCapabilityBrightness;
      private TwainCapability fCapabilityContrast;


      public DataSourceCapabilities GetCapabilities()
      {
        if(fCapabilities == null)
        {
          if(this.Open())
          {
            fCapabilities = new DataSourceCapabilities();

            fCapabilities.ColorModes = GetAvailableValuesForColorMode();
            fCapabilities.PageTypes = GetAvailableValuesForPageType();
            fCapabilities.Resolutions = GetAvailableValuesForResolution();

            this.Close();
          }
        }

        return fCapabilities;
      }


      private List<ColorModeEnum> GetAvailableValuesForColorMode()
      {
        List<ColorModeEnum> result = new List<ColorModeEnum>();

        foreach(object item in fCapabilityPixelType.Items)
        {
          switch((TwCapPixelType)item)
          {
            case TwCapPixelType.BW: result.Add(ColorModeEnum.BW); break;
            case TwCapPixelType.RGB: result.Add(ColorModeEnum.RGB); break;
            case TwCapPixelType.Gray: result.Add(ColorModeEnum.Gray); break;
            default: break; // Unused capability
          }
        }

        return result;
      }


      private List<PageTypeEnum> GetAvailableValuesForPageType()
      {
        List<PageTypeEnum> result = new List<PageTypeEnum>();

        foreach(object item in fCapabilityPageSize.Items)
        {
          switch((TwCapPageType)item)
          {
            case TwCapPageType.UsLetter: result.Add(PageTypeEnum.Letter); break;
            case TwCapPageType.UsLegal: result.Add(PageTypeEnum.Legal); break;
            default: break; // Unused capability
          }
        }

        return result;
      }


      private List<int> GetAvailableValuesForResolution()
      {
        List<int> result = new List<int>();

        foreach(object item in fCapabilityResolutionX.Items)
        {
          result.Add((int)(float)item);
        }

        return result;
      }


      private bool IsOpen
      {
        get { return (bool)(fState > StateType.Closed); }
      }


      private bool Open()
      {
        if(fTwain.OpenDataSource(fIdent))
        {
          fState = StateType.Idle;

          fCapabilitySupportedTypes = new TwainCapability(fTwain, fIdent, TwCap.SupportedCapabilities);
          fCapabilityPixelType = new TwainCapability(fTwain, fIdent, TwCap.IPixelType);
          fCapabilityPageSize = new TwainCapability(fTwain, fIdent, TwCap.SupportedSizes);
          fCapabilityResolutionX = new TwainCapability(fTwain, fIdent, TwCap.XResolution);
          fCapabilityResolutionY = new TwainCapability(fTwain, fIdent, TwCap.YResolution);
          fCapabilityFeederEnabled = new TwainCapability(fTwain, fIdent, TwCap.FeederEnabled);
          fCapabilityFeederLoaded = new TwainCapability(fTwain, fIdent, TwCap.FeederLoaded);
          fCapabilityDuplex = new TwainCapability(fTwain, fIdent, TwCap.Duplex);
          fCapabilityDuplexEnabled = new TwainCapability(fTwain, fIdent, TwCap.DuplexEnabled);
          fCapabilityBitDepthReduction = new TwainCapability(fTwain, fIdent, TwCap.BitDepthReduction);
          fCapabilityThreshold = new TwainCapability(fTwain, fIdent, TwCap.Threshold);
          fCapabilityBrightness = new TwainCapability(fTwain, fIdent, TwCap.Brightness);
          fCapabilityContrast = new TwainCapability(fTwain, fIdent, TwCap.Contrast);
        }
        return IsOpen;
      }


      private void Close()
      {
        if(IsOpen)
        {
          fTwain.CloseDataSource(fIdent);
          fState = StateType.Closed;
        }
      }


      private bool IsActive()
      {
        return (bool)(fState == StateType.Active);
      }


      public bool Acquire(DataSourceSettings settings, AcquireCallback callback)
      {
        bool success = false;

        if(this.Open())
        {
          fSettings = settings;

          if(fSettings != null)
          {
            // Setup Pixel Type
            TwCapPixelType pixelType;

            switch(fSettings.ColorMode)
            {
              case ColorModeEnum.BW: pixelType = TwCapPixelType.BW; break;
              case ColorModeEnum.RGB: pixelType = TwCapPixelType.RGB; break;
              case ColorModeEnum.Gray: pixelType = TwCapPixelType.Gray; break;
              default: pixelType = TwCapPixelType.BW; break;
            }

            fCapabilityPixelType.CurrentValue = pixelType;

#if USE_PAGE_TYPE
            // Page Type Setting is not used anymore; Image Layout is defined instead
            TwCapPageType pageType;

            switch(fSettings.PageType)
            {
              case PageTypeEnum.Letter: pageType = TwCapPageType.UsLetter; break;
              case PageTypeEnum.Legal: pageType = TwCapPageType.UsLegal; break;
              default: pageType = TwCapPageType.UsLetter; break;
            }

            fCapabilityPageSize.CurrentValue = pageType;
#else
            // Setup Scan Area
            TwImageLayout ilayout = new TwImageLayout();

            if(fTwain.GetImageLayout(fIdent, ilayout))
            {
              Bounds scanArea = settings.ScanArea;

              ilayout.Frame.Left = (float)scanArea.X;
              ilayout.Frame.Top = (float)scanArea.Y;
              ilayout.Frame.Right = (float)(scanArea.X + scanArea.Width);
              ilayout.Frame.Bottom = (float)(scanArea.Y + scanArea.Height);
              fTwain.SetImageLayout(fIdent, ilayout);
            }
#endif

            // Setup Resolution
            float resolution = (float)fSettings.Resolution;

            fCapabilityResolutionX.CurrentValue = resolution;
            fCapabilityResolutionY.CurrentValue = resolution;

            // Enable/Disable Document Feeder
            if(fSettings.EnableFeeder)
            {
              fCapabilityFeederEnabled.CurrentValue = fCapabilityFeederLoaded.CurrentValue;
            }
            else
            {
              fCapabilityFeederEnabled.CurrentValue = false;
            }

            if (((UInt16)fCapabilityDuplex.CurrentValue) > 0)
            {
              if (((bool)fCapabilityFeederEnabled.CurrentValue) == true)
              {
                fCapabilityDuplexEnabled.CurrentValue = fSettings.EnableDuplex;
              }
              else
              {
                fCapabilityDuplexEnabled.CurrentValue = false;
              }
            }
            else
            {
              // Don't touch the duplex enabled if the capability does not exist
            }

            // Analog adjustments
            fCapabilityThreshold.ScaledValue = settings.Threshold / 100.0;
            fCapabilityBrightness.ScaledValue = settings.Brightness / 100.0;
            fCapabilityContrast.ScaledValue = settings.Contrast / 100.0;
          }

          fCallback = callback;

          StartMessageFilter();

          fState = StateType.Active; // This makes IsActive = true

          if(fTwain.StartDataSession(fIdent, settings.ShowSettingsUI, settings.ShowTransferUI))
          {
            success = true;
          }
          else
          {
            Stop(); // TODO: Is this OK?
          }
        }

        return success;
      }


      private void Stop()
      {
        if(IsActive())
        {
          fTwain.TransferEnd(fIdent);
          fTwain.DiscardPendingTransfers(fIdent);
          fTwain.FinishDataSession(fIdent);
          StopMessageFilter();
          this.Close();
          Raise_ScanningComplete();
        }
      }


      private void StartMessageFilter()
      {
        HwndSource.FromHwnd(fTwain.GetWindowHandle())?.AddHook(this.PreFilterMessage);
      }


      private void StopMessageFilter()
      {
        HwndSource.FromHwnd(fTwain.GetWindowHandle())?.RemoveHook(this.PreFilterMessage);
      }


      private IntPtr PreFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
      {
        if (IsActive())
        {
          int pos = LibUser32.GetMessagePos();

          WINMSG message = new WINMSG();
          message.hwnd = hwnd;
          message.message = msg;
          message.wParam = wParam;
          message.lParam = lParam;
          message.time = LibUser32.GetMessageTime();
          message.x = (short)pos;
          message.y = (short)(pos >> 16);

          TwMSG msgCode = 0;

          if (fTwain.ProcessEvent(fIdent, ref message, ref msgCode))
          {
            switch (msgCode)
            {
              case TwMSG.XFerReady:
                {
                  bool success = TransferPictures();

                  if ((success == false) || (fSettings.ShowSettingsUI == false))
                  {
                    Stop(); // Need this only if UI is not enabled
                  }
                }
                break;

              case TwMSG.CloseDS:
              case TwMSG.CloseDSOK:
              case TwMSG.CloseDSReq:
                {
                  Stop(); // This happens only if UI is enabled
                }
                break;

              case TwMSG.DeviceEvent:
                break;

              default:
                break;
            }
          }
        }

        return IntPtr.Zero;
      }


      private bool TransferPictures()
      {
        bool success = IsActive();

        if(success)
        {
          TwPendingXfers pxfr = new TwPendingXfers();
          success = fTwain.GetNumberOfPendingTransfers(fIdent, pxfr);

          while(success && (pxfr.Count != 0))
          {
            TwImageInfo iinf = new TwImageInfo();
            success = fTwain.GetImageInfo(fIdent, iinf);

            IntPtr hBitmap = IntPtr.Zero;

            if(success)
            {
              success = fTwain.TransferImage(fIdent, ref hBitmap);
            }

            if(success)
            {
              IntPtr ptrBitmap = LibKernel32.GlobalLock(hBitmap);
              Image image = TwainUtils.DibToImage(ptrBitmap);
              LibKernel32.GlobalUnlock(hBitmap);
              Raise_NewPictureData(image);
            }

            if(success)
            {
              success = fTwain.TransferEnd(fIdent, pxfr);
            }
          }
        }

        return success;
      }


      public AcquireCallback fCallback;

      private void Raise_NewPictureData(Image image)
      {
        fCallback?.Invoke(image);
      }

      private void Raise_ScanningComplete()
      {
        fCallback?.Invoke(null);
      }
    }
  }
}