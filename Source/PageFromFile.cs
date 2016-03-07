using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utils;


namespace Model
{
  public class PageFromFile : Page
  {
    private string fileName;
    private bool tempFile;


    public PageFromFile(string fileName)
      : this(fileName, false, PageSize.Letter)
    {
      // nothing extra
    }


    public PageFromFile(string fileName, bool temporary_file, PageSize size)
    {
      this.tempFile = temporary_file;
      this.fileName = fileName;
      this.size = size;
      
      // create a thumbnail
      using (Bitmap myBitmap = new Bitmap(fileName))
      {
        CreateThumbnail(myBitmap);
      }
    }


    public override Image getImage()
    {
      return Image.FromFile(fileName);
    }


    public override void cleanUp()
    {
      if(this.tempFile)
      {
        File.Delete(fileName);
      }

      base.pageCleanUp();
    }
  }
}
