using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using Model;
using Defines;
using Utils;


namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for WindowMain.xaml
  /// </summary>
  public partial class WindowMain : Window
  {
    private AppSettings fAppSettings;
    private Scanner fScanner;
    private Printer fPrinter;
    private PdfExporter fPdfExporter;
    private PdfImporter fPdfImporter;
    private ImageLoader fImageLoader;
    private Document fDocument;
    private InsertionMark  fInsertionMark;
    private Point fDragStartPosition;

    private Cyotek.Windows.Forms.ImageBox PictureBoxPreview;


    public WindowMain()
    {
      InitializeComponent();

      fAppSettings = new AppSettings();

      fScanner = new Scanner();
      fScanner.OnScanningComplete += fScanner_OnScanningComplete;
      fPrinter = new Printer();
      fPdfExporter = new PdfExporter();
      fPdfImporter = new PdfImporter();
      fImageLoader = new ImageLoader();

      fDocument = new Document();
      fDocument.OnPageAdded += fDocument_OnPageAdded;
      fDocument.OnPageRemoved += fDocument_OnPageRemoved;
      fDocument.OnPageUpdated += fDocument_OnPageUpdated;
      fDocument.OnPageMoved += fDocument_OnPageMoved;
       
      fInsertionMark = new InsertionMark();
      fDragStartPosition = new Point(0, 0);
    }


    private void RibbonWin_Loaded(object sender, RoutedEventArgs e)
    {
      Grid child = VisualTreeHelper.GetChild((DependencyObject)sender, 0) as Grid;
      if(child != null)
      {
        child.RowDefinitions[0].Height = new GridLength(0);
      }
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ListViewPages.AllowDrop = true;
 
      PictureBoxPreview = new Cyotek.Windows.Forms.ImageBox();

      this.PictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PictureBoxPreview.TabStop = false;
      this.PictureBoxPreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

      RightBarWindowsFormsHost.Child = PictureBoxPreview;

      if(fScanner.Open())
      {
        fScanner.SelectActiveDataSource(fAppSettings.CurrentScanner);
      }

      if(String.IsNullOrEmpty(fScanner.GetActiveDataSourceName()))
      {
        ButtonScan.IsEnabled = false;
      }

      lblCursorPosition.Text = "";
      lblDragDropInfo.Text = "";
      RefreshControls();
    }


    void fScanner_OnScanningComplete(object sender, EventArgs e)
    {
      // Nothing to do
    }


    void RefreshControls()
    {
      lblCursorPosition.Text = ListViewPages.Items.Count + " items";
    }


    private void Scan(PageTypeEnum pageType)
    {
      ScanSettings settings = new ScanSettings();

      settings.EnableFeeder = fAppSettings.EnableFeeder;
      settings.ColorMode = fAppSettings.ColorMode;
      settings.PageType = pageType;// fAppSettings.PageType;
      settings.Resolution = fAppSettings.Resolution;
      settings.Threshold = fAppSettings.Threshold;
      settings.Brightness = fAppSettings.Brightness;
      settings.Contrast = fAppSettings.Contrast;

      if(fScanner.Acquire(fDocument, settings, fAppSettings.UseScannerNativeUI, true) == false)
      {
        Utils.Dialogs.ShowError("Scanner failed to start");
      }

      RefreshControls();
    }


    private void RenumberPages()
    {
      for(int i = 0; i < ListViewPages.Items.Count; i++)
      {
        ListViewPage listViewItem = (ListViewPage)ListViewPages.Items[i];
        listViewItem.Index = i;
      }
    }


    private void fDocument_OnPageAdded(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      ListViewPages.Items.Insert(args.Index, new ListViewPage(fDocument, args.Index));
      RefreshControls();
    }


    private void fDocument_OnPageRemoved(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;

      if(args.Index >= 0)
      {
        // Remove page from ListView
        ListViewPages.Items.RemoveAt(args.Index);
        RenumberPages();
      }
      else
      {
        // Negative index, all pages are removed
        ListViewPages.Items.Clear();
      }

      RefreshControls();
    }


    private void fDocument_OnPageUpdated(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      ListViewPage listViewItem = (ListViewPage)ListViewPages.Items[args.Index];
      listViewItem.RefreshIcon();
      ListViewPages_SelectionChanged(null, null);
      RefreshControls();
    }


    private void fDocument_OnPageMoved(object sender, EventArgs e)
    {
      DocumentPageMoveEventArgs args = (DocumentPageMoveEventArgs)e;

      int firstIndex;
      int lastIndex;

      if(args.SourceIndex < args.TargetIndex)
      {
        firstIndex = args.SourceIndex;
        lastIndex = args.TargetIndex;
      }
      else
      {
        firstIndex = args.TargetIndex;
        lastIndex = args.SourceIndex;
      }

      for(int i = firstIndex; i <= lastIndex; i++)
      {
        ListViewPage listViewItem = (ListViewPage)ListViewPages.Items[i];
        listViewItem.Refresh();
      }

      ListViewPages.SelectedIndex = args.TargetIndex;
      RefreshControls();
    }
    

    private ListViewPage ListViewSelectedItem
    {
      get { return (ListViewPage)ListViewPages.SelectedItem; }
    }


    private void ListViewPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ListViewPage item = ListViewSelectedItem;

      if(item == null)
      {
        // No picture is selected
        PictureBoxPreview.Image = null;
      }
      else
      {
        Model.Page page = fDocument.GetPage(item.Index);
        PictureBoxPreview.Image = page.GetLayoutImage();
        PictureBoxPreview.ZoomToFit();

        lblDragDropInfo.Text = "" + item.PageNumber;
      }
    }


    private void ButtonScanLetter_Click(object sender, EventArgs e)
    {
      Scan(PageTypeEnum.Letter);
    }


    private void ButtonScanLegal_Click(object sender, EventArgs e)
    {
      Scan(PageTypeEnum.Legal);
    }


    private void ButtonLoadImages_Click(object sender, EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

      // Set the file dialog to filter for graphics files. 
      openFileDialog1.Filter =
          "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|" +
          "All files (*.*)|*.*";

      // Allow the user to select multiple images. 
      openFileDialog1.Multiselect = true;
      openFileDialog1.Title = "My Image Browser";

      if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        fImageLoader.LoadImagesFromFiles(fDocument, openFileDialog1.FileNames);
      }

      RefreshControls();
    }


    private void ButtonLoadPdf_Click(object sender, EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

      // Set the file dialog to filter for graphics files. 
      openFileDialog1.Filter =
          "PDF (*.PDF)|*.PDF";

      openFileDialog1.Multiselect = false;
      openFileDialog1.Title = "Select a PDF File";

      if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        fPdfImporter.LoadDocument(fDocument, openFileDialog1.FileName);
      }

      RefreshControls();
    }


    private void ButtonSavePdf_Click(object sender, EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

      if(!System.IO.Directory.Exists(fAppSettings.LastDirectory))
      {
        fAppSettings.LastDirectory = "M:\\";
      }

      saveFileDialog1.InitialDirectory = fAppSettings.LastDirectory;
      saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
      saveFileDialog1.FilterIndex = 1;

      if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string fileName = saveFileDialog1.FileName;

        fPdfExporter.SaveDocument(fDocument, fileName);

        Thread.Sleep(5000);

//        fDocument.RemoveAll();

        fAppSettings.LastDirectory = System.IO.Path.GetDirectoryName(fileName);
      }
    }


    private void ButtonPrint_Click(object sender, EventArgs e)
    {
      if(fDocument.NumPages > 0)
      {
        System.Windows.Forms.PrintDialog dlg = new System.Windows.Forms.PrintDialog();
        dlg.AllowSomePages = true;
        dlg.UseEXDialog = true;
        dlg.PrinterSettings.PrinterName = "PrimoPDF";
        dlg.PrinterSettings.MinimumPage = 1;
        dlg.PrinterSettings.MaximumPage = fDocument.NumPages;
        dlg.PrinterSettings.FromPage = dlg.PrinterSettings.MinimumPage;
        dlg.PrinterSettings.ToPage = dlg.PrinterSettings.MaximumPage;

        if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          //if(MenuSettingsPrinterUsePreview.Checked)
          //{
          fPrinter.PreviewDocument(fDocument, dlg.PrinterSettings);
          //}
          //else
          //{
          //  fPrinter.PrintDocument(fDocument, dlg.PrinterSettings);
          //}
        }
      }
    }


    private void ButtonRotateClockwise_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        fDocument.RotatePageClockwise(ListViewSelectedItem.Index);
      }
    }


    private void ButtonRotateCounterClockwise_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        fDocument.RotatePageCounterClockwise(ListViewSelectedItem.Index);
      }
    }


    private void ButtonMirrorHorizontally_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        fDocument.MirrorPageHorizontally(ListViewSelectedItem.Index);
      }
    }


    private void ButtonMirrorVertically_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        fDocument.MirrorPageVertically(ListViewSelectedItem.Index);
      }
    }


    private void ButtonLandscape_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        fDocument.LandscapePage(ListViewSelectedItem.Index);
      }
    }


    private void Button2Sided_Click(object sender, EventArgs e)
    {
      if(MessageBoxResult.OK == MessageBox.Show("This will sort the images in front/back order", "Confirm", MessageBoxButton.YesNo))
      {
        fDocument.RearrangePages2Sided();
      }
    }


    private void ButtonDelete_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        if(MessageBoxResult.OK == MessageBox.Show("Delete selected image?", "Delete?", MessageBoxButton.OKCancel))
        {
          fDocument.DeletePage(ListViewSelectedItem.Index);
        }
      }
    }


    private void ButtonDeleteAll_Click(object sender, EventArgs e)
    {
      if(MessageBoxResult.OK == MessageBox.Show("Remove all pages?", "Delete?", MessageBoxButton.OKCancel))
      {
        fDocument.RemoveAll();
      }
    }


    private void ListViewPages_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      fDragStartPosition = e.GetPosition(null);
    }

    
    private void ListViewPages_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      System.Windows.Controls.ListView listView = sender as System.Windows.Controls.ListView;
      if((listView != null) && (listView.SelectedItem != null) && (e.LeftButton == MouseButtonState.Pressed))
      {
        Point mousePos = e.GetPosition(null);
        Vector diff = fDragStartPosition - mousePos;

        if(diff.Length > 15)
        {
          DragDrop.DoDragDrop(listView, listView.SelectedItem, DragDropEffects.All);
        }
      }
    }


    private void ListViewPages_DragEnter(object sender, System.Windows.DragEventArgs e)
    {
      ListViewPage sourceData = (ListViewPage)e.Data.GetData(typeof(ListViewPage));

      Point pt = e.GetPosition(ListViewPages);
      ListViewItem item = WpfAssist.GetListViewItemAtPoint(ListViewPages, pt);

      if((sourceData != null) && (item != null))
      {
        int targetIndex = ((ListViewPage)item.Content).Index;
        int sourceIndex = sourceData.Index;

        if(targetIndex != sourceIndex)
        {
          fInsertionMark.Show(item, targetIndex < sourceIndex);
        }
        else
        {
          fInsertionMark.Hide();
        }
      }

      e.Handled = true;
    }


    private void ListViewPages_DragLeave(object sender, System.Windows.DragEventArgs e)
    {
      fInsertionMark.Hide();
    }


    private void ListViewPages_DragOver(object sender, System.Windows.DragEventArgs e)
    {
      // Auto-scroll list view
      ListView li = sender as ListView;
      ScrollViewer sv = WpfAssist.FindVisualChild<ScrollViewer>(ListViewPages);

      double tolerance = 10;
      double verticalPos = e.GetPosition(li).Y;
      double offset = 3;

      if(verticalPos < tolerance) // Top of visible list?
      {
        sv.ScrollToVerticalOffset(sv.VerticalOffset - offset); //Scroll up.
      }
      else if(verticalPos > li.ActualHeight - tolerance) //Bottom of visible list?
      {
        sv.ScrollToVerticalOffset(sv.VerticalOffset + offset); //Scroll down.    
      }

      // Enable/disable drop
      if(e.Data.GetDataPresent(typeof(ListViewPage)))
      {
        if(fInsertionMark.IsVisible)
        {
          e.Effects = DragDropEffects.Move;
        }
        else 
        {
          e.Effects = DragDropEffects.None;
        }
      }
      else if(e.Data.GetDataPresent(DataFormats.FileDrop, false))
      {
        e.Effects = DragDropEffects.Copy;
      }
      else
      {
        e.Effects = DragDropEffects.None;
      }
      e.Handled = true;
    }


    private void ListViewPages_Drop(object sender, System.Windows.DragEventArgs e)
    {
      int targetIndex = fInsertionMark.Index;

      if(targetIndex >= 0)
      {
        ListViewPage draggedItem = (ListViewPage)e.Data.GetData(typeof(ListViewPage));

        if(draggedItem != null)
        {
          //lblDragDropInfo.Text = "Drop" + draggedItem.Index + " : " + targetIndex;
          fDocument.MovePage(draggedItem.Index, targetIndex);
        }
      }

      fInsertionMark.Hide();

      string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

      if(fileNames != null)
      {
        foreach(string filename in fileNames)
        {
          string ext = System.IO.Path.GetExtension(filename).ToUpper();

          if(ext == ".PDF")
          {
            fPdfImporter.LoadDocument(fDocument, filename);
          }
          else if((ext == ".BMP") || (ext == ".JPG") || (ext == ".GIF") || (ext == ".PNG"))
          {
            fImageLoader.LoadFromFile(fDocument, filename);
          }
          else
          {
            // Not a valid file
          }
        }
      }
    }
  }


  public class ListViewPage : INotifyPropertyChanged
  {
    private Document fPage;
    private int fIndex;

    public ListViewPage(Document doc, int index)
    {
      fPage = doc;
      fIndex = index;
    }

    public void Refresh()
    {
      OnPropertyChanged("PageNumber");
      OnPropertyChanged("Icon");
      OnPropertyChanged("Info");
    }

    public void RefreshIcon()
    {
      OnPropertyChanged("Icon");
    }

    public int Index
    {
      get { return fIndex; }
      set { if(fIndex != value) { fIndex = value; OnPropertyChanged("PageNumber"); } }
    }

    public int PageNumber
    {
      get { return fIndex + 1; }
    }

    public System.Drawing.Image Icon 
    {
      get 
      {
        Model.Page page = fPage.GetPage(fIndex);
        return page.LayoutThumbnail; 
      }
    }

    public string Info
    {
      get
      {
        Model.Page page = fPage.GetPage(fIndex);
        return page.Name;
      }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      if(PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
  }


  class InsertionMark
  {
    private InsertionMarkAdorner fAdorner;
    private ListViewItem fItem;

    public InsertionMark()
    {
      fAdorner = null;
      fItem = null;
    }

    public int Index
    {
      get 
      {
        int result = -1;

        if(fItem != null)
        {
          result = ((ListViewPage)fItem.Content).Index;
        }

        return result;
      }
    }

    public void Show(ListViewItem item, bool before)
    {
      Hide();
      fItem = item;
      fAdorner = new InsertionMarkAdorner(fItem, before);
    }

    public void Hide()
    {
      if(fAdorner != null)
      {
        fAdorner.Close(fItem);
        fAdorner = null;
        fItem = null;
      }
    }

    public bool IsVisible
    {
      get { return (fAdorner != null); }
    }
  }


  class InsertionMarkAdorner : Adorner
  {
    private bool _before;

    public InsertionMarkAdorner(ListViewItem item, bool before) :
      base(item)
    {
      _before = before;
      AdornerLayer.GetAdornerLayer(item).Add(this);
    }

    public void Close(ListViewItem item)
    {
      AdornerLayer.GetAdornerLayer(item).Remove(this);
    }

    protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
    {
      if(_before)
      {
        drawingContext.DrawRectangle(System.Windows.Media.Brushes.Gray, null, new System.Windows.Rect(0, 0, ActualWidth, 3));
      }
      else
      {
        drawingContext.DrawRectangle(System.Windows.Media.Brushes.Gray, null, new System.Windows.Rect(0, ActualHeight - 3, ActualWidth, 3));
      }
    }
  }
}
