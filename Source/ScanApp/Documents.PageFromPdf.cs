using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseUtils;
using HouseImaging;
using PdfProcessing;


namespace Documents
{
  public class PdfSettings
  {
    public bool AttemptSingleImageMode { get; set; } = false;

    public bool ImportNativePages { get; set; } = false;

    public int RenderResolution { get; set; } = 300;

    public PdfSettings Copy()
    {
      return new PdfSettings
      {
        AttemptSingleImageMode = this.AttemptSingleImageMode,
        ImportNativePages = this.ImportNativePages,
        RenderResolution = this.RenderResolution
      };
    }
  }


  class PageFromPdf : Page
  {
    static public PageFromPdf Create(PdfPage source, PdfSettings settings)
    {
      Size2D pageSize = source.Size;
      return new PageFromPdf(pageSize, source, settings);
    }


    private PdfPage fSource;
    private bool fSingleImageMode;
    private bool fImportNativePages;
    private int fRenderResolution;


    public PageFromPdf(Size2D pageSize, PdfPage source, PdfSettings settings) : base(pageSize)
    {
      fSource = source;
      fSingleImageMode = settings.AttemptSingleImageMode;
      fImportNativePages = settings.ImportNativePages;
      fRenderResolution = settings.RenderResolution;
    }


    public override bool CanModify()
    {
      return !fImportNativePages;
    }


    public override List<KeyValuePair<string, string>> GetInfoTable()
    {
      List<KeyValuePair<string, string>> result = base.GetInfoTable();

      result.Add(new KeyValuePair<string, string>("Source", "PDF"));
      result.Add(new KeyValuePair<string, string>("File", System.IO.Path.GetFileName(fSource.Document.Filename)));
      result.Add(new KeyValuePair<string, string>("Page Index", fSource.Index.ToString()));

      string mode;

      if (fSingleImageMode)
      {
        mode = "Single image";
      }
      else if (fImportNativePages)
      {
        mode = "Native";
      }
      else
      {
        mode = "Render";
      }

      result.Add(new KeyValuePair<string, string>("Mode", mode));
      result.Add(new KeyValuePair<string, string>("Render resolution", fRenderResolution.ToString()));

      return result;
    }


    public override string Name
    {
      get { return "Hello"; }
    }


    public override ImageInfo GetSourceImage()
    {
      ImageInfo result = null;

      if (fSingleImageMode)
      {
        result = fSource.ExtractSingleImage();
      }

      if (result == null)
      {
        fSingleImageMode = false;
        result = fSource.Render(fRenderResolution);
      }

      return result;
    }


    public override ImageInfo GetSourceThumbnail(int width, int height)
    {
      ImageInfo result = null;

      if (fSingleImageMode)
      {
        result = base.GetSourceThumbnail(width, height);
      }

      if (result == null)
      {
        int resW = (int)(width / Size.Width);
        int resH = (int)(height / Size.Height);
        int res = resW < resH ? resW : resH;
        result = fSource.Render(res);
      }

      return result;
    }


    public override void ExportToPdf(PdfDocument pdf, ExportSettings exportSettings)
    {
      if (fImportNativePages && !fSingleImageMode)
      {
        pdf.CopyNativePage(fSource);
      }
      else
      {
        base.ExportToPdf(pdf, exportSettings);
      }
    }
  }
}
