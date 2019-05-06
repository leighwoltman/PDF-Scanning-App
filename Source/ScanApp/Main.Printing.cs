using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using HouseUtils;
using HouseImaging;


namespace ScanApp
{
  class Printing
  {
    private PrintDocument fPrintDocument;
    private List<ListViewPageItem> fDocument;
    private int fPrintCurrentPage;
    private int fPrintToPage;


    public Printing()
    {
      fPrintDocument = new PrintDocument();
      fPrintDocument.BeginPrint += fPrintDocument_BeginPrint;
      fPrintDocument.PrintPage += fPrintDocument_PrintPage;
      fPrintDocument.QueryPageSettings += fPrintDocument_QueryPageSettings;
      fPrintCurrentPage = 0;
      fPrintToPage = 0;
    }


    public void PrintDocument(List<ListViewPageItem> document, bool usePreview = true)
    {
      PrinterSettings settings = Dialogs.ExecutePrintDialog("PrimoPDF", 1, document.Count);

      if (settings != null)
      {
        if (usePreview)
        {
          PreviewDocument(document, settings);
        }
        else
        {
          PrintDocument(document);
        }
      }
    }


    public void PrintDocument(List<ListViewPageItem> document, PrinterSettings printerSettings)
    {
      fDocument = document;
      fPrintDocument.PrinterSettings = printerSettings;
      fPrintDocument.Print();
    }


    public void PreviewDocument(List<ListViewPageItem> document, PrinterSettings printerSettings)
    {
      fDocument = document;
      fPrintDocument.PrinterSettings = printerSettings;
      Dialogs.ExecutePrintPreview(fPrintDocument);
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
      ListViewPageItem pageItem = fDocument[pageIndex];
      Size2D pageSize = pageItem.Page.Size;
      int pageWidth = (int)(100 * pageSize.Width); // Convert to Hundreth of Inch
      int pageHeight = (int)(100 * pageSize.Height); // Convert to Hundreth of Inch
      e.PageSettings.PaperSize = new PaperSize("Custom", pageWidth, pageHeight);
    }


    void fPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      int pageIndex = fPrintCurrentPage - 1;
      ListViewPageItem pageItem = fDocument[pageIndex];
      /*
            using (ImageInfo myImage = pageItem.Page.GetTransformedImage())
            {
              Rectangle imageRect = new Rectangle();
              Bounds imageBounds = pageItem.Page.GetImageBounds(myImage.SizePixels);

              // Convert the image boundaries to output resolution
              imageRect.X = (int)(imageBounds.X * e.Graphics.DpiX);
              imageRect.Y = (int)(imageBounds.Y * e.Graphics.DpiY);
              imageRect.Width = (int)(imageBounds.Width * e.Graphics.DpiX);
              imageRect.Height = (int)(imageBounds.Height * e.Graphics.DpiY);

              e.Graphics.PageUnit = GraphicsUnit.Pixel;
              e.Graphics.DrawImage(myImage.SystemImage, imageRect);
            }
      */
      using (ImageInfo myImage = pageItem.Page.GetLayout())
      {
        Rectangle imageRect = new Rectangle();
        Size2D imageBounds = pageItem.Page.Size;

        // Convert the image boundaries to output resolution
        imageRect.X = 0;
        imageRect.Y = 0;
        imageRect.Width = (int)(imageBounds.Width * e.Graphics.DpiX);
        imageRect.Height = (int)(imageBounds.Height * e.Graphics.DpiY);

        e.Graphics.PageUnit = GraphicsUnit.Pixel;
        e.Graphics.DrawImage(myImage.SystemImage, imageRect);
      }

      e.HasMorePages = fPrintCurrentPage < fPrintToPage;
      fPrintCurrentPage++;
    }
  }
}
