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


    public bool LoadFromFile(Document document, string filename, SizeInches size)
    {
      bool validFile;

      try
      {
        System.Drawing.Image image = Imaging.LoadImageFromFile(filename);
        validFile = true;
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
        validFile = false;
      }

      if(validFile)
      {
        Page myPage = new PageFromFile(filename, size);
        document.AddPage(myPage);
      }

      return validFile;
    }
  }
}
