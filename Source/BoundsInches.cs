using System;
using System.Collections.Generic;
using System.Text;


namespace Model
{
  public class BoundsInches
  {
    private double fX;
    private double fY;
    private double fWidth;
    private double fHeight;


    public double X
    {
      get { return fX; }
    }


    public double Y
    {
      get { return fY; }
    }


    public double Width
    {
      get { return fWidth; }
    }


    public double Height
    {
      get { return fHeight; }
    }


    public BoundsInches(double x, double y, double width, double height)
    {
      fX = x;
      fY = y;
      fWidth = width;
      fHeight = height;
    }
  }
}
