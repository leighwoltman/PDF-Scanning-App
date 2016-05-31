using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Defines;


namespace Model
{
  class ImageSaver
  {
    public void SaveImages(Document document, string directory, List<int> pageNumbers)
    {
      foreach(int num in pageNumbers)
      {
        // Get the current page from document
        Page page = document.GetPage(num);
        System.Drawing.Image image = page.GetImage();
        ImageFormatEnum imageFormat = Imaging.GetImageFormat(image);
        string ext = Imaging.GetImageFormatExtension(imageFormat);

        Imaging.SaveImageToFile(image, Path.Combine(directory, page.Name));
      }
    }
  }
}
