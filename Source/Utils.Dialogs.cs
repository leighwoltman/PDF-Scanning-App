using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace Utils
{
  class Dialogs
  {
    static public void AdoptForm(Form form, Control parent)
    {
      form.TopLevel = false;
      form.Parent = parent;
      form.ControlBox = false;
      form.Text = "";
      form.FormBorderStyle = FormBorderStyle.None;
      form.Dock = DockStyle.Fill;
      form.Visible = true;
    }


    static public string ShowSaveFileDialog()
    {
      return ShowSaveFileDialog(null, null);
    }


    static public void ShowError(string message)
    {
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    
    static public string ShowSaveFileDialog(string defaultFilename, string filter)
    {
      string result = null;
      SaveFileDialog F = new SaveFileDialog();

      F.FileName = defaultFilename;
      F.Filter = filter;

      if(F.ShowDialog() == DialogResult.OK)
      {
        result = F.FileName;
      }

      return result;
    }


    static public string ShowOpenFileDialog()
    {
      return ShowOpenFileDialog(null);
    }


    static public string ShowOpenFileDialog(string filter)
    {
      string result = null;
      OpenFileDialog F = new OpenFileDialog();

      if(F.ShowDialog() == DialogResult.OK)
      {
        result = F.FileName;
      }

      return result;
    }
  }
}
