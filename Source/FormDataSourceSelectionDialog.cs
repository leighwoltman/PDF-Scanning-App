using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PDFScanningApp
{
  public partial class FormDataSourceSelectionDialog : Form
  {
    public FormDataSourceSelectionDialog(List<string> dataSourceList)
    {
      InitializeComponent();

      foreach(string item in dataSourceList)
      {
        ComboBoxDataSources.Items.Add(item);
        ComboBoxDataSources.SelectedIndex = 0;
      }
    }


    public bool UseNativeUI
    {
      get 
      {
        return CheckBoxUseNativeUI.Checked;
      }
      set 
      { 
        CheckBoxUseNativeUI.Checked = value;
      }
    }


    public string SelectedDataSource
    {
      get
      {
        return (string)ComboBoxDataSources.SelectedItem;
      }
      set
      {
        foreach(string item in ComboBoxDataSources.Items)
        {
          if(item == value)
          {
            ComboBoxDataSources.SelectedItem = item;
          }
        }
      }
    }
  }
}
