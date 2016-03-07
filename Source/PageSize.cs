using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
  public class PageSize : IEquatable<PageSize>
  {
    public double Width;
    public double Height;

    public PageSize(double width, double height)
    {
      this.Width = width;
      this.Height = height;
    }

    //public void RotateOrientation()
    //{
    //  double temp = this.Width;
    //  this.Width = this.Height;
    //  this.Height = temp;
    //}

    public bool Equals(PageSize other)
    {
      if (other == null) return false;
      return (this.Width == other.Width && this.Height == other.Height);
    }

    
    static public PageSize Letter
    {
      get
      {
        return new PageSize(8.5, 11);
      }
    }

    static public PageSize Legal
    {
      get
      {
        return new PageSize(8.5, 14);
      }
    }
  }
}
