using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Source;
using Model;
using Utils;


namespace PDFScanningApp
{
  public partial class FormMain : Form
  {
    private AppSettings fAppSettings;
    private Scanner fScanner;
    private Printer fPrinter;
    private PdfExporter fPdfExporter;
    private PdfImporter fPdfImporter;
    private ImageLoader fImageLoader;
    private Document fDocument;

    // Special UI
    private Cyotek.Windows.Forms.ImageBox PictureBoxPreview;

    
    public FormMain()
    {
      InitializeComponent();
      UtilDialogs.MainWindow = this.Handle;
      this.Text = UtilApp.GetApplicationName();

      // Create PictureBoxPreview from special component;
      this.PictureBoxPreview = new Cyotek.Windows.Forms.ImageBox();
      this.PictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PictureBoxPreview.TabStop = false;
      this.PictureBoxPreview.BorderStyle = BorderStyle.None;
      this.PanelPreview.Controls.Add(this.PictureBoxPreview);

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

      MenuSettingsPrinterUsePreview.Checked = true;
    }


    private void FormMain_Load(object sender, EventArgs e)
    {
      if(fScanner.Open())
      {
        foreach(string item in fScanner.GetDataSourceNames())
        {
          ComboBoxScanners.Items.Add(item);
        }

        for(int i = 0; i < ComboBoxScanners.Items.Count; i++)
        {
          if(ComboBoxScanners.Items[i].ToString() == fAppSettings.CurrentScanner)
          {
            ComboBoxScanners.SelectedIndex = i;
          }
        }
      }

      StatusLabel1.Text = "";
      StatusLabel2.Text = "";
      RefreshControls();
    }


    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      fScanner.Close();
    }


    void fScanner_OnScanningComplete(object sender, EventArgs e)
    {
      // Nothing to do
    }


    void RefreshControls()
    {
      StatusLabel1.Text = ListViewPages.Items.Count + " items";
    }


    private void RefreshScanner()
    {
      string selectedScanner = (string)ComboBoxScanners.SelectedItem;
      fScanner.SelectActiveDataSource(selectedScanner);
      fAppSettings.CurrentScanner = fScanner.GetActiveDataSourceName();
    }


    private bool ExecuteDataSourceSettingsDialog()
    {
      bool result = false;

      RefreshScanner();

      string dataSourceName = fScanner.GetActiveDataSourceName();

      if(String.IsNullOrEmpty(dataSourceName) == false)
      {
        FormDataSourceSettingsDialog F = new FormDataSourceSettingsDialog(dataSourceName);

        F.SetAvailableValuesForColorMode(fScanner.GetAvailableValuesForColorMode());
        F.SetAvailableValuesForPageType(fScanner.GetAvailableValuesForPageType());
        F.SetAvailableValuesForResolution(fScanner.GetAvailableValuesForResolution());

        F.UseNativeUI = fAppSettings.UseScannerNativeUI;
        F.EnableFeeder = fAppSettings.EnableFeeder;
        F.ColorMode = fAppSettings.ColorMode;
        F.PageType = fAppSettings.PageType;
        F.Resolution = fAppSettings.Resolution;
        F.Threshold = fAppSettings.Threshold;
        F.Brightness = fAppSettings.Brightness;
        F.Contrast = fAppSettings.Contrast;

        DialogResult dr = F.ShowDialog();

        if(dr == DialogResult.OK)
        {
          fAppSettings.UseScannerNativeUI = F.UseNativeUI;
          fAppSettings.EnableFeeder = F.EnableFeeder;
          fAppSettings.ColorMode = F.ColorMode;
          fAppSettings.PageType = F.PageType;
          fAppSettings.Resolution = F.Resolution;
          fAppSettings.Threshold = F.Threshold;
          fAppSettings.Brightness = F.Brightness;
          fAppSettings.Contrast = F.Contrast;

          result = true;
        }
      }

      return result;
    }


    private void Scan(PageTypeEnum pageType)
    {
      RefreshScanner();

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
        UtilDialogs.ShowError("Scanner failed to start");
      }

