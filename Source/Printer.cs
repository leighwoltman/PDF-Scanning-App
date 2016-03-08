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

    
    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      System.Drawing.Image img = (Image)fDocument.GetPage(pageIndex).getImage();
      Point loc = new Point(0, 0);
      e.Graphics.DrawImage(img, loc);
      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
