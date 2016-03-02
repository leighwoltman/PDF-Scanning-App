using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;


namespace Model
{
  class Printer
  {
    private PrintDocument fPrintDocument;
    private Document fDocument;
    private int fPrintCurrentPage;
    private int fPrintToPage;


    public Printer()
    {
      fPrintDocument = new PrintDocument();
      fPrintDocument.PrintPage += fPrintDocument_PrintPage;
      fPrintCurrentPage = 0;
      fPrintToPage = 0;
    }


    public void PrintDocument(Document document, PrinterSettings printerSettings)
    {
      fDocument = document;
      fPrintCurrentPage = printerSettings.FromPage;
      fPrintToPage = printerSettings.ToPage;
      fPrintDocument.PrinterSettings = printerSettings;
      fPrintDocument.DocumentName = "Scanner" + fPrintCurrentPage + "-" + fPrintToPage;
      fPrintDocument.Print();
    }


    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      System.Drawing.Image img = (Image)fDocument.GetPage(pageIndex).getImage().Clone();
      Point loc = new Point(0, 0);

      // TODO: Review Landscape
      if(e.PageSettings.Landscape)
      {
        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
      }

      e.Graphics.DrawImage(img, loc);
      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
