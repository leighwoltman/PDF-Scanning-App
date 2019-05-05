using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using HouseUtils;


namespace Scanning
{
  partial class WiaDataSourceManager
  {
    class WiaDataSource : InterfaceDataSource
    {
      private WIA.DeviceInfo fIdent;
      private WIA.Device fDevice;
      private WIA.Item fItem;
      private WIA.CommonDialog fCommonDialog;
      private DataSourceCapabilities fCapabilities;


      public WiaDataSource(WIA.DeviceInfo info)
      {
        fIdent = info;
        fDevice = null;
        fItem = null;
        fCommonDialog = null;
        fCapabilities = null;
      }


      public string Name
      {
        get { return "WIA I/F: " + (string)fIdent.Properties["Name"].get_Value(); }
      }


      public DataSourceCapabilities GetCapabilities()
      {
        if (fCapabilities == null)
        {
          if (this.Open())
          {
            fCapabilities = new DataSourceCapabilities();

            fCapabilities.ColorModes = new List<ColorModeEnum>() { ColorModeEnum.BW, ColorModeEnum.Gray, ColorModeEnum.RGB };
            fCapabilities.PageTypes = new List<PageTypeEnum>() { PageTypeEnum.Letter, PageTypeEnum.Legal };
            fCapabilities.Resolutions = new List<int>();

            for (int res = 100; res < 4000; res += 100)
            {
              try
              {
                WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalResolution, res);
                fCapabilities.Resolutions.Add(res);
              }
              catch
              { }
            }

            this.Close();
          }
        }

        return fCapabilities;
      }


      private bool IsOpen
      {
        get { return (bool)(fDevice != null); }
      }


      private bool Open()
      {
        if(IsOpen == false)
        {
          fDevice = fIdent.Connect();
          fItem = fDevice.Items[1] as WIA.Item;
          fCommonDialog = new WIA.CommonDialog();
        }
        return IsOpen;
      }


      private void Close()
      {
        if(IsOpen)
        {
          fDevice = null;
          fItem = null;
          fCommonDialog = null;
        }
      }


      public bool Acquire(DataSourceSettings settings, AcquireCallback callback)
      {
        bool result = false;

        if(this.Open())
        {
          int colorMode;

          switch(settings.ColorMode)
          {
            case ColorModeEnum.RGB: colorMode = (int)WiaPropertyCurrentIntent.IMAGE_TYPE_COLOR; break;
            case ColorModeEnum.Gray: colorMode = (int)WiaPropertyCurrentIntent.IMAGE_TYPE_GRAYSCALE; break;
            default: colorMode = (int)WiaPropertyCurrentIntent.IMAGE_TYPE_TEXT; break;
          }

          int resolution = settings.Resolution;

          int brightness = (int)((settings.Brightness - 50) * 20);

          int contrast = (int)((settings.Contrast - 50) * 20);

          int threshold = 100 - settings.Threshold;

          AdjustScannerSettings(resolution, settings.ScanArea, brightness, contrast, threshold, colorMode, settings.EnableFeeder);

          bool morePages = true;

          while(morePages)
          {
            Image newImage = Scan(settings.ShowTransferUI);

            if(newImage != null)
            {
              callback(newImage);
            }

            morePages = HasMorePages();
          }

          this.Close();

          callback(null);

          result = true;
        }

        return result;
      }


      private bool AdjustScannerSettings(
        int resolutionDpi,
        Bounds scanAreaInches,
        int brightnessPercents,
        int contrastPercents,
        int threshold,
        int colorMode,
        bool enableFeeder)
      {
        bool result;

        try
        {
          // Select flatbad then the legal paper type. Gives exception if document handling style does not support paper type.
          SelectFeederOrFlatbad(enableFeeder && IsFeederLoaded());

          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalResolution, resolutionDpi);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalResolution, resolutionDpi);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalStartPosition, (int)(scanAreaInches.X * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalStartPosition, (int)(scanAreaInches.Y * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalExtent, (int)(scanAreaInches.Width * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalExtent, (int)(scanAreaInches.Height * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.Brightness, brightnessPercents);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.Contrast, contrastPercents);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.Threshold, threshold);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.CurrentIntent, colorMode);

          result = true;
        }
        catch
        {
          result = false;
        }

        return result;
      }


      public Image Scan(bool showTransferUI)
      {
        Image result;

        try
        {
          WIA.ImageFile imageFile;

          if(showTransferUI)
          {
            imageFile = (WIA.ImageFile)fCommonDialog.ShowTransfer(fItem, WiaDefines.FormatBMP, false);
          }
          else
          {
            imageFile = (WIA.ImageFile)fItem.Transfer(WiaDefines.FormatBMP);
          }

          result = WiaUtils.WiaImageFileToImage(imageFile);
        }
        catch(Exception ex)
        {
          string msg = ex.Message;
          result = null;
        }

        return result;
      }


      private bool HasMorePages()
      {
        bool result = false;

        try
        {
          if (IsFeederSelected())
          {
            result = IsFeederLoaded();
          }
        }
        catch(Exception ex)
        {
          string msg = ex.Message;
          result = false;
        }

        return result;
      }


      private bool IsFeederLoaded()
      {
        bool result = false;

        object documentHandlingStatus = WiaUtils.GetProperty(fDevice.Properties, WiaProperty.DocumentHandlingStatus);

        if (documentHandlingStatus != null)
        {
          if ((Convert.ToUInt32(documentHandlingStatus) & (uint)WiaPropertyDocumentHandlingStatus.FEED_READY) != 0)
          {
            result = true;
          }
        }

        return result;
      }


      private bool IsFeederSelected()
      {
        bool result = false;

        object documentHandlingSelect = WiaUtils.GetProperty(fDevice.Properties, WiaProperty.DocumentHandlingSelect);

        if (documentHandlingSelect != null)
        {
          if ((Convert.ToUInt32(documentHandlingSelect) & (uint)WiaPropertyDocumentHandlingSelect.FEEDER) != 0)
          {
            result = true;
          }
        }

        return result;
      }


      private void SelectFeederOrFlatbad(bool feeder)
      {
        UInt32 value = (uint)(feeder? WiaPropertyDocumentHandlingSelect.FEEDER : WiaPropertyDocumentHandlingSelect.FLATBED);
        WiaUtils.SetProperty(fDevice.Properties, WiaProperty.DocumentHandlingSelect, value);
      }
    }
  }
}