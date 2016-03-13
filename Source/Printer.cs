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
      fPrintDocument.BeginPrint += fPrintDocument_BeginPrint;
      fPrintDocument.PrintPage += fPrintDocument_PrintPage;
      fPrintDocument.QueryPageSettings += fPrintDocument_QueryPageSettings;
      fPrintCurrentPage = 0;
      fPrintToPage = 0;
    }


    public void PrintDocument(Document document, PrinterSettings printerSettings)
    {
      fDocument = document;
      fPrintDocument.PrinterSettings = printerSettings;
      fPrintDocument.Print();
    }


    public void PreviewDocument(Document document, PrinterSettings printerSettings)
    {
      fDocument = document;
      fPrintDocument.PrinterSettings = printerSettings;

      PrintPreviewDialog dlg = new PrintPreviewDialog();
      dlg.Document = fPrintDocument;
      dlg.ShowDialog();
    }


    void fPrintDocument_BeginPrint(object sender, PrintEventArgs e)
    {
      fPrintCurrentPage = fPrintDocument.PrinterSettings.FromPage;
      fPrintToPage = fPrintDocument.PrinterSettings.ToPage;
      fPrintDocument.DocumentName = "Scanner" + fPrintCurrentPage + "-" + fPrintToPage;
    }


    void fPrintDocument_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      Page page = fDocument.GetPage(pageIndex);
      PageSize pageSize = page.Size;
      int width = InchesToHundrethsOfInch(pageSize.Width);
      int height = InchesToHundrethsOfInch(pageSize.Height);
      e.PageSettings.PaperSize = new PaperSize("Custom", width, height);
    }


    int InchesToHundrethsOfInch(double value)
    {
      return (int)(value * 100);
    }


    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      Page page = fDocument.GetPage(pageIndex);

      double image_aspect_ratio = page.ImageWidthPixels / (double)page.ImageHeightPixels;

      PageSize pageSize = page.Size;
      double page_aspect_ratio = pageSize.Width / pageSize.Height;

      Rectangle rect = new Rectangle();

      if(image_aspect_ratio > page_aspect_ratio)
      {
        // means our image has the width as the maximum dimension
        double imageWidth = pageSize.Width;
        double imageHeight = pageSize.Width / image_aspect_ratio;
        
        rect.X = InchesToHundrethsOfInch(0);
        rect.Y = InchesToHundrethsOfInch((pageSize.Height - imageHeight) / 2);
        rect.Width = InchesToHundrethsOfInch(imageWidth);
        rect.Height = InchesToHundrethsOfInch(imageHeight);
      }
      else
      {
        // means our image has the height as the maximum dimension
        double imageWidth = pageSize.Height * image_aspect_ratio;
        double imageHeight = pageSize.Height;

        rect.X = InchesToHundrethsOfInch((pageSize.Width - imageWidth) / 2);
        rect.Y = InchesToHundrethsOfInch(0);
        rect.Width = InchesToHundrethsOfInch(imageWidth);
        rect.Height = InchesToHundrethsOfInch(imageHeight);
      }

      Image img = page.GetImage();
      e.Graphics.DrawImage(img, rect);

      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
