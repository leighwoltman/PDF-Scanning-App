using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;


namespace WiaInterface
{
  public partial class WiaDataSourceManager
  {
    class WiaUtils
    {
      public static void SetProperty(WIA.Properties properties, WiaProperty propertyId, object value)
      {
        foreach(WIA.Property property in properties)
        {
          if(property.PropertyID == (int)propertyId)
          {
            property.set_Value(value);
            break;
          }
        }
      }


      public static object GetProperty(WIA.Properties properties, WiaProperty propertyId)
      {
        object result = null;

        foreach(WIA.Property property in properties)
        {
          if(property.PropertyID == (int)propertyId)
          {
            result = property.get_Value();
            break;
          }
        }

        return result;
      }


      public static Image WiaImageFileToImage(WIA.ImageFile imageFile)
      {
        // Converts the ImageFile to a byte array, then a memory stream, and then an Image
        Byte[] imageBytes = (byte[])imageFile.FileData.get_BinaryData();
        MemoryStream ms = new MemoryStream(imageBytes);
        return Image.FromStream(ms);
      }
    }
  }
}