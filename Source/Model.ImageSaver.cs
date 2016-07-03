using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Defines;


namespace Model
{
  class ImageSaver
  {
    public void SaveImages(Document document, string filename, List<int> pageNumbers, int compressionRate)
    {
      string directory = System.IO.Path.GetDirectoryName(filename);
      string extension = System.IO.Path.GetExtension(filename);

      System.Drawing.Imaging.ImageFormat format;

      switch(extension.ToUpper())
      {
        case ".JPG":
          {
            format = System.Drawing.Imaging.ImageFormat.Jpeg;
          }
          break;
        case ".GIF":
          {
            format = System.Drawing.Imaging.ImageFormat.Gif;
          }
          break;
        case ".PNG":
          {
            format = System.Drawing.Imaging.ImageFormat.Png;
          }
          break;
        case ".BMP":
          {
            format = System.Drawing.Imaging.ImageFormat.Bmp;
          }
          break;
        default:
          {
            format = null;
          }
          break;
      }

      foreach(int num in pageNumbers)
      {
        Page page = document.GetPage(num);

        Image image = page.GetImage();

        if (format == null)
        {
          // Since the format is not specified use the native format from the image
          format = image.RawFormat;
        }

        string saveName = page.Name;

        if(pageNumbers.Count == 1)
        {
          saveName = System.IO.Path.GetFileNameWithoutExtension(filename);
        }

        SaveImage(image, format, directory, saveName, compressionRate);
      }
    }


    private void SaveImage(Image image, System.Drawing.Imaging.ImageFormat format, string directory, string name, int compressionRate)
    {
      byte[] byteArray = Imaging.EncodeImage(image, format, compressionRate);
      // In some cases the format of the image in memory and the format in the output file differ. 
      // For example MemoryBmp is always saved as PNG by the ImageConverter. Therefore since the 
      // byteArray contains image data converted for output, it has the right output format. 
      string ext = Imaging.GetImageExtensionFromByteArray(byteArray);
      string fileName = Path.Combine(directory, name + ext);
      Imaging.SaveImageByteArrayToFile(fileName, byteArray);
    }
  }
}
