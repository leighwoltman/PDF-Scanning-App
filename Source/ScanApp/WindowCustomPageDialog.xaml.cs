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


namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowCustomPageDialog.xaml
  /// </summary>
  public partial class WindowCustomPageDialog : Window, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    public Size2D PageSize { get; set; }

  
    public WindowCustomPageDialog()
    {
      InitializeComponent();
      DataContext = this;
    }


    public bool ExecuteDialog()
    {
      // TODO: This should not be necessary, maybe something to be fixed in the up/down component?
      RaisePropertyChanged("PageSize");

      bool? result = this.ShowDialog();
      return result.HasValue ? result.Value : false;
    }


    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
