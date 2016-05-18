using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Defines;
using Utils;


namespace Model
{
  public class PageFromFile : Page
  {
    private string fFileName;


    public PageFromFile(string fileName, SizeInches size)
    {
      fFileName = fileName;
      Initialize(size, 0, 0);
    }


    public override string Name
    {
      get { return System.IO.Path.GetFileName(fFileName); }
    }


    protected override Image CreateImage()
    {
      return Imaging.LoadImageFromFile(fFileName);
    }
  }
}
