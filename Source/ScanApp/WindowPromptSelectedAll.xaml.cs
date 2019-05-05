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


namespace ScanApp
{
  /// <summary>
  /// Interaction logic for WindowPromptSelectedAll.xaml
  /// </summary>
  public partial class WindowPromptSelectedAll : Window
  {
    public enum ResultEnum { None = 0, Selected, All };


    public ResultEnum Result;


    public WindowPromptSelectedAll()
    {
      InitializeComponent();
      Result = ResultEnum.None;
    }

    private void ButtonAll_Click(object sender, RoutedEventArgs e)
    {
      Result = ResultEnum.All;
      Close();
    }

    private void ButtonSelected_Click(object sender, RoutedEventArgs e)
    {
      Result = ResultEnum.Selected;
      Close();
    }
  }
}
