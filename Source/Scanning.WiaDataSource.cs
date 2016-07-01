using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Scanning;
using Defines;
using Utils;


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


      public WiaDataSource(WIA.DeviceInfo info)
      {
        fIdent = info;
        fDevice = null;
        fItem = null;
        fCommonDialog = null;
      }


      public string Name
      {
        get { return "WIA I/F: " + (string)fIdent.Properties["Name"].get_Value(); }
      }


      public DataSourceCapabilities GetCapabilities()
      {
        DataSourceCapabilities result = null;

        if(this.Open())
        {
          result = new DataSourceCapabilities();

          result.ColorModes = new List<ColorModeEnum>() { ColorModeEnum.BW, ColorModeEnum.Gray, ColorModeEnum.RGB };
          result.PageTypes = new List<PageTypeEnum>() { PageTypeEnum.Letter, PageTypeEnum.Legal };
          result.Resolutions = new List<int>() { 100, 200, 300 };

          this.Close();
        }

        return result;
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


      public bool Acquire(DataSourceSettings settings)
      {
        bool result = false;

        if(this.Open())
        {
          int colorMode;

          switch(settings.ColorMode)
          {
            case ColorModeEnum.RGB: colorMode = (int)WiaPropertyCurrentIntent.WIA_INTENT_IMAGE_TYPE_COLOR; break;
            case ColorModeEnum.Gray: colorMode = (int)WiaPropertyCurrentIntent.WIA_INTENT_IMAGE_TYPE_GRAYSCALE; break;
            default: colorMode = (int)WiaPropertyCurrentIntent.WIA_INTENT_IMAGE_TYPE_TEXT; break;
          }

          int resolution = settings.Resolution;

          int brightness = (int)((settings.Brightness - 0.5) * 2000);

          int contrast = (int)((settings.Contrast - 0.5) * 2000);

          AdjustScannerSettings(resolution, settings.ScanArea, brightness, contrast, colorMode);

          bool morePages = true;

          while(morePages)
          {
            Image newImage = Scan(settings.ShowTransferUI);

            if(newImage != null)
            {
              Raise_OnNewPictureData(newImage, settings);
            }

            morePages = HasMorePages();
          }

          this.Close();
          Raise_OnScanningComplete();

          result = true;
        }

        return result;
      }


      private bool AdjustScannerSettings(
        int resolutionDpi,
        BoundsInches scanAreaInches,
        int brightnessPercents,
        int contrastPercents,
        int colorMode)
      {
        bool result;

        try
        {
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalResolution, resolutionDpi);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalResolution, resolutionDpi);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalStartPosition, (int)(scanAreaInches.X * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalStartPosition, (int)(scanAreaInches.Y * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.HorizontalExtent, (int)(scanAreaInches.Width * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.VerticalExtent, (int)(scanAreaInches.Height * resolutionDpi));
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.Brightness, brightnessPercents);
          WiaUtils.SetProperty(fItem.Properties, WiaProperty.Contrast, contrastPercents);
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
          //determine if there are any more pages waiting

          object documentHandlingStatus = WiaUtils.GetProperty(fDevice.Properties, WiaProperty.DocumentHandlingStatus);
          object documentHandlingSelect = WiaUtils.GetProperty(fDevice.Properties, WiaProperty.DocumentHandlingSelect);

          if(documentHandlingSelect != null) // may not exist on flatbed scanner but required for feeder
          {
            if((Convert.ToUInt32(documentHandlingSelect) & (uint)WiaPropertyDocumentHandlingSelect.FEEDER) != 0)
            {
              if(documentHandlingStatus != null)
              {
                if((Convert.ToUInt32(documentHandlingStatus) & (uint)WiaPropertyDocumentHandlingStatus.FEED_READY) != 0)
                {
                  result = true;
                }
              }
            }
          }
        }
        catch(Exception ex)
        {
          string msg = ex.Message;
          result = false;
        }

        return result;
      }


      public event EventHandler OnNewPictureData;

      private void Raise_OnNewPictureData(Image image, DataSourceSettings theSettings)
      {
        if(OnNewPictureData != null)
        {
          NewPictureEventArgs args = new NewPictureEventArgs();
          args.TheImage = image;
          args.TheSettings = theSettings;
          OnNewPictureData(this, args);
        }
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
}