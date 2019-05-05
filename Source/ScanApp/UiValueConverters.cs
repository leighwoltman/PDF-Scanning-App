using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScanApp
{
  public abstract class SimpleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value);
    }

    protected abstract object Convert(object value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }


  public class StringToBoolConverter : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return !string.IsNullOrEmpty((string)value);
    }
  }


  public class BoolToVisibleCollapsed : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }
  }


  public class BoolInverter : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return (bool)value ? false : true;
    }
  }


  public class ItemCountToEnabledConverter : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return ((int)value > 1);
    }
  }


  public class ScanPageTypeToButtonLabel : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return "Scan " + (string)value;
    }
  }


  public class FileNameFromPath : SimpleConverter
  {
    protected override object Convert(object value)
    {
      return System.IO.Path.GetFileName((string)value);
    }
  }
}
