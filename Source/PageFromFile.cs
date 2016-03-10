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


    public PageFromFile(string fileName)
      : this(fileName, PageSize.Letter)
    {
      // nothing extra
    }


    public PageFromFile(string fileName, PageSize size)
    {
      this.fileName = fileName;
      this.fSize = size;
     
      InitializeImage();
    }


    protected override Image CreateImage()
    {
      return Image.FromFile(fileName);
    }
  }
}
