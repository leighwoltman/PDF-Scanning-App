using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HouseUtils;
using Documents;
using System.Collections.ObjectModel;

namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowAppSettings.xaml
  /// </summary>
  public partial class WindowAppSettings : Window
  {
    class Model : BindingPropertyClass
    {
      public void Raise()
      {
        // These need to be raised once, for initialization, since the values only change by UI, and not programmatically
        RaisePropertyChanged("PageTypes");
        RaisePropertyChanged("DefaultPageType");
        RaisePropertyChanged("CustomPageSize");
        RaisePropertyChanged("Pdf_AttemptSingleImageMode");
        RaisePropertyChanged("Pdf_ImportNativePages");
        RaisePropertyChanged("Pdf_RenderResolution");
        RaisePropertyChanged("Export_CompressImages");
        RaisePropertyChanged("Export_CompressionFactor");
        RaisePropertyChanged("Export_RemovePages");
        RaisePropertyChanged("ShowPrintButton");
        RaisePropertyChanged("ExcludedScanners");
      }

      public bool ShowPrintButton { get; set; }

      public List<string> PageTypes { get; set; }

      public string DefaultPageType { get; set; }

      public Size2D CustomPageSize { get; set; } = new Size2D(0, 0);

      public bool Pdf_AttemptSingleImageMode { get; set; }

      public bool Pdf_ImportNativePages { get; set; }

      public int Pdf_RenderResolution { get; set; }

      public bool Export_CompressImages { get; set; }

      public bool Export_RemovePages { get; set; }

      public int Export_CompressionFactor { get; set; }

      public ObservableCollection<BoolStringClass> ExcludedScanners { get; set; } = new ObservableCollection<BoolStringClass>();
    }


    private Model fModel = new Model();


    public WindowAppSettings()
    {
      InitializeComponent();

      DataContext = fModel;
    }


    public bool ShowPrintButton
    {
      get { return fModel.ShowPrintButton; }
      set { fModel.ShowPrintButton = value; }
    }

    public List<string> PageTypes
    {
      get { return fModel.PageTypes; }
      set { fModel.PageTypes = value; }
    }

    public string DefaultPageType
    {
      get { return fModel.DefaultPageType; }
      set { fModel.DefaultPageType = value; }
    }

    public Size2D CustomPageSize
    {
      get { return fModel.CustomPageSize.Copy(); }
      set { fModel.CustomPageSize = value.Copy(); }
    }

    public List<BoolStringClass> ExcludedScanners
    {
      get
      {
        var retval = new List<BoolStringClass>();
        foreach (var l in fModel.ExcludedScanners)
        {
          retval.Add(l.Copy());
        }
        return retval;
      }
      set
      {
        fModel.ExcludedScanners = new ObservableCollection<BoolStringClass>();
        foreach(var l in value)
        {
          fModel.ExcludedScanners.Add(l.Copy());
        }
      }
    }

    public PdfSettings PdfSettings
    {
      get
      {
        return new PdfSettings
        {
          AttemptSingleImageMode = fModel.Pdf_AttemptSingleImageMode,
          ImportNativePages = fModel.Pdf_ImportNativePages,
          RenderResolution = fModel.Pdf_RenderResolution
        };
      }
      set
      {
        fModel.Pdf_AttemptSingleImageMode = value.AttemptSingleImageMode;
        fModel.Pdf_ImportNativePages = value.ImportNativePages;
        fModel.Pdf_RenderResolution = value.RenderResolution;
      }
    }

    public ExportSettings ExportSettings
    {
      get
      {
        return new ExportSettings
        {
          CompressImagesForPdfExport = fModel.Export_CompressImages,
          RemovePagesAfterPdfExport = fModel.Export_RemovePages,
          JpegCompressionFactor = fModel.Export_CompressionFactor
        };
      }
      set
      {
        fModel.Export_CompressImages = value.CompressImagesForPdfExport;
        fModel.Export_RemovePages = value.RemovePagesAfterPdfExport;
        fModel.Export_CompressionFactor = value.JpegCompressionFactor;
      }
    }


    public bool ExecuteDialog()
    {
      fModel.Raise();
      bool? result = this.ShowDialog();
      return result.HasValue ? result.Value : false;
    }


    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }

  public class BoolStringClass
  {
    public string TheText { get; set; }
    public bool IsSelected { get; set; }
    public BoolStringClass Copy()
    {
      var retval = new BoolStringClass();
      retval.TheText = this.TheText;
      retval.IsSelected = this.IsSelected;
      return retval;
    }
  }
}
