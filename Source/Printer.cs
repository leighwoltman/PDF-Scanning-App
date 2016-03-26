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
      int pageWidth = (int)(100 * pageSize.Width); // Convert to Hundreth of Inch
      int pageHeight = (int)(100 * pageSize.Height); // Convert to Hundreth of Inch
      e.PageSettings.PaperSize = new PaperSize("Custom", pageWidth, pageHeight);
    }


    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      Page page = fDocument.GetPage(pageIndex);

      Rectangle imageRect = page.ImageBounds;

      double scaleX = e.Graphics.DpiX / page.ResolutionDpiX;
      double scaleY = e.Graphics.DpiY / page.ResolutionDpiY;

      // Convert the image boundaries to output resolution
      imageRect.X = (int)(imageRect.X * scaleX);
      imageRect.Y = (int)(imageRect.Y * scaleY); 
      imageRect.Width = (int)(imageRect.Width * scaleX);
      imageRect.Height = (int)(imageRect.Height * scaleY);

      Image img = page.GetImage();
      e.Graphics.PageUnit = GraphicsUnit.Pixel;
      e.Graphics.DrawImage(img, imageRect);

      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
