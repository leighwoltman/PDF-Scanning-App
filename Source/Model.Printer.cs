using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Utils;


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
      SizeInches pageSize = page.Size;
      int pageWidth = (int)(100 * pageSize.Width); // Convert to Hundreth of Inch
      int pageHeight = (int)(100 * pageSize.Height); // Convert to Hundreth of Inch
      e.PageSettings.PaperSize = new PaperSize("Custom", pageWidth, pageHeight);
    }


    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      Page page = fDocument.GetPage(pageIndex);

      Rectangle imageRect = new Rectangle();
      BoundsInches imageBounds = page.ImageBoundsInches;

      // Convert the image boundaries to output resolution
      imageRect.X = (int)(imageBounds.X * e.Graphics.DpiX);
      imageRect.Y = (int)(imageBounds.Y * e.Graphics.DpiY);
      imageRect.Width = (int)(imageBounds.Width * e.Graphics.DpiX);
      imageRect.Height = (int)(imageBounds.Height * e.Graphics.DpiY);

      Image image = page.GetImageInOriginalFormat();
      e.Graphics.PageUnit = GraphicsUnit.Pixel;
      e.Graphics.DrawImage(image, imageRect);
      image.Dispose();

      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
