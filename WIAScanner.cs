using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WIA;

namespace WIATest
{
    class WIAScanner
    {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        
        class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 0x00000001;
            public const uint FLATBED = 0x00000002;
        }
        
        class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEED_READY = 0x00000001;
        }
        
        class WIA_PROPERTIES
        {
            public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
            public const uint WIA_DIP_FIRST = 2;
            public const uint WIA_DPA_FIRST = WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPC_FIRST = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            //
            // Scanner only device properties (DPS)
            //
            public const uint WIA_DPS_FIRST = WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = WIA_DPS_FIRST + 13;
            public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = WIA_DPS_FIRST + 14;
        }

        public static List<Image> Scan(string scannerId, double width_inches, double height_inches, double dpi)
        {
          List<Image> retval = new List<Image>();

          bool more_pages = true;

          while (more_pages)
          {
            try
            {
              // select the correct scanner using the provided scannerId parameter
              WIA.DeviceManager manager = new WIA.DeviceManager();
              WIA.Device device = null;
              foreach (WIA.DeviceInfo info in manager.DeviceInfos)
              {
                if (info.DeviceID == scannerId)
                {
                  // connect to scanner
                  device = info.Connect();
                  break;
                }
              }

              // device was not found
              if (device == null)
              {
                return null;
              }

              WIA.Item item = device.Items[1] as WIA.Item;

              item.Properties["6147"].set_Value(dpi);
              item.Properties["6148"].set_Value(dpi);
              //setting start coordinates
              item.Properties["6149"].set_Value(0);
              item.Properties["6150"].set_Value(0);
              //setting width and height
              item.Properties["6151"].set_Value((int)(width_inches * dpi));
              item.Properties["6152"].set_Value((int)(height_inches * dpi));
              //1 if colorful; 2 if gray
              item.Properties["6146"].set_Value(1);

              try
              {
                // scan image
                WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                WIA.ImageFile image = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatBMP, false);

                // save to temp file
                string fileName = Path.GetTempFileName();
                File.Delete(fileName);
                image.SaveFile(fileName);

                image = null;

                using (var sourceImage = Image.FromFile(fileName))
                {
                  Image result = new Bitmap(sourceImage.Width, sourceImage.Height,
                    PixelFormat.Format32bppArgb);
                  using (var canvas = Graphics.FromImage(result))
                  {
                    canvas.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height);
                  }
                  retval.Add(result);
                }
                File.Delete(fileName);
                // delete any temp files
                var files = Directory.GetFiles(Path.GetDirectoryName(fileName));

                foreach (var file in files)
                {
                  string name = Path.GetFileName(file);
                  string extension = Path.GetExtension(file);

                  if (name.StartsWith("img") && extension == ".tmp")
                  {
                    // then delete the file
                    try
                    {
                      File.Delete(file);
                    }
                    catch
                    { }
                  }
                }

                item = null;

                //determine if there are any more pages waiting
                Property documentHandlingSelect = null;
                Property documentHandlingStatus = null;

                foreach (Property prop in device.Properties)
                {
                  if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_SELECT)
                    documentHandlingSelect = prop;
                  if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_STATUS)
                    documentHandlingStatus = prop;
                }
                more_pages = false; //assume there are no more pages
                if (documentHandlingSelect != null)
                //may not exist on flatbed scanner but required for feeder
                {
                  //check for document feeder
                  if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
                  {
                    more_pages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
                  }
                }
              }
              catch (Exception exc)
              {
                throw exc;
              }
            }
            catch
            {
              
            }
          }
          return retval;
        }
    }
}