      RefreshControls();
    }

    
    private void fDocument_OnPageAdded(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      ListViewPages.Items.Insert(args.Index, new ListViewItem());
      RefreshControls();
    }


    private void fDocument_OnPageRemoved(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;

      if(args.Index >= 0)
      {
        // Remove page from ListView
        ListViewPages.Items.RemoveAt(args.Index);
      }
      else
      {
        // Negative index, all pages are removed
        ListViewPages.Items.Clear();
        // This does not get called on clear even though items are unselected
        ListViewPages_SelectedIndexChanged(sender, e);
      }

      RefreshControls();
    }


    private void fDocument_OnPageUpdated(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      ListViewPages.RedrawItems(args.Index, args.Index, true);
      ListViewPages_SelectedIndexChanged(sender, e);
      RefreshControls();
    }


    private void fDocument_OnPageMoved(object sender, EventArgs e)
    {
      DocumentPageMoveEventArgs args = (DocumentPageMoveEventArgs)e;

      if(args.SourceIndex < args.TargetIndex)
      {
        ListViewPages.RedrawItems(args.SourceIndex, args.TargetIndex, true);
      }
      else
      {
        ListViewPages.RedrawItems(args.TargetIndex, args.SourceIndex, true);
      }

      ListViewPages.Items[args.TargetIndex].Selected = true;
      RefreshControls();
    }


    private ListViewItem ListViewSelectedItem
    {
      get
      {
        ListViewItem result = null;

        if(ListViewPages.SelectedItems.Count > 0)
        {
          result = ListViewPages.SelectedItems[0];
        }

        return result;
      }
    }


    private void ListViewPages_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListViewItem item = ListViewSelectedItem;

      if(item == null)
      {
        // No picture is selected
        PictureBoxPreview.Image = null;
      }
      else
      {
        Page page = fDocument.GetPage(item.Index);
        PictureBoxPreview.Image = GetPageImage(page.GetImage(), page.Bounds, page.ImageBounds);
        PictureBoxPreview.ZoomToFit();
      }
    }


    private Bitmap GetPageImage(Image img, Rectangle pageRect, Rectangle imageRect)
    {
      Bitmap pageBitmap = new Bitmap(pageRect.Width, pageRect.Height);

      using(Graphics g = Graphics.FromImage(pageBitmap))
      {
        g.FillRectangle(Brushes.White, pageRect);
        g.DrawImage(img, imageRect);
      }

      return pageBitmap;
    }


    private void ListViewPages_Layout(object sender, LayoutEventArgs e)
    {
      ListViewPages.Columns[0].Width = ListViewPages.ClientSize.Width;
    }


    private void ListViewPages_ItemDrag(object sender, ItemDragEventArgs e)
    {
      ListViewPages.DoDragDrop(e.Item, DragDropEffects.Move);
    }


    private void ListViewPages_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }


    private void ListViewPages_DragLeave(object sender, EventArgs e)
    {
      ListViewPages.InsertionMark.Index = -1;
    }


    private void ListViewPages_DragOver(object sender, DragEventArgs e)
    {
      ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
      Point targetPoint = ListViewPages.PointToClient(new Point(e.X, e.Y));
      int targetIndex = ListViewPages.InsertionMark.NearestIndex(targetPoint);

      ListViewPages.InsertionMark.Index = targetIndex;

      if(targetIndex > draggedItem.Index)
      {
        ListViewPages.InsertionMark.AppearsAfterItem = true;
      }
      else
      {
        ListViewPages.InsertionMark.AppearsAfterItem = false;
      }
      
      if(targetIndex >= 0)
      {
        if(targetIndex == ListViewPages.TopItem.Index)
        {
          if(targetIndex > 0)
          {
            ListViewPages.Items[targetIndex - 1].EnsureVisible();
          }
        }
        else
        {
          ListViewPages.Items[targetIndex].EnsureVisible();
        }
      }

      StatusLabel2.Text = "Page " + (draggedItem.Index + 1) + " >> " + (targetIndex + 1);
    }


    private void ListViewPages_DragDrop(object sender, DragEventArgs e)
    {
      int targetIndex = ListViewPages.InsertionMark.Index;

      if(targetIndex >= 0)
      {
        ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
        fDocument.MovePage(draggedItem.Index, targetIndex);
      }

      // Clear the insertion mark and the status
      ListViewPages.InsertionMark.Index = -1;
      StatusLabel2.Text = "";
    }


    private void ListViewPages_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
      if(ListViewPages.Items.Count > 0)
      {
        // if selected, mark the background differently
        if(e.Item.Selected)
        {
          e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);
        }

        int margin = 5;
        int maxSize = e.Bounds.Height - 2 * margin;

        Rectangle rectPic = new Rectangle( e.Bounds.X + e.Bounds.Width / 5,
                                           e.Bounds.Y + margin,
                                           maxSize,
                                           maxSize);

        int infoColumnLeft = rectPic.X + rectPic.Width + 10;

        Rectangle rectLine1 = new Rectangle( infoColumnLeft,
                                             rectPic.Y,
                                             e.Bounds.Width - infoColumnLeft - margin,
                                             ListViewPages.Font.Height + 5);

        Rectangle rectLine2 = new Rectangle( infoColumnLeft,
                                             rectPic.Y + rectPic.Height / 3,
                                             e.Bounds.Width - infoColumnLeft - margin,
                                             ListViewPages.Font.Height + 5);

        Page page = fDocument.GetPage(e.Item.Index);

        Rectangle miniPageBounds = UtilImaging.FitToArea(page.Bounds.Width, page.Bounds.Height, rectPic);
        Rectangle miniImageBounds = UtilImaging.FitToArea(page.ImageBounds.Width, page.ImageBounds.Height, miniPageBounds);

        e.Graphics.FillRectangle(Brushes.White, miniPageBounds);
        e.Graphics.DrawImage(page.Thumbnail, miniImageBounds);
        e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), miniPageBounds);
        e.Graphics.DrawString("Page " + (e.Item.Index + 1), ListViewPages.Font, Brushes.Black, rectLine1);
        e.Graphics.DrawString("Details", ListViewPages.Font, Brushes.DarkGray, rectLine2);
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
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

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
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

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
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();

      if(!Directory.Exists(fAppSettings.LastDirectory))
      {
        fAppSettings.LastDirectory = "M:\\";
      }

      saveFileDialog1.InitialDirectory = fAppSettings.LastDirectory;
      saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
      saveFileDialog1.FilterIndex = 1;

      if(saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string fileName = saveFileDialog1.FileName;

        fPdfExporter.SaveDocument(fDocument, fileName);

        Thread.Sleep(5000);

        // fDocument.RemoveAll();

        fAppSettings.LastDirectory = Path.GetDirectoryName(fileName);
      }
    }


    private void ButtonPrint_Click(object sender, EventArgs e)
    {
      if(fDocument.NumPages > 0)
      {
        PrintDialog dlg = new PrintDialog();
        dlg.AllowSomePages = true;
        dlg.UseEXDialog = true;
        dlg.PrinterSettings.PrinterName = "PrimoPDF";
        dlg.PrinterSettings.MinimumPage = 1;
        dlg.PrinterSettings.MaximumPage = fDocument.NumPages;
        dlg.PrinterSettings.FromPage = dlg.PrinterSettings.MinimumPage;
        dlg.PrinterSettings.ToPage = dlg.PrinterSettings.MaximumPage;

        if(dlg.ShowDialog() == DialogResult.OK)
        {
          if(MenuSettingsPrinterUsePreview.Checked)
          {
            fPrinter.PreviewDocument(fDocument, dlg.PrinterSettings);
          }
          else 
          {
            fPrinter.PrintDocument(fDocument, dlg.PrinterSettings);
          }
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
      if(DialogResult.Yes == MessageBox.Show("This will sort the images in front/back order", "Confirm", MessageBoxButtons.YesNo))
      {
        fDocument.RearrangePages2Sided();
      }
    }


    private void ButtonDelete_Click(object sender, EventArgs e)
    {
      if(ListViewSelectedItem != null)
      {
        if(DialogResult.OK == MessageBox.Show("Delete selected image?", "Delete?", MessageBoxButtons.OKCancel))
        {
          fDocument.DeletePage(ListViewSelectedItem.Index);
        }
      }
    }


    private void ButtonDeleteAll_Click(object sender, EventArgs e)
    {
      if(DialogResult.OK == MessageBox.Show("Remove all pages?", "Delete?", MessageBoxButtons.OKCancel))
      {
        fDocument.RemoveAll();
      }
    }


    private void MenuSettingsScanner_Click(object sender, EventArgs e)
    {
      ExecuteDataSourceSettingsDialog();
    }
  }
}
