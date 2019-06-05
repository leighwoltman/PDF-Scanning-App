using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Scanning;


namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowScanSettingsDialog.xaml
  /// </summary>
  public partial class WindowScanSettingsDialog : Window, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    public ScanSettings Settings { get; set; }


    public string ProfileName { get; set; }


    private List<string> fReservedNames;


    public ObservableCollection<ColorModeEnum> ColorModes { get; private set; } 
      = new ObservableCollection<ColorModeEnum>();


    public ObservableCollection<int> Resolutions { get; private set; }
      = new ObservableCollection<int>();


    public WindowScanSettingsDialog()
    {
      InitializeComponent();
      DataContext = this;
    }


    public void SetCapabilities(ScanCapabilities capabilities)
    {
      ColorModes.Clear();

      foreach (var item in capabilities.ColorModes)
      {
        ColorModes.Add(item);
      }

      if(capabilities.Resolutions.Count > 0)
      {
        Resolutions.Clear();

        foreach (var item in capabilities.Resolutions)
        {
          Resolutions.Add(item);
        }
      }

      RaisePropertyChanged("Settings");
    }


    public bool ExecuteDialog(List<string> reservedNames)
    {
      fReservedNames = reservedNames;
      fReservedNames.Remove(this.ProfileName);

      if (Settings == null)
      {
        Settings = ScanSettings.Default;
      }

      if (ColorModes.Count == 0)
      {
        ColorModes.Add(Settings.ColorMode);
      }

      if (Resolutions.Count == 0)
      {
        Resolutions.Add(100);
        Resolutions.Add(150);
        Resolutions.Add(200);
        Resolutions.Add(300);
        Resolutions.Add(600);
      }

      // These need to be raised once, for initialization, since the values only change by UI, and not programmatically
      RaisePropertyChanged("ProfileName");
      RaisePropertyChanged("Settings");

      bool? result = this.ShowDialog();
      return result.HasValue ? result.Value : false;
    }


    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      if (fReservedNames.Contains(this.ProfileName))
      {
        MessageBox.Show("Not good", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      else
      {
        DialogResult = true;
      }
    }
  }
}
