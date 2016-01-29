using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;


namespace PDFScanningApp
{
  class Document
  {
    List<Page> pages;

    public Document()
    {
      pages = new List<Page>();
    }

    public void AddPage(Page newPage)
    {
      pages.Add(newPage);
    }

    public void DeletePage(Page pageToDelete)
    {
      pages.Remove(pageToDelete);
    }

    public void MovePageUp(Page pageToMove)
    {
      int index = pages.IndexOf(pageToMove);
      pages.RemoveAt(index);
      pages.Insert(index - 1, pageToMove);
    }

    public void MovePageDown(Page pageToMove)
    {
      int index = pages.IndexOf(pageToMove);
      pages.RemoveAt(index);
      pages.Insert(index + 1, pageToMove);
    }

    public void MovePageTo(Page pageToMove, int targetIndex)
    {
      int index = pages.IndexOf(pageToMove);
      pages.RemoveAt(index);
      pages.Insert(targetIndex, pageToMove);
    }

    public void Save(string fileName)
    {
      PdfDocument document = new PdfDocument();
      document.Info.Title = "Created with PDFsharp";

      for (int i = 0; i < pages.Count; i++)
      {
        Page imgContainer = pages[i];

        // Create an empty page
        PdfPage page = document.AddPage();
        if (imgContainer.getScanPageSize() == ScanPageSize.Legal)
        {
          page.Size = PageSize.Legal;
        }
        else
        {
          page.Size = PageSize.Letter;
        }

        // we need to swap the height and the width if this is a landscape image
        double aspect_ratio = ((double)imgContainer.getWidth()) / ((double)imgContainer.getHeight());

        if (imgContainer.isLandscape())
        {
          page.Orientation = PageOrientation.Landscape;
        }
        else
        {
          page.Orientation = PageOrientation.Portrait;
        }

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);
        // Create a font
        XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

        int draw_point_x = 0;
        int draw_point_y = 0;
        int draw_point_width = 0;
        int draw_point_height = 0;

        if (imgContainer.isLandscape())
        {
          // these are swapped
          draw_point_width = (int)page.Height;
          draw_point_height = (int)page.Width;

          if (aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
          {
            // means our image has the width as the maximum dimension
            draw_point_height = (int)((double)draw_point_width / aspect_ratio);
            draw_point_y = ((int)page.Height - draw_point_height) / 2;
            draw_point_x = ((int)page.Width - draw_point_width) / 2;
          }
          else
          {
            // means our image has the height as the maximum dimension
            draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
            draw_point_x = ((int)page.Width - draw_point_width) / 2;
            draw_point_y = ((int)page.Height - draw_point_height) / 2;
          }
        }
        else
        {
          draw_point_width = (int)page.Width;
          draw_point_height = (int)page.Height;

          if (aspect_ratio > ((double)draw_point_width / (double)draw_point_height))
          {
            // means our image has the width as the maximum dimension
            draw_point_height = (int)((double)draw_point_width / aspect_ratio);
            draw_point_y = ((int)page.Height - draw_point_height) / 2;
          }
          else
          {
            // means our image has the height as the maximum dimension
            draw_point_width = (int)(aspect_ratio * (double)draw_point_height);
            draw_point_x = ((int)page.Width - draw_point_width) / 2;
          }
        }


        XImage image = XImage.FromFile(imgContainer.getFileName());

        if (imgContainer.isRotated())
        {
          // rotate around the center of the page
          gfx.RotateAtTransform(-180, new XPoint(page.Width / 2, page.Height / 2));
        }

        if (imgContainer.isLandscape())
        {
          // rotate around the center of the page
          gfx.RotateAtTransform(90, new XPoint(page.Width / 2, page.Height / 2));
        }

        gfx.DrawImage(image, draw_point_x, draw_point_y, draw_point_width, draw_point_height);
        image.Dispose();
      }

      // Save the document...
      document.Save(fileName);

    }

    public void RemoveAll()
    {
      int count = pages.Count;

      for(int i = 0; i < count; i++)
      {
        pages[0].cleanUp();
        pages.RemoveAt(0);
      }
    }
  }
}
