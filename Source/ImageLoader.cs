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
        Page myPage = new PageFromFile(filename);
        document.AddPage(myPage);
      }
    }
  }
}
