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
      : this(fileName, SizeInches.Letter)
    {
      // nothing extra
    }


    public PageFromFile(string fileName, SizeInches size)
    {
      this.fileName = fileName;
      Initialize(size, 0, 0);
    }


    protected override Image CreateImage()
    {
      return Image.FromFile(fileName);
    }
  }
}
