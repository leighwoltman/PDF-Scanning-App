using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Utils
{
  class WpfAssist
  {
    static public ListViewItem GetListViewItemAtPoint(ListView listView, Point pt)
    {
      HitTestResult hitTest = VisualTreeHelper.HitTest(listView, pt);

      DependencyObject depObj = hitTest.VisualHit as DependencyObject;

      if(depObj != null)
      {
        // go up the visual hierarchy until we find the list view item the click came from  
        // the click might have been on the grid or column headers so we need to cater for this  
        DependencyObject current = depObj;
        while((current != null) && (current != listView))
        {
          ListViewItem ListViewItem = current as ListViewItem;
          if(ListViewItem != null)
          {
            return ListViewItem;
          }
          current = VisualTreeHelper.GetParent(current);
        }
      }

      return null;
    }


    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
           where T : DependencyObject
    {
      if(depObj != null)
      {
        for(int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
          DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

          if(child != null && child is T)
          {
            yield return (T)child;
          }

          foreach(T childOfChild in FindVisualChildren<T>(child))
          {
            yield return childOfChild;
          }
        }
      }
    }


    public static childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
    {
      foreach(childItem child in FindVisualChildren<childItem>(obj))
      {
        return child;
      }

      return null;
    }
  }
}
