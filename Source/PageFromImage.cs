using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utils;


namespace Model
{
  public class PageFromImage : Page
  {
    private Image myImageFile;

    public PageFromImage(Image image)
    {
      this.myImageFile = image;
      CreateThumbnail(image);
      this.size = PageSize.Letter;
    }


    public override Image getImage()
    {
      return myImageFile;
    }


    public override void cleanUp()
    {
      this.myImageFile.Dispose();
      this.myImageFile = null;

      base.pageCleanUp();
    }
  }
}
