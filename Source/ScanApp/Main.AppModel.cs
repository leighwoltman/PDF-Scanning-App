using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HouseUtils;
using Documents;


namespace ScanApp
{
  class AppModel : BindingPropertyClass
  {
    #region BindingProperties

    public ObservableCollection<ListViewPageItem> PageItems { get; private set; } = new ObservableCollection<ListViewPageItem>();

    public int GetNumSelectedPages()
    {
      return PageItems.Count(p => p.IsSelected);
    }

    private ListViewPageItem fSelectedPageItem;
    public ListViewPageItem SelectedPageItem
    {
      get { return fSelectedPageItem; }
      set { if (fSelectedPageItem != value) { fSelectedPageItem = value; RaisePropertyChanged("SelectedPageItem"); } }
    }

    public ObservableCollection<string> ScannerNames { get; private set; } = new ObservableCollection<string>();

    private string _selectedScanner = null;
    public string SelectedScanner
    {
      get { return _selectedScanner; }
      set { if (_selectedScanner != value) { _selectedScanner = value; RaisePropertyChanged("SelectedScanner"); } }
    }

    public ObservableCollection<string> ScanProfiles { get; private set; } = null;

    private string _selectedScanProfile = string.Empty;
    public string SelectedScanProfile
    {
      get { return _selectedScanProfile; }
      set { if (_selectedScanProfile != value) { _selectedScanProfile = value; RaisePropertyChanged("SelectedScanProfile"); } }
    }

    private string fVersionText = string.Empty;
    public string VersionText
    {
      get { return fVersionText; }
      set { if (fVersionText != value) { fVersionText = value; RaisePropertyChanged("VersionText"); } }
    }

    private string fCurrentFilePath = string.Empty;
    public string CurrentFilePath
    {
      get { return fCurrentFilePath; }
      set { if (fCurrentFilePath != value) { fCurrentFilePath = value; RaisePropertyChanged("CurrentFilePath"); } }
    }

    private string fStatusText = string.Empty;
    public string StatusText
    {
      get { return fStatusText; }
      set { if (fStatusText != value) { fStatusText = value; RaisePropertyChanged("StatusText"); } }
    }

    private string fExportText = string.Empty;
    public string ExportText
    {
      get { return fExportText; }
      set { if (fExportText != value) { fExportText = value; RaisePropertyChanged("ExportText"); } }
    }

    private string fLayoutText = string.Empty;
    public string LayoutText
    {
      get { return fLayoutText; }
      set { if (fLayoutText != value) { fLayoutText = value; RaisePropertyChanged("LayoutText"); } }
    }

    private bool fScanning = false;
    public bool Scanning
    {
      get { return fScanning; }
      set { if (fScanning != value) { fScanning = value; RaisePropertyChanged("Scanning"); RefreshEnables(); } }
    }

    private bool fExporting = false;
    public bool Exporting
    {
      get { return fExporting; }
      set { if (fExporting != value) { fExporting = value; RaisePropertyChanged("Exporting"); RefreshEnables(); } }
    }

    public ObservableCollection<string> PageTypes { get; private set; } = null;

    private string fScanPageType = string.Empty;
    public string ScanPageType
    {
      get { return fScanPageType; }
      set { if (fScanPageType != value) { fScanPageType = value; RaisePropertyChanged("ScanPageType"); } }
    }

    private bool fPrintEnabled = false;
    public bool PrintEnabled
    {
      get { return fPrintEnabled; }
      set { if (fPrintEnabled != value) { fPrintEnabled = value; RaisePropertyChanged("PrintEnabled"); } }
    }

    private bool fDuplexScanEnabled = true;
    public bool DuplexScanEnabled
    {
      get { return fDuplexScanEnabled; }
      set { if (fDuplexScanEnabled != value) { fDuplexScanEnabled = value; RaisePropertyChanged("DuplexScanEnabled"); } }
    }

    public CommandHandler Command_ImageInfo { get; set; }
    public CommandHandler Command_CompareImages { get; set; }
    public CommandHandler Command_MirrorHorizontally { get; set; }
    public CommandHandler Command_MirrorVertically { get; set; }
    public CommandHandler Command_RotateCounterClockwise { get; set; }
    public CommandHandler Command_RotateClockwise { get; set; }
    public CommandHandler Command_Landscape { get; set; }
    public CommandHandler Command_Delete { get; set; }
    public CommandHandler Command_DeleteAll { get; set; }
    public CommandHandler Command_SelectAll { get; set; }
    public CommandHandler Command_Shuffle2Sided { get; set; }
    public CommandHandler Command_LoadImages { get; set; }
    public CommandHandler Command_OpenPdf { get; set; }
    public CommandHandler Command_SaveImages { get; set; }
    public CommandHandler Command_SaveToPdf { get; set; }
    public CommandHandler Command_SaveAllToPdf { get; set; }
    public CommandHandler Command_AppendAllToPdf { get; set; }
    public CommandHandler Command_Print { get; set; }
    public CommandHandler Command_Settings { get; set; }
    public CommandHandler Command_Scan { get; set; }
    public CommandHandler Command_ScanPageType { get; set; }
    public CommandHandler Command_ProfileAdd { get; set; }
    public CommandHandler Command_ProfileRemove { get; set; }
    public CommandHandler Command_ProfileEdit { get; set; }

