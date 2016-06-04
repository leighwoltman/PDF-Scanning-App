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
        SaveImage(image, format, directory, page.Name, compressionRate);

        // Alternate methods:

        // Image image1 = page.GetImageInOriginalFormat(compressionRate);
        // SaveImage(image1, format, directory, page.Name + "-Org", compressionRate);

        // Image image2 = page.GetCompressedImage(compressionRate);
        // SaveImage(image2, format, directory, page.Name + "-Comp", compressionRate);
      }
    }


    private void SaveImage(Image image, System.Drawing.Imaging.ImageFormat format, string directory, string name, int compressionRate)
    {
      byte[] byteArray;

      if(format == null)
      {
        // No conversion, use native format to save the image
        // Use Image->ByteArray then ByteArray->ext because this yields the output format correctly:
        // In some cases the format of the image in memory and the format in the output file differ. 
        // For example MemoryBmp is always saved as PNG by the ImageConverter. Therefore since the 
        // byteArray contains image data converted for output, it has the right output format. 
        byteArray = Imaging.ByteArrayFromImage(image);
      }
      else
      {
        byteArray = Imaging.EncodeImage(image, format, compressionRate);
      }

      string ext = Imaging.GetImageExtensionFromByteArray(byteArray);
      string fileName = Path.Combine(directory, name + ext);
      Imaging.SaveImageByteArrayToFile(fileName, byteArray);
    }
  }
}
