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
  /// Interaction logic for WindowImageProperties.xaml
  /// </summary>
  public partial class WindowImageProperties : Window
  {
    Dictionary<string, string> fItems;

    public WindowImageProperties(Dictionary<string, string> items)
    {
      InitializeComponent();
      fItems = items;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      for(int i = 0; i < fItems.Keys.Count; i++)
      {
        InfoGrid.RowDefinitions.Add(new RowDefinition());
      }

      int row = 0;

      foreach(string key in fItems.Keys)
      {
        TextBlock nameText = new TextBlock();
        nameText.Text = key;
        nameText.FontSize = 12;
        nameText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        nameText.Margin = new Thickness(0, 2, 10, 2);
        Grid.SetRow(nameText, row);
        Grid.SetColumn(nameText, 0);

        TextBlock valueText = new TextBlock();
        valueText.Text = fItems[key];
        valueText.FontSize = 12;
        valueText.Margin = new Thickness(10, 2, 0, 2);
        Grid.SetRow(valueText, row);
        Grid.SetColumn(valueText, 1);

        InfoGrid.Children.Add(nameText);
        InfoGrid.Children.Add(valueText);

        row++;
      }
    }
  }
}
