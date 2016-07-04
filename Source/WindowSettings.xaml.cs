using System;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;
using Utils;


namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for WindowSettings.xaml
  /// </summary>
  public partial class WindowSettings : Window
  {
    private AppSettings fAppSettings;
    private int fRow;

    // UI Elements
    private DoubleUpDown UpDownDoubleCustomPageWidth;
    private DoubleUpDown UpDownDoubleCustomPageHeight;
    private ComboBox ComboBoxScannerColorMode;
    private IntegerUpDown UpDownIntegerScannerResolution;
    private DoubleUpDown UpDownDoubleScannerThreshold;
    private DoubleUpDown UpDownDoubleScannerBrightness;
    private DoubleUpDown UpDownDoubleScannerContrast;
    private IntegerUpDown UpDownIntegerScannerCompressionFactor;
    private DoubleUpDown UpDownDoubleDefaultPageWidth;
    private DoubleUpDown UpDownDoubleDefaultPageHeight;
    private ComboBox ComboBoxPageScaling;
    private IntegerUpDown UpDownIntegerDpiPdfRendering;
    private IntegerUpDown UpDownIntegerDpiPdfExport;
    private IntegerUpDown UpDownIntegerExportCompressionFactor;
    private ComboBox ComboBoxPdfImportAction;
    private CheckBox CheckBoxPdfImageImport;
    private CheckBox CheckBoxRemovePagesPdfExport;
    private CheckBox CheckBoxShowPrintButton;


    public WindowSettings(AppSettings appSettings)
    {
      InitializeComponent();
      BuildUI();
      fAppSettings = appSettings;
      fRow = 0;
      SettingsToUI();
    }


    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      UIToSettings();
      this.DialogResult = true;
    }


    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

    
    private void SettingsToUI()
    {
      SizeInches customPageSize = fAppSettings.ScannerCustomPageSize;
      UpDownDoubleCustomPageWidth.Value = customPageSize.Width;
      UpDownDoubleCustomPageHeight.Value = customPageSize.Height;

      SizeInches defaultPageSize = fAppSettings.DefaultPageSize;
      UpDownDoubleDefaultPageWidth.Value = defaultPageSize.Width;
      UpDownDoubleDefaultPageHeight.Value = defaultPageSize.Height;

      switch(fAppSettings.ScannerColorMode)
      {
        default:
        case Defines.ColorModeEnum.BW: ComboBoxScannerColorMode.SelectedIndex = 0; break;
        case Defines.ColorModeEnum.Gray: ComboBoxScannerColorMode.SelectedIndex = 1; break;
        case Defines.ColorModeEnum.RGB: ComboBoxScannerColorMode.SelectedIndex = 2; break;
      }

      UpDownIntegerScannerResolution.Value = fAppSettings.ScannerResolution;
      UpDownDoubleScannerThreshold.Value = fAppSettings.ScannerThreshold;
      UpDownDoubleScannerBrightness.Value = fAppSettings.ScannerBrightness;
      UpDownDoubleScannerContrast.Value = fAppSettings.ScannerContrast;
      UpDownIntegerScannerCompressionFactor.Value = fAppSettings.ScannerCompressionFactor;

      switch(fAppSettings.PageScaling)
      {
        default:
        case Defines.PageScalingEnum.UseDpi: ComboBoxPageScaling.SelectedIndex = 0; break;
        case Defines.PageScalingEnum.ShrinkOnly: ComboBoxPageScaling.SelectedIndex = 1; break;
        case Defines.PageScalingEnum.StretchShrink: ComboBoxPageScaling.SelectedIndex = 2; break;
      }

      UpDownIntegerDpiPdfRendering.Value = fAppSettings.PdfViewingResolution;
      UpDownIntegerDpiPdfExport.Value = fAppSettings.PdfExportResolution;
      UpDownIntegerExportCompressionFactor.Value = fAppSettings.ExportCompressionFactor;
      ComboBoxPdfImportAction.SelectedIndex = (fAppSettings.AlwaysNativePdfImport) ? 0 : 1;
      CheckBoxPdfImageImport.IsChecked = fAppSettings.AttemptPdfSingleImageImport;
      CheckBoxRemovePagesPdfExport.IsChecked = fAppSettings.RemovePagesAfterPdfExport;
      CheckBoxShowPrintButton.IsChecked = fAppSettings.ShowPrintButton;
    }


    private void UIToSettings()
    {
      SizeInches customPageSize = new SizeInches((double)UpDownDoubleCustomPageWidth.Value, (double)UpDownDoubleCustomPageHeight.Value);
      fAppSettings.ScannerCustomPageSize = customPageSize;

      SizeInches defaultPageSize = new SizeInches((double)UpDownDoubleDefaultPageWidth.Value, (double)UpDownDoubleDefaultPageHeight.Value);
      fAppSettings.DefaultPageSize = defaultPageSize;

      switch(ComboBoxScannerColorMode.SelectedIndex)
      {
        default:
        case 0: fAppSettings.ScannerColorMode = Defines.ColorModeEnum.BW; break;
        case 1: fAppSettings.ScannerColorMode = Defines.ColorModeEnum.Gray; break;
        case 2: fAppSettings.ScannerColorMode = Defines.ColorModeEnum.RGB; break;
      }

      fAppSettings.ScannerResolution = (int)UpDownIntegerScannerResolution.Value;
      fAppSettings.ScannerThreshold = (double)UpDownDoubleScannerThreshold.Value;
      fAppSettings.ScannerBrightness = (double)UpDownDoubleScannerBrightness.Value;
      fAppSettings.ScannerContrast = (double)UpDownDoubleScannerContrast.Value;
      fAppSettings.ScannerCompressionFactor = (int)UpDownIntegerScannerCompressionFactor.Value;

      switch(ComboBoxPageScaling.SelectedIndex)
      {
        default:
        case 0: fAppSettings.PageScaling = Defines.PageScalingEnum.UseDpi; break;
        case 1: fAppSettings.PageScaling = Defines.PageScalingEnum.ShrinkOnly; break;
        case 2: fAppSettings.PageScaling = Defines.PageScalingEnum.StretchShrink; break;
      }

      fAppSettings.PdfViewingResolution = (int)UpDownIntegerDpiPdfRendering.Value;
      fAppSettings.PdfExportResolution = (int)UpDownIntegerDpiPdfExport.Value;
      fAppSettings.ExportCompressionFactor = (int)UpDownIntegerExportCompressionFactor.Value;
      fAppSettings.AlwaysNativePdfImport = (ComboBoxPdfImportAction.SelectedIndex == 0);
      fAppSettings.AttemptPdfSingleImageImport = (bool)CheckBoxPdfImageImport.IsChecked;
      fAppSettings.RemovePagesAfterPdfExport = (bool)CheckBoxRemovePagesPdfExport.IsChecked;
      fAppSettings.ShowPrintButton = (bool)CheckBoxShowPrintButton.IsChecked;
    }


    private void BuildUI()
    {
      AddCategory("Scanner Settings");
      UpDownDoubleCustomPageWidth = AddDouble("Custom Page Size:", "Width:", 0, 10, "inches");
      UpDownDoubleCustomPageHeight = AddDouble("", "Height:", 0, 10, "inches");
      List<string> colorMode = new List<string>() { "Black/White", "Grayscale", "Color" };
      ComboBoxScannerColorMode = AddList("Color Mode:", "", colorMode, "");
      UpDownIntegerScannerResolution = AddInteger("Resolution:", "", 0, 10, "DPI");
      UpDownDoubleScannerThreshold = AddDouble("Threshold:", "", 0, 10, "");
      UpDownDoubleScannerBrightness = AddDouble("Brightness:", "", 0, 10, "");
      UpDownDoubleScannerContrast = AddDouble("Contrast:", "", 0, 10, "");
      UpDownIntegerScannerCompressionFactor = AddInteger("Compression Factor:", "", 0, 10, "");
      AddCategory("Program Settings");
      UpDownDoubleDefaultPageWidth = AddDouble("Default Page Size (for image import):", "Width:", 0, 10, "inches");
      UpDownDoubleDefaultPageHeight = AddDouble("", "Height:", 0, 10, "inches");
      List<string> pageScaling = new List<string>() { "Use DPI", "Fit (Only Shrink)", "Fit(Stretch / Shrink)" };
      ComboBoxPageScaling = AddList("Page Scaling:", "", pageScaling, "");
      UpDownIntegerDpiPdfRendering = AddInteger("PDF Viewing Resolution:", "", 0, 10, "DPI");
      UpDownIntegerDpiPdfExport = AddInteger("PDF Export Resolution:", "", 0, 10, "DPI");
      UpDownIntegerExportCompressionFactor = AddInteger("Export Compression Factor:", null, 0, 10, "");
      List<string> pdfImportAction = new List<string>() { "Always Native", "Always Render" };
      ComboBoxPdfImportAction = AddList("PDF Import Action:", "", pdfImportAction, "");
      CheckBoxPdfImageImport = AddCheck("Attempt PDF Image Import:", "", "");
      CheckBoxRemovePagesPdfExport = AddCheck("Remove Pages on PDF Save:", "", "");
      CheckBoxShowPrintButton = AddCheck("Show Print Button:", "", "");
    }


    private ComboBox AddList(string title, string subTitle, List<string> values, string unit)
    {
      ComboBox element = new ComboBox();
      element.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
      element.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
      element.Margin = new Thickness(10, 3, 0, 3);
      element.SelectedIndex = 0;

      foreach(string item in values)
      {
        element.Items.Add(item);
      }

      AddRow(title, subTitle, element, unit);
      return element;
    }


    private DoubleUpDown AddDouble(string title, string subTitle, double min, double max, string unit)
    {
      DoubleUpDown element = new DoubleUpDown();
      element.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
      element.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
      element.Margin = new Thickness(10, 3, 0, 3);
      element.Minimum = min;
      element.Maximum = max;
      element.Increment = 0.01;
      element.FormatString = "0.##";
      element.MouseWheelActiveTrigger = Xceed.Wpf.Toolkit.Primitives.MouseWheelActiveTrigger.Disabled;

      AddRow(title, subTitle, element, unit);
      return element;
    }


    private IntegerUpDown AddInteger(string title, string subTitle, int min, int max, string unit)
    {
      IntegerUpDown element = new IntegerUpDown();
      element.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
      element.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
      element.Margin = new Thickness(10, 3, 0, 3);
      element.Minimum = min;
      element.Maximum = max;

      AddRow(title, subTitle, element, unit);
      return element;
    }


    private CheckBox AddCheck(string title, string subTitle, string unit)
    {
      CheckBox element = new CheckBox();
      element.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      element.VerticalAlignment = System.Windows.VerticalAlignment.Center;
      element.Margin = new Thickness(10, 3, 0, 3);

      AddRow(title, subTitle, element, unit);
      return element;
    }


    private void AddCategory(string title)
    {
      RowDefinition row = new RowDefinition();
      row.Height = new GridLength(40);
      InfoGrid.RowDefinitions.Add(row);

      Label labelTitle = new Label();
      labelTitle.Content = title;
      labelTitle.FontSize = 12;
      labelTitle.FontWeight = FontWeights.Bold;
      labelTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      labelTitle.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
      labelTitle.Margin = new Thickness(0, 2, 10, 2);
      Grid.SetRow(labelTitle, fRow);
      Grid.SetColumn(labelTitle, 0);
      InfoGrid.Children.Add(labelTitle);

      fRow++;
    }


    private void AddRow(string title, string subTitle, UIElement element, string unit)
    {
      RowDefinition row = new RowDefinition();
      row.Height = new GridLength(27);
      InfoGrid.RowDefinitions.Add(row);

      Label labelTitle = new Label();
      labelTitle.Content = title;
      labelTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
      labelTitle.VerticalAlignment = System.Windows.VerticalAlignment.Center;
      Grid.SetRow(labelTitle, fRow);
      Grid.SetColumn(labelTitle, 0);
      InfoGrid.Children.Add(labelTitle);

      if(String.IsNullOrEmpty(subTitle))
      {
        Grid.SetRow(element, fRow);
        Grid.SetColumn(element, 1);
        Grid.SetColumnSpan(element, 2);
        InfoGrid.Children.Add(element);
      }
      else
      {
        Label labelSubTitle = new Label();
        labelSubTitle.Content = subTitle;
        labelSubTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        labelSubTitle.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        Grid.SetRow(labelSubTitle, fRow);
        Grid.SetColumn(labelSubTitle, 1);
        InfoGrid.Children.Add(labelSubTitle);

        Grid.SetRow(element, fRow);
        Grid.SetColumn(element, 2);
        Grid.SetColumnSpan(element, 1);
        InfoGrid.Children.Add(element);
      }

      Label labelUnit = new Label();
      labelUnit.Content = unit;
      labelUnit.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      labelUnit.VerticalAlignment = System.Windows.VerticalAlignment.Center;
      Grid.SetRow(labelUnit, fRow);
      Grid.SetColumn(labelUnit, 4);
      InfoGrid.Children.Add(labelUnit);

      fRow++;
    }
  }
}
