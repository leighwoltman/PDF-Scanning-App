using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Utils;


namespace Model
{
  class PdfExporter
  {
    public void SaveDocument(Document document, string filename)
    {
      PdfDocument pdfDocument = new PdfDocument();
      pdfDocument.Info.Title = "Created with PDFsharp";

      for(int i = 0; i < document.NumPages; i++)
      {
        // Get the current page from document

        document.GetPage(i).AddPdfPage(pdfDocument);
      }

      // Save the document...
      pdfDocument.Save(filename);
    }
  }
}
