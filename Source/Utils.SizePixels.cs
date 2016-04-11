using System;
using System.Collections.Generic;
using System.Text;


namespace Utils
{
  public class SizePixels
  {
    private int fWidth;
    private int fHeight;


    public int Width
    {
      get { return fWidth; }
    }


    public int Height
    {
      get { return fHeight; }
    }


    public SizePixels(int width, int height)
    {
      fWidth = width;
      fHeight = height;
    }


    public SizePixels Transform(bool sideways)
    {
      return sideways ? new SizePixels(fHeight, fWidth) : this;
    }
  }
}
