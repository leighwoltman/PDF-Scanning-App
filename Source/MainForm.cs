using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace PDFScanningApp
{
  public partial class MainForm : Form
  {
    private Settings settings;
    private const int none_selected = -1;
    private int selected_index = none_selected;
    private Document myDocument;


    public MainForm(Settings settings)
    {
      InitializeComponent();
      this.settings = settings;

      myDocument = new Document();
      myDocument.OnPageAdded += myDocument_OnPageAdded;
      myDocument.OnPageRemoved += myDocument_OnPageRemoved;
      myDocument.OnPageUpdated += myDocument_OnPageUpdated;
      myDocument.OnPageMoved += myDocument_OnPageMoved;

      comboBoxResolution.Items.Add("200 dpi");
      comboBoxResolution.Items.Add("300 dpi");
      comboBoxResolution.SelectedIndex = 1;
    }


    private void MainForm_Load(object sender, EventArgs e)
    {
      RefreshControls();
    }


    void RefreshControls()
    {
      int numPages = myDocument.NumPages;

      // we have to have at least 3 pages to make this worth while
      // only allow this button when no page is selected
      if((numPages > 2) && (selected_index == none_selected))
      {
        sided2Button.Enabled = true;
      }
      else
      {
        sided2Button.Enabled = false;
      }

      if(numPages > 0)
      {
        saveButton.Enabled = true;
      }
      else
      {
        saveButton.Enabled = false;
      }

      if(selected_index == none_selected)
      {
        moveLeftButton.Enabled = false;
        moveRightButton.Enabled = false;
        deleteButton.Enabled = false;
        rotateButton.Enabled = false;
        buttonLandscape.Enabled = false;
      }
      else
      {
        if(selected_index == 0)
        {
          moveLeftButton.Enabled = false;
        }
        else
        {
          moveLeftButton.Enabled = true;
        }

        if(selected_index == (numPages - 1))
        {
          moveRightButton.Enabled = false;
        }
        else 
        {
          moveRightButton.Enabled = true;
        }

        deleteButton.Enabled = true;
        rotateButton.Enabled = true;
        buttonLandscape.Enabled = true;
      }
    }


    void myDocument_OnPageAdded(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      Page page = myDocument.GetPage(args.Index);
      PictureBox myPictureBox = new PictureBox();
      myPictureBox.Image = page.Thumbnail;
      myPictureBox.Height = 180;
      myPictureBox.Width = 180;
      myPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
      myPictureBox.Click += picBox_Click;
      imagePanel.Controls.Add(myPictureBox);
      RefreshControls();
    }


    void myDocument_OnPageRemoved(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;

      if(args.Index >= 0)
      {
        PictureBox myPictureBox = (PictureBox)imagePanel.Controls[args.Index];
        myPictureBox.Image = null; // No exception, but needed to make this similar to below -Caner
        imagePanel.Controls.RemoveAt(args.Index);
      }
      else
      {
        // Negative index, all pages are removed
        // first copy the controls to another croup;
        int count = imagePanel.Controls.Count;

        for(int i = 0; i < count; i++)
        {
          PictureBox myPictureBox = (PictureBox)imagePanel.Controls[0];
          myPictureBox.Image = null; // Exception if I don't do this first -Caner
          imagePanel.Controls.RemoveAt(0);
        }
      }

      selected_index = none_selected;
      RefreshControls();
    }


    void myDocument_OnPageUpdated(object sender, EventArgs e)
    {
      DocumentPageEventArgs args = (DocumentPageEventArgs)e;
      Control pbox = imagePanel.Controls[args.Index];
      pbox.Refresh();
      RefreshControls();
    }


    void myDocument_OnPageMoved(object sender, EventArgs e)
    {
      DocumentPageMoveEventArgs args = (DocumentPageMoveEventArgs)e;
      imagePanel.Controls.SetChildIndex(imagePanel.Controls[args.SourceIndex], args.TargetIndex);
      selected_index = args.TargetIndex;
      RefreshControls();
    }


    int getScanResolution()
    {
      if(comboBoxResolution.SelectedIndex == 1)
      {
        return 300;
      }
      else
      {
        return 200;
      }
    }


    void picBox_Click(object sender, EventArgs e)
    {
      PictureBox pBox = (PictureBox)(sender);

      if(selected_index != imagePanel.Controls.IndexOf(pBox))
      {
        // remove the BorderStyle of the existing index
        if(selected_index != none_selected)
        {
          ((PictureBox)(imagePanel.Controls[selected_index])).BorderStyle = BorderStyle.None;
          ((PictureBox)(imagePanel.Controls[selected_index])).BackColor = imagePanel.BackColor;
        }

        // select this one
        pBox.BorderStyle = BorderStyle.FixedSingle;
        pBox.BackColor = Color.DarkGray;
        selected_index = imagePanel.Controls.IndexOf(pBox);
      }
      else
      {
        // remove the index
        pBox.BorderStyle = BorderStyle.None;
        pBox.BackColor = imagePanel.BackColor;
        selected_index = none_selected;
      }

      RefreshControls();
    }


    private void button3_Click(object sender, EventArgs e)
    {
      Scan(8.48, 11, getScanResolution());

      button3.Focus();
    }


    private void button1_Click(object sender, EventArgs e)
    {
      Scan(8.48, 14, getScanResolution());

      button1.Focus();
    }


    private void moveLeftButton_Click(object sender, EventArgs e)
    {
      myDocument.MovePage(selected_index, selected_index - 1);
    }


    private void moveRightButton_Click(object sender, EventArgs e)
    {
      myDocument.MovePage(selected_index, selected_index + 1);
    }


    private void deleteButton_Click(object sender, EventArgs e)
    {
      if(DialogResult.OK == MessageBox.Show("Delete selected image?", "Delete?", MessageBoxButtons.OKCancel))
      {
        myDocument.DeletePage(selected_index);
      }
    }


    private void rotateButton_Click(object sender, EventArgs e)
    {
      myDocument.RotatePage(selected_index);
    }


    private void buttonLandscape_Click(object sender, EventArgs e)
    {
      myDocument.LandscapePage(selected_index);
    }


    private void sided2Button_Click(object sender, EventArgs e)
    {
      int loc_of_next = 1;
      while(loc_of_next < imagePanel.Controls.Count - 1)
      {
        myDocument.MovePage(imagePanel.Controls.Count - 1, loc_of_next);
        loc_of_next += 2;
      }
    }


    private void Scan(double width, double height, double resolution)
    {
      DataSource ds = new DataSource();
      ds.OnNewPage += ds_OnNewPage;
      ds.OnScanningComplete += ds_OnScanningComplete;

      if(ds.Scan(settings.CurrentScanner, width, height, resolution) == false)
      {
        // an error occured in scanning the file
        MessageBox.Show("Last image now scanned correctly, rescan it. If this error keeps occuring close the program and restart it.");
      }
    }


    void ds_OnNewPage(object sender, EventArgs e)
    {
      DataSourceNewPageEventArgs args = (DataSourceNewPageEventArgs)e;
      myDocument.AddPage(args.ThePage);
    }


    void ds_OnScanningComplete(object sender, EventArgs e)
    {
      // TBD
    }


    private void saveButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();

      if(!Directory.Exists(settings.LastDirectory))
      {
        settings.LastDirectory = "M:\\";
      }

      saveFileDialog1.InitialDirectory = settings.LastDirectory;
      saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
      saveFileDialog1.FilterIndex = 1;

      if(saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string fileName = saveFileDialog1.FileName;

        myDocument.Save(fileName);

        Thread.Sleep(5000);

        myDocument.RemoveAll();

        settings.LastDirectory = Path.GetDirectoryName(fileName);
        settings.SaveSettings();
      }
    }


    private void imageLoad_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      // Set the file dialog to filter for graphics files. 
      openFileDialog1.Filter =
          "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF,*.PNG|" +
          "All files (*.*)|*.*";

      // Allow the user to select multiple images. 
      openFileDialog1.Multiselect = true;
      openFileDialog1.Title = "My Image Browser";

      if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        // load the files
        for(int i = 0; i < openFileDialog1.FileNames.Length; i++)
        {
          Page myPage = new Page(openFileDialog1.FileNames[i]);
          myDocument.AddPage(myPage);
        }
      }

      button3.Focus();
      RefreshControls();
    }
  }
}

