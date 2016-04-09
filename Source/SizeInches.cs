using System;
using System.Collections.Generic;
using System.Text;


namespace Model
{
  public class SizeInches : IEquatable<SizeInches>
  {
    private double fWidth;
    private double fHeight;


    public double Width
    {
      get { return fWidth; }
    }


    public double Height
    {
      get { return fHeight; }
    }


    public SizeInches(double width, double height)
    {
      fWidth = width;
      fHeight = height;
    }


    public bool Equals(SizeInches other)
    {
      if (other == null) return false;
      return (this.Width == other.Width && this.Height == other.Height);
    }


    public SizeInches Transform(bool sideways)
    {
      return sideways ? new SizeInches(fHeight, fWidth) : this;
    }

    
    static public SizeInches Letter
    {
      get
      {
        return new SizeInches(8.5, 11);
      }
    }

    static public SizeInches Legal
    {
      get
      {
        return new SizeInches(8.5, 14);
      }
    }
  }
}
