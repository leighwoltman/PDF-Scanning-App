using System;
using System.Collections.Generic;
using System.Text;
using Utils;


namespace Model
{
  class ImageLoader
  {
    public void LoadImagesFromFiles(Document document, string[] filenames, SizeInches size)
    {
      foreach(string filename in filenames)
      {
        LoadFromFile(document, filename, size);
      }
    }


    public void LoadFromFile(Document document, string filename, SizeInches size)
    {
      Page myPage = new PageFromFile(filename, size);
      document.AddPage(myPage);
    }
  }
}
