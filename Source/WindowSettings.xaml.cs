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
using Utils;

namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for WindowSettings.xaml
  /// </summary>
  public partial class WindowSettings : Window
  {
    private AppSettings fAppSettings;

    public WindowSettings(AppSettings appSettings)
    {
      InitializeComponent();

      fAppSettings = appSettings;

      SizeInches customPageSize = fAppSettings.CustomPageSize;
      UpDownDoubleCustomPageWidth.Value = customPageSize.Width;
      UpDownDoubleCustomPageHeight.Value = customPageSize.Height;

      SizeInches defaultPageSize = fAppSettings.DefaultPageSize;
      UpDownDoubleDefaultPageWidth.Value = defaultPageSize.Width;
      UpDownDoubleDefaultPageHeight.Value = defaultPageSize.Height;

      switch(fAppSettings.ColorMode)
      {
        default:
        case Defines.ColorModeEnum.BW: ComboBoxScannerColorMode.SelectedIndex = 0; break;
        case Defines.ColorModeEnum.Gray: ComboBoxScannerColorMode.SelectedIndex = 1; break;
        case Defines.ColorModeEnum.RGB: ComboBoxScannerColorMode.SelectedIndex = 2; break;
      }

      UpDownIntegerScannerResolution.Value = fAppSettings.Resolution;
      UpDownDoubleScannerThreshold.Value = fAppSettings.Threshold;
      UpDownDoubleScannerBrightness.Value = fAppSettings.Brightness;
      UpDownDoubleScannerContrast.Value = fAppSettings.Contrast;
      UpDownIntegerScannerCompressionFactor.Value = fAppSettings.ScannerCompressionFactor;

      switch (fAppSettings.PageScaling)
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

    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      SizeInches customPageSize = new SizeInches((double)UpDownDoubleCustomPageWidth.Value, (double)UpDownDoubleCustomPageHeight.Value);
      fAppSettings.CustomPageSize = customPageSize;

      SizeInches defaultPageSize = new SizeInches((double)UpDownDoubleDefaultPageWidth.Value, (double)UpDownDoubleDefaultPageHeight.Value);
      fAppSettings.DefaultPageSize = defaultPageSize;

      switch (ComboBoxScannerColorMode.SelectedIndex)
      {
        default:
        case 0: fAppSettings.ColorMode = Defines.ColorModeEnum.BW; break;
        case 1: fAppSettings.ColorMode = Defines.ColorModeEnum.Gray; break;
        case 2: fAppSettings.ColorMode = Defines.ColorModeEnum.RGB; break;
      }

      fAppSettings.Resolution = (int)UpDownIntegerScannerResolution.Value;
      fAppSettings.Threshold = (double)UpDownDoubleScannerThreshold.Value;
      fAppSettings.Brightness = (double)UpDownDoubleScannerBrightness.Value;
      fAppSettings.Contrast = (double)UpDownDoubleScannerContrast.Value;
      fAppSettings.ScannerCompressionFactor = (int)UpDownIntegerScannerCompressionFactor.Value;

      switch (ComboBoxPageScaling.SelectedIndex)
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

      this.DialogResult = true;
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }
  }
}
