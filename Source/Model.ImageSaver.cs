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
        Page page = document.GetPage(num);
        System.Drawing.Image image = page.GetImageInOriginalFormat();
        Imaging.SaveImageToFile(image, directory, page.Name);
      }
    }
  }
}
