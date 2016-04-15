using System;
using System.Collections.Generic;
using System.Text;


namespace Model
{
  class ImageLoader
  {
    public void LoadImagesFromFiles(Document document, string[] filenames)
    {
      foreach(string filename in filenames)
      {
        LoadFromFile(document, filename);
      }
    }


    public void LoadFromFile(Document document, string filename)
    {
      Page myPage = new PageFromFile(filename);
      document.AddPage(myPage);
    }
  }
}
