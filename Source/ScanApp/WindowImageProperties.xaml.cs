using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowImageProperties.xaml
  /// </summary>
  public partial class WindowImageProperties : Window
  {
    public ObservableCollection<KeyValuePair<string, string>> PropertyList { get; private set; } = new ObservableCollection<KeyValuePair<string, string>>();


    public WindowImageProperties(List<KeyValuePair<string, string>> items)
    {
      InitializeComponent();

      foreach (var item in items)
      {
        PropertyList.Add(item);
      }

      DataContext = this;
    }
  }
}
