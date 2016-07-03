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

namespace PDFScanningApp
{
  /// <summary>
  /// Interaction logic for WindowPromptAppendOverwrite.xaml
  /// </summary>
  public partial class WindowPromptAppendOverwrite : Window
  {
    public enum ResultEnum { Cancel = 0, Append, Overwrite };


    public ResultEnum Result;


    public WindowPromptAppendOverwrite()
    {
      InitializeComponent();
      Result = ResultEnum.Cancel;
    }


    private void ButtonAppend_Click(object sender, RoutedEventArgs e)
    {
      Result = ResultEnum.Append;
      Close();
    }


    private void ButtonOverwite_Click(object sender, RoutedEventArgs e)
    {
      Result = ResultEnum.Overwrite;
      Close();
    }
  }
}