    #endregion


    private AppSettings fAppSettings;


    public AppModel(AppSettings settings)
    {
      fAppSettings = settings;
      RefreshProfiles();

      PageTypes = new ObservableCollection<string>(fAppSettings.PageSizes.Keys);
      ScanPageType = fAppSettings.DefaultPageType;
    }


    public void RefreshProfiles()
    {
      ScanProfiles = new ObservableCollection<string>(fAppSettings.ScanProfiles.Keys);
      RaisePropertyChanged("ScanProfiles"); // TODO: Sync the ObservableCollection or recreate?

      if (ScanProfiles.Count > 0)
      {
        if(ScanProfiles.Contains(SelectedScanProfile) == false)
        {
          SelectedScanProfile = ScanProfiles[0];
        }
      }
      else
      {
        SelectedScanProfile = string.Empty;
      }
    }


    public void RefreshEnables()
    {
      bool hasPages = PageItems.Count > 0;
      bool hasSelected = GetNumSelectedPages() > 0;

      Command_ImageInfo.IsEnabled = hasSelected;
      Command_CompareImages.IsEnabled = hasSelected;
      Command_MirrorHorizontally.IsEnabled = hasSelected && (Exporting == false);
      Command_MirrorVertically.IsEnabled = hasSelected && (Exporting == false);
      Command_RotateCounterClockwise.IsEnabled = hasSelected && (Exporting == false);
      Command_RotateClockwise.IsEnabled = hasSelected && (Exporting == false);
      Command_Landscape.IsEnabled = hasSelected && (Exporting == false);
      Command_Delete.IsEnabled = hasSelected && (Exporting == false);
      Command_DeleteAll.IsEnabled = hasPages && (Exporting == false);
      Command_SelectAll.IsEnabled = hasPages;
      Command_Shuffle2Sided.IsEnabled = hasPages && (Exporting == false);
      Command_LoadImages.IsEnabled = (Exporting == false);
      Command_OpenPdf.IsEnabled = (Exporting == false);
      Command_SaveImages.IsEnabled = hasPages && (Exporting == false);
      Command_SaveToPdf.IsEnabled = hasPages && (Exporting == false);
      Command_Print.IsEnabled = hasPages && (Scanning == false) && (Exporting == false);
      Command_Settings.IsEnabled = true;
      Command_Scan.IsEnabled = (Scanning == false);
      Command_ScanPageType.IsEnabled = true;
      Command_ProfileAdd.IsEnabled = true;
      Command_ProfileRemove.IsEnabled = true;
      Command_ProfileEdit.IsEnabled = true;

      // UI Update
      PrintEnabled = fAppSettings.ShowPrintButton;
    }
  }


  public class ListViewPageItem : BindingPropertyClass
  {
    private int fIndex;
    private Page fPage;
    private bool fIsSelected;


    public ListViewPageItem(int index, Page page)
    {
      fIndex = index;
      fPage = page;
      fIsSelected = false;
    }

    public Page Page
    {
      get { return fPage; }
    }

    public string Name
    {
      get { return fPage.Name; }
    }

    public string Info
    {
      get { return fPage.Name; } // TODO:
    }

    public bool IsSelected
    {
      get { return fIsSelected; }
      set { if (fIsSelected != value) { fIsSelected = value; RaisePropertyChanged("IsSelected"); } }
    }

    public int Index
    {
      get { return fIndex; }
      set { if (fIndex != value) { fIndex = value; RaisePropertyChanged("PageNumber"); } }
    }

    public int PageNumber
    {
      get { return fIndex + 1; }
    }

    private System.Windows.Media.ImageSource fThumbnail = null;
    public System.Windows.Media.ImageSource Thumbnail
    {
      get { return fThumbnail; }
      set { if (fThumbnail != value) { fThumbnail = value; RaisePropertyChanged("Thumbnail"); } }
    }

    private System.Windows.Media.ImageSource fImage = null;
    public System.Windows.Media.ImageSource Image
    {
      get { return fImage; }
      set { if (fImage != value) { fImage = value; RaisePropertyChanged("Image"); } }
    }

    public bool ThumbnailIsDirty { get; set; } = false;
    public bool ImageIsDirty { get; set; } = false;
  }
}
