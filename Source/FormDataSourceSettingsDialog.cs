using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Model;
using Defines;


namespace PDFScanningApp
{
  public partial class FormDataSourceSettingsDialog : Form
  {
    public FormDataSourceSettingsDialog(string title)
    {
      InitializeComponent();
      this.Text = title;
    }


    private void RefreshControls()
    {
      if(CheckBoxUseNativeUI.Checked)
      {
        GroupBoxSettings.Enabled = false;
      }
      else
      {
        GroupBoxSettings.Enabled = true;
      }
    }


    private void SelectItemComboBox(ComboBox comboBox, object value)
    {
      foreach(object item in comboBox.Items)
      {
        if(item.ToString() == value.ToString())
        {
          comboBox.SelectedItem = item;
        }
      }
    }


    public void SetAvailableValuesForColorMode(List<ColorModeEnum> values)
    {
      foreach(ColorModeEnum item in values)
      {
        ComboBoxColorMode.Items.Add(item);
      }
    }


    public void SetAvailableValuesForPageType(List<PageTypeEnum> values)
    {
      foreach(PageTypeEnum item in values)
      {
        ComboBoxPageType.Items.Add(item);
      }
    }


    public void SetAvailableValuesForResolution(List<int> values)
    {
      foreach(int item in values)
      {
        ComboBoxResolution.Items.Add(item);
      }
    }


    public bool UseNativeUI
    {
      get { return CheckBoxUseNativeUI.Checked; }
      set { CheckBoxUseNativeUI.Checked = value; }
    }


    public bool EnableFeeder
    {
      get { return CheckBoxEnableFeeder.Checked; }
      set { CheckBoxEnableFeeder.Checked = value; }
    }


    public ColorModeEnum ColorMode
    {
      get { return (ColorModeEnum)ComboBoxColorMode.SelectedItem; }
      set { SelectItemComboBox(ComboBoxColorMode, value); }
    }


    public PageTypeEnum PageType
    {
      get { return (PageTypeEnum)ComboBoxPageType.SelectedItem; }
      set { SelectItemComboBox(ComboBoxPageType, value); }
    }


    public int Resolution
    {
      get { return (int)ComboBoxResolution.SelectedItem; }
      set { SelectItemComboBox(ComboBoxResolution, value); }
    }


    public double Threshold
    {
      get { return ScaledValueFromNumericUpDown(NumericUpDownThreshold); }
      set { ScaledValueToNumericUpDown(value, NumericUpDownThreshold); }
    }


    public double Brightness
    {
      get { return ScaledValueFromNumericUpDown(NumericUpDownBrightness); }
      set { ScaledValueToNumericUpDown(value, NumericUpDownBrightness); }
    }


    public double Contrast
    {
      get { return ScaledValueFromNumericUpDown(NumericUpDownContrast); }
      set { ScaledValueToNumericUpDown(value, NumericUpDownContrast); }
    }


    private void ScaledValueToNumericUpDown(double value, NumericUpDown nud)
    {
      nud.Value = nud.Minimum + Convert.ToDecimal(value) * (nud.Maximum - nud.Minimum);
    }


    private double ScaledValueFromNumericUpDown(NumericUpDown nud)
    {
      return Convert.ToDouble((nud.Value - nud.Minimum) / (nud.Maximum - nud.Minimum));
    }


    private void CheckBoxUseNativeUI_CheckStateChanged(object sender, EventArgs e)
    {
      RefreshControls();
    }
  }
}
