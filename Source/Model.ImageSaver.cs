﻿using System;
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
    public void SaveImages(Document document, string directory, List<int> pageNumbers, int compressionRate)
    {
      foreach(int num in pageNumbers)
      {
        Page page = document.GetPage(num);

        Image image1 = page.GetImageInOriginalFormat(compressionRate);
        SaveImage(image1, directory, page.Name + "-Org");

        Image image2 = page.GetImage();
        SaveImage(image2, directory, page.Name + "-Mem");

        Image image3 = page.GetCompressedImage(compressionRate);
        SaveImage(image3, directory, page.Name + "-Comp");
      }
    }


    private void SaveImage(Image image, string directory, string name)
    {
      // Use Image->ByteArray then ByteArray->ext because this yields the output format correctly:
      // In some cases the format of the image in memory and the format in the output file differ. 
      // For example MemoryBmp is always saved as PNG by the ImageConverter. Therefore since the 
      // byteArray contains image data converted for output, it has the right output format. 
      byte[] byteArray = Imaging.ByteArrayFromImage(image);
      string ext = Imaging.GetImageExtensionFromByteArray(byteArray);
      string fileName = Path.Combine(directory, name + ext);
      Imaging.SaveImageByteArrayToFile(fileName, byteArray);
    }
  }
}
