using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HouseUtils;
using HouseControls;


namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowMain.xaml
  /// </summary>
  public partial class WindowMain : Window
  {
    private AppSettings fAppSettings;
    private AppModel fModel;
    private AppProcessing fProcessing;
    private ScanPortal fScanPortal;


    public WindowMain()
    {
      InitializeComponent();

      // TODO: Remove this when we are certain that it is not needed
      // HouseUtils.Threading.DispatcherUI.DefaultUI = this.Dispatcher;

      fAppSettings = AppSettings.LoadOrNew();

      fModel = new AppModel(fAppSettings);
      fModel.VersionText = AppInfo.GetApplicationName() + "\rVersion: "+ AppInfo.GetApplicationV3();
      fModel.PageItems.CollectionChanged += fModel_PageItems_CollectionChanged;
      fModel.Command_ImageInfo = new CommandHandler(Handle_ImageInfo);
      fModel.Command_CompareImages = new CommandHandler(Handle_CompareImages);
      fModel.Command_MirrorHorizontally = new CommandHandler(Handle_MirrorHorizontally);
      fModel.Command_MirrorVertically = new CommandHandler(Handle_MirrorVertically);
      fModel.Command_RotateCounterClockwise = new CommandHandler(Handle_RotateCounterClockwise);
      fModel.Command_RotateClockwise = new CommandHandler(Handle_RotateClockwise);
      fModel.Command_Landscape = new CommandHandler(Handle_Landscape);
      fModel.Command_Delete = new CommandHandler(Handle_Delete);
      fModel.Command_DeleteAll = new CommandHandler(Handle_DeleteAll);
      fModel.Command_SelectAll = new CommandHandler(Handle_SelectAll);
      fModel.Command_Shuffle2Sided = new CommandHandler(Handle_Shuffle2Sided);
      fModel.Command_LoadImages = new CommandHandler(Handle_LoadImages);
      fModel.Command_OpenPdf = new CommandHandler(Handle_OpenPdf);
      fModel.Command_SaveImages = new CommandHandler(Handle_SaveImages);
      fModel.Command_SaveToPdf = new CommandHandler(Handle_SaveToPdf);
      fModel.Command_Print = new CommandHandler(Handle_Print);
      fModel.Command_Settings = new CommandHandler(Handle_Settings);
      fModel.Command_Scan = new CommandHandler(Handle_Scan);
      fModel.Command_ScanPageType = new CommandHandler(Handle_ScanPageType);
      fModel.Command_ProfileAdd = new CommandHandler(Handle_ProfileAdd);
      fModel.Command_ProfileRemove = new CommandHandler(Handle_ProfileRemove);
      fModel.Command_ProfileEdit = new CommandHandler(Handle_ProfileEdit);
      fModel.RefreshEnables();

      DataContext = fModel;

      fProcessing = new AppProcessing(fAppSettings, fModel);
      fProcessing.BlankThumbnail = (BitmapImage)this.Resources["ImageBlankPage"];

      fScanPortal = new ScanPortal();

      InitializeDragDrop();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      fScanPortal.Open((list) => 
      {
        if (list != null)
        {
          foreach (var item in list)
          {
            fModel.ScannerNames.Add(item);
          }

          if (fModel.ScannerNames.Count > 0)
          {
            fModel.SelectedScanner = fModel.ScannerNames[0];
          }
        }
      });

      RefreshStatusDisplay();
    }


    private void Window_Closed(object sender, EventArgs e)
    {
      fScanPortal.Terminate();
      fProcessing.Terminate();
      fAppSettings.Save();
    }


    private void RibbonWin_Loaded(object sender, RoutedEventArgs e)
    {
      Grid child = VisualTreeHelper.GetChild((DependencyObject)sender, 0) as Grid;
      if (child != null)
      {
        child.RowDefinitions[0].Height = new GridLength(0);
      }
    }


    private void Handle_LoadImages()
    {
      string filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
      string initDirectory = fAppSettings.LastDirectoryForLoading;
      string[] filenames = HouseUtils.Dialogs.SelectMultipleFiles(filter, "Select Images", initDirectory);
      if ((filenames != null) && (filenames.Length > 0))
      {
        fProcessing.ImportFiles(filenames);
        fAppSettings.LastDirectoryForLoading = Path.GetDirectoryName(filenames[0]);
      }
    }


    private void Handle_OpenPdf()
    {
      string filter = "PDF Files (*.PDF)|*.PDF|All files (*.*)|*.*";
      string initDirectory = fAppSettings.LastDirectoryForLoading;
      string filename = HouseUtils.Dialogs.SelectFile(filter, "Select a PDF File", initDirectory);
      if (string.IsNullOrEmpty(filename) == false)
      {
        if (string.IsNullOrEmpty(fModel.CurrentFilePath))
        {
          fModel.CurrentFilePath = filename;
        }
        fProcessing.ImportFile(filename);
        fAppSettings.LastDirectoryForLoading = Path.GetDirectoryName(filename);
      }
    }


    private List<ListViewPageItem> GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum action = WindowPromptSelectedAll.ResultEnum.None)
    {
      List<ListViewPageItem> pageItems = new List<ListViewPageItem>();

      if (action == WindowPromptSelectedAll.ResultEnum.None)
      {
        if (ListViewPages.SelectedItems.Count == 0)
        {
          action = WindowPromptSelectedAll.ResultEnum.All;
        }
        else if (ListViewPages.SelectedItems.Count < fModel.PageItems.Count)
        {
          WindowPromptSelectedAll F = new WindowPromptSelectedAll();
          F.Owner = this;
          F.WindowStartupLocation = WindowStartupLocation.CenterOwner;
          F.ShowDialog();
          action = F.Result;
        }
        else
        {
          action = WindowPromptSelectedAll.ResultEnum.All;
        }
      }

      if (action == WindowPromptSelectedAll.ResultEnum.Selected)
      {
        // Only selected pages
        pageItems.AddRange(fModel.PageItems.Where(item => item.IsSelected));
      }
      else if (action == WindowPromptSelectedAll.ResultEnum.All)
      {
        // All pages
        pageItems.AddRange(fModel.PageItems);
      }
      else
      {
        // Cancel, return zero pages
      }

      return pageItems;
    }


    private void Handle_SaveToPdf()
    {
      List<ListViewPageItem> pageItems = GetPageItemsForProcessing();

      if (pageItems.Count > 0)
      {
        string filter = "PDF files (*.pdf)|*.pdf";
        string initDirectory = fAppSettings.LastDirectoryForSaving;
        string initFilename;

        if (string.IsNullOrEmpty(fModel.CurrentFilePath))
        {
          initFilename = "Saved_" + pageItems.Count + "_Pages";
        }
        else
        {
          initFilename = fModel.CurrentFilePath;
        }

        string fileName = HouseUtils.Dialogs.SelectWhereToSave(filter, "Save PDF As", initDirectory, initFilename, false);

        if (string.IsNullOrEmpty(fileName) == false)
        {
          WindowPromptAppendOverwrite.ResultEnum action = WindowPromptAppendOverwrite.ResultEnum.Overwrite;

          if (File.Exists(fileName))
          {
            WindowPromptAppendOverwrite F = new WindowPromptAppendOverwrite();
            F.Owner = this;
            F.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            F.ShowDialog();
            action = F.Result;
          }

          if (action != WindowPromptAppendOverwrite.ResultEnum.Cancel)
          {
            bool append = false;

            if (action == WindowPromptAppendOverwrite.ResultEnum.Append)
            {
              append = true;
            }

            fProcessing.ExportToPdf(fileName, pageItems, append, fAppSettings.ExportSettings.Copy());
            fAppSettings.LastDirectoryForSaving = Path.GetDirectoryName(fileName);
          }
        }
      }
    }


    private void Handle_SaveImages()
    {
      List<ListViewPageItem> pageItems = GetPageItemsForProcessing();

      if (pageItems.Count > 0)
      {
        string filter = "Default format (*.*)|*.*|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png";
        string initDirectory = fAppSettings.LastDirectoryForSaving;
        string initFilename;

        if (pageItems.Count > 1)
        {
          initFilename = "(Multiple files)";
        }
        else
        {
          ListViewPageItem pageItem = pageItems[0];
          initFilename = pageItem.Name; // Default page name
        }

        string fileName = HouseUtils.Dialogs.SelectWhereToSave(filter, "Save Images", initDirectory, initFilename);

        if (string.IsNullOrEmpty(fileName) == false)
        {
          fProcessing.SaveImages(fileName, pageItems, fAppSettings.ExportSettings.Copy());
          fAppSettings.LastDirectoryForSaving = Path.GetDirectoryName(fileName);
        }

        fModel.Exporting = false;
      }
    }


    private void Handle_Print()
    {
      List<ListViewPageItem> pageItems = GetPageItemsForProcessing();

      if (pageItems.Count > 0)
      {
        Printing printer = new Printing();
        printer.PrintDocument(pageItems, true);
      }
    }


    private void Handle_Settings()
    {
      WindowAppSettings F = new WindowAppSettings();
      F.Owner = this;
      F.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      F.PageTypes = new List<string>(fAppSettings.PageSizes.Keys);

      F.ShowPrintButton = fAppSettings.ShowPrintButton;
      F.PdfSettings = fAppSettings.PdfSettings;
      F.ExportSettings = fAppSettings.ExportSettings;
      F.DefaultPageType = fAppSettings.DefaultPageType;
      F.CustomPageSize = fAppSettings.GetCustomPageSize();

      if (F.ExecuteDialog())
      {
        fAppSettings.ShowPrintButton = F.ShowPrintButton;
        fAppSettings.PdfSettings = F.PdfSettings;
        fAppSettings.ExportSettings = F.ExportSettings;
        fAppSettings.DefaultPageType = F.DefaultPageType;
        fAppSettings.SetCustomPageSize(F.CustomPageSize);
      }

      fModel.RefreshEnables();
    }


    private void Handle_ScanPageType()
    {
      // Do nothing
    }


    private void Handle_Scan()
    {
      string profile = fModel.SelectedScanProfile;

      if (string.IsNullOrEmpty(profile) == false)
      {
        ScanSettings settings = fAppSettings.GetScanSettings(profile);

        Size2D pageSize = fAppSettings.GetPageSize(fModel.ScanPageType);

        // TODO: Define area as (x, y, width, height)
        Bounds scanArea = new Bounds(0, 0, pageSize);

        fModel.Scanning = true;

        fScanPortal.Acquire(fModel.SelectedScanner, scanArea, fModel.DuplexScanEnabled, settings, (image) =>
        {
          if (image != null)
          {
            Documents.Page myPage = Documents.PageFromScanner.Create(image, settings.Resolution);
            fProcessing.AddPage(myPage);
          }
          else
          {
            // End of scan
            fModel.Scanning = false;
          }
        });
      }
    }


    private void Handle_ImageInfo()
    {
      if (ListViewPages.SelectedItems.Count == 1)
      {
        ListViewPageItem pageItem = (ListViewPageItem)ListViewPages.SelectedItems[0];

        fProcessing.GetPageInfo(pageItem, (properties) => 
        {
          WindowImageProperties F = new WindowImageProperties(properties);
          F.Owner = this;
          F.WindowStartupLocation = WindowStartupLocation.CenterOwner;
          F.ShowDialog();
        });
      }
    }


    private void Handle_CompareImages()
    {
      if (ListViewPages.SelectedItems.Count == 2)
      {
        ListViewPageItem pageItem1 = (ListViewPageItem)ListViewPages.SelectedItems[0];
        ListViewPageItem pageItem2 = (ListViewPageItem)ListViewPages.SelectedItems[1];

        fProcessing.ComparePages(pageItem1, pageItem2, (isEqual) => 
        {
          ShowStatus("Compare: " + (isEqual ? "Images are same" : "Images are different"));
        });
      }
    }


    private void Handle_MirrorHorizontally()
    {
      fProcessing.PagesMirrorHorizontally(GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected));
    }


    private void Handle_MirrorVertically()
    {
      fProcessing.PagesMirrorVertically(GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected));
    }


    private void Handle_RotateCounterClockwise()
    {
      fProcessing.PagesRotateCounterClockwise(GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected));
    }


    private void Handle_RotateClockwise()
    {
      fProcessing.PagesRotateClockwise(GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected));
    }


    private void Handle_Landscape()
    {
      fProcessing.PagesRotateSideways(GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected));
    }


    private void Handle_Delete()
    {
      List<ListViewPageItem> pageItems = GetPageItemsForProcessing(WindowPromptSelectedAll.ResultEnum.Selected);

      if (pageItems.Count > 0)
      {
        bool confirmed = false;

        if (pageItems.Count == 1)
        {
          confirmed = Dialogs.Confirm("This will remove the selected page");
        }
        else
        {
          confirmed = Dialogs.Confirm(string.Format("This will remove selected {0} pages", pageItems.Count));
        }

        if (confirmed)
        {
          fProcessing.PagesDelete(pageItems);
        }
      }
    }


    private void Handle_DeleteAll()
    {
      if (Dialogs.Confirm("This will remove all pages"))
      {
        fProcessing.AllPagesDelete();
      }
    }


    private void Handle_SelectAll()
    {
      fProcessing.AllPagesSelect();
    }


    private void Handle_Shuffle2Sided()
    {
      if (Dialogs.Confirm("This will sort the images in front/back order"))
      {
        fProcessing.AllPagesShuffle2Sided();
      }
    }


    private void fModel_PageItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (fModel.PageItems.Count == 0)
      {
        fModel.CurrentFilePath = string.Empty;
      }
      fModel.RefreshEnables();
      RefreshStatusDisplay();
    }


    private void Handle_ProfileAdd()
    {
      ExecuteScanSettingsDialog(string.Empty);
    }


    private void Handle_ProfileEdit()
    {
      string profile = fModel.SelectedScanProfile;

      if (string.IsNullOrEmpty(profile) == false)
      {
        ExecuteScanSettingsDialog(profile);
      }
    }


    private void ExecuteScanSettingsDialog(string profile)
    {
      WindowScanSettingsDialog dialog = new WindowScanSettingsDialog();
      dialog.Owner = this;
      dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

      fScanPortal.GetCapabilities(fModel.SelectedScanner, (capabilities) =>
      {
        // This will most likely happen after dialog is already shown
        dialog.SetCapabilities(capabilities);
      });

      dialog.ProfileName = profile;
      dialog.Settings = fAppSettings.GetScanSettings(profile);

      List<string> reservedNames = fAppSettings.GetScanProfileNames();

      if (dialog.ExecuteDialog(reservedNames))
      {
        fAppSettings.RenameProfile(profile, dialog.ProfileName);
        fAppSettings.UpdateProfile(dialog.ProfileName, dialog.Settings);
        fModel.RefreshProfiles();
        fModel.SelectedScanProfile = dialog.ProfileName;
      }
    }


    private void Handle_ProfileRemove()
    {
      string profile = fModel.SelectedScanProfile;

      if (string.IsNullOrEmpty(profile) == false)
      {
        if (HouseUtils.Dialogs.Confirm("Press OK to continue"))
        {
          fAppSettings.RemoveProfile(profile);
          fModel.RefreshProfiles();
        }
      }
    }


    private void ListViewPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      fProcessing.RefreshDisplay();
      fModel.RefreshEnables();
      RefreshStatusDisplay();
    }


    private void RefreshStatusDisplay()
    {
      string statusText = fModel.PageItems.Count + " Items";

      int numSelected = fModel.GetNumSelectedPages();

      if (numSelected > 1)
      {
        statusText += string.Format(". {0} selected", numSelected);
      }
      else
      {
        var selectedItem = fModel.SelectedPageItem;

        if (selectedItem != null)
        {
          statusText += string.Format(". Selected: {0}", selectedItem.Name);
        }
      }

      ShowStatus(statusText);
    }


    private void ShowStatus(string message)
    {
      fModel.StatusText = message;

      if (fModel.Exporting == false)
      {
        fModel.ExportText = string.Empty;
      }
    }


    #region DRAG_DROP

    private Point? fpMouseDownPos = null;
    private InsertionMark fInsertionMark = null;


    private void InitializeDragDrop()
    {
      fInsertionMark = new InsertionMark(ListViewPages);
    }


    private void ListViewPages_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      fpMouseDownPos = e.GetPosition(ListViewPages);
    }


    private void ListViewPages_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      // Comes here only if left button is up without dragging started
      fpMouseDownPos = null;
    }


    private void ListViewPages_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if ((e.LeftButton == MouseButtonState.Pressed) && (fpMouseDownPos.HasValue))
      {
        Point pt = e.GetPosition(ListViewPages);

        if ((fpMouseDownPos.Value - pt).Length > 10)
        {
          ListViewItem dragStartItem = WpfListViewAssist.GetListViewItemAtPoint(ListViewPages, fpMouseDownPos.Value);

          if (dragStartItem != null)
          {
            DragDropEffects effects = DragDropEffects.Move;

            if (ListViewPages.SelectedItems.Count > 1)
            {
              effects = DragDropEffects.Copy;
            }

            DragDrop.DoDragDrop(ListViewPages, dragStartItem, effects);
          }

          fInsertionMark.Hide();
          fpMouseDownPos = null; // Make sure this code is executed once per Mouse Down
          e.Handled = true;
        }
      }
    }


    private void ListViewPages_DragOver(object sender, System.Windows.DragEventArgs e)
    {
      int sourceIndex = -1;

      if (e.Data.GetDataPresent(typeof(ListViewItem)))
      {
        ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

        if (draggedItem != null)
        {
          sourceIndex = ListViewPages.Items.IndexOf(draggedItem.Content);
        }
      }
      else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
      {
        // Importing files
        if ((fModel.Command_OpenPdf.IsEnabled == false) || (fModel.Command_LoadImages.IsEnabled == false))
        {
          e.Effects = DragDropEffects.None;
        }
      }
      else
      {
        e.Effects = DragDropEffects.None;
      }

      if (e.Effects == DragDropEffects.None)
      {
        fInsertionMark.Hide();
      }
      else
      {
        int targetIndex = -1;

        Point pt = e.GetPosition(ListViewPages);
        ListViewItem item = WpfListViewAssist.GetListViewItemAtPoint(ListViewPages, pt);

        if (item != null)
        {
          targetIndex = ListViewPages.Items.IndexOf(item.Content);
        }

        if (targetIndex < 0)
        {
          fInsertionMark.Hide();
        }
        else
        {
          int scrollIndex = targetIndex;

          if (pt.Y < item.ActualHeight)
          {
            if (scrollIndex > 0)
            {
              scrollIndex--;
            }
          }
          else if (pt.Y > (ListViewPages.ActualHeight - item.ActualHeight))
          {
            if (scrollIndex < (ListViewPages.Items.Count - 1))
            {
              scrollIndex++;
            }
          }
          else
          {
            // No Scrolling
          }

          if (scrollIndex != targetIndex)
          {
            fInsertionMark.Hide();
            ListViewPages.ScrollIntoView(ListViewPages.Items[scrollIndex]);
            System.Threading.Thread.Sleep(70);   //slow down the scrolling a bit
          }
          else if (sourceIndex < 0)
          {
            fInsertionMark.Show(item, true);
          }
          else if (targetIndex == sourceIndex)
          {
            fInsertionMark.Hide();
          }
          else
          {
            fInsertionMark.Show(item, targetIndex < sourceIndex);
          }
        }
      }

      e.Handled = true;
    }


    private void ListViewPages_Drop(object sender, System.Windows.DragEventArgs e)
    {
      // Check for dragged ListViewPage ----
      int targetIndex = fInsertionMark.Index;

      if (targetIndex >= 0)
      {
        if (e.Data.GetDataPresent(typeof(ListViewItem)))
        {
          ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

          if (draggedItem != null)
          {
            int sourceIndex = ListViewPages.Items.IndexOf(draggedItem.Content);
            fModel.PageItems.Move(sourceIndex, targetIndex);
          }
        }
      }

      // Check for dragged files from another application -----
      if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
      {
        string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
        if (filenames != null)
        {
          fProcessing.ImportFiles(filenames); // TODO: At target index
        }
      }

      e.Handled = true;
    }


    private void ListViewPages_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      if (e.EscapePressed)
      {
        e.Action = DragAction.Cancel;
        // In case the drap operation was initiated from another application and we did not call DoDrapDrop...
        fInsertionMark.Hide();
      }
    }

    #endregion DRAG_DROP


    private void Image_LayoutChanged(object sender, EventArgs e)
    {
      ControlImagePanZoom ctrl = sender as ControlImagePanZoom;
      fModel.LayoutText = string.Format("Scale:{0:0.00} Pan:{1:0.},{2:0.}", ctrl.Scale, ctrl.PanX, ctrl.PanY);
    }
  }
}
