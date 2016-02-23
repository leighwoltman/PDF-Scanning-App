using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using Source;
using Model;
using Utils;


namespace PDFScanningApp
{
  public partial class MainForm : Form
  {
    private AppSettings fAppSettings;
    private Scanner fScanner;
    private PdfExporter fPdfExporter;
    private Document myDocument;
    private const int none_selected = -1;
    private int selected_index = none_selected;


    public MainForm()
    {
      InitializeComponent();
      UtilDialogs.MainWindow = this;

      fAppSettings = new AppSettings();

      fScanner = new Scanner();
      fScanner.OnScanningComplete += fScanner_OnScanningComplete;

      fPdfExporter = new PdfExporter();

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
      if(fScanner.Open())
      {
        RefreshScannerActiveDataSource();
      }
      else
      {
        UtilDialogs.ShowError("Device manager failed to initialize");
      }

      RefreshControls();
    }


    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      fScanner.Close();
    }


    void fScanner_OnScanningComplete(object sender, EventArgs e)
    {
      // Nothing to do
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


    private bool RefreshScannerActiveDataSource()
    {
      bool success = true;

      if(String.IsNullOrEmpty(fScanner.GetActiveDataSourceName()))
      {
        if(String.IsNullOrEmpty(fAppSettings.CurrentScanner))
        {
          ExecuteDataSourceSelectionDialog();
        }

        if(fScanner.SelectActiveDataSource(fAppSettings.CurrentScanner) == false)
        {
          fAppSettings.CurrentScanner = null;
          UtilDialogs.ShowError("No valid data source selected");
          success = false;
        }
      }

      return success;
    }


    private bool ExecuteDataSourceSelectionDialog()
    {
      bool result = false;

      FormDataSourceSelectionDialog F = new FormDataSourceSelectionDialog(fScanner.GetDataSourceNames());

      F.SelectedDataSource = fAppSettings.CurrentScanner;
      F.UseNativeUI = fAppSettings.UseScannerNativeUI;

      if(F.ShowDialog() == DialogResult.OK)
      {
        fAppSettings.CurrentScanner = F.SelectedDataSource;
        fAppSettings.UseScannerNativeUI = F.UseNativeUI;
        result = true;
      }

      return result;
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
      Scan(PageTypeEnum.Letter, getScanResolution());

      button3.Focus();
    }


    private void button1_Click(object sender, EventArgs e)
    {
      Scan(PageTypeEnum.Legal, getScanResolution());

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


    private void Scan(PageTypeEnum pageType, double resolution)
    {
      if(RefreshScannerActiveDataSource())
      {
        ScanSettings settings = new ScanSettings();

        settings.EnableFeeder = true;
        settings.ColorMode = ColorModeEnum.RGB;
        settings.PageType = pageType;
        settings.Resolution = (int)resolution;
        settings.Threshold = 0.75;
        settings.Brightness = 0.5;
        settings.Contrast = 0.5;

        if(fScanner.Acquire(myDocument, settings, fAppSettings.UseScannerNativeUI, true) == false)
        {
          UtilDialogs.ShowError("Scanner failed to start");
        }

        RefreshControls();
      }
    }

    
    private void saveButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();

      if(!Directory.Exists(fAppSettings.LastDirectory))
      {
        fAppSettings.LastDirectory = "M:\\";
      }

      saveFileDialog1.InitialDirectory = fAppSettings.LastDirectory;
      saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
      saveFileDialog1.FilterIndex = 1;

      if(saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string fileName = saveFileDialog1.FileName;

        fPdfExporter.SaveDocument(myDocument, fileName);

        Thread.Sleep(5000);

        myDocument.RemoveAll();

        fAppSettings.LastDirectory = Path.GetDirectoryName(fileName);
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

    private void pdfLoad_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      // Set the file dialog to filter for graphics files. 
      openFileDialog1.Filter =
          "PDF (*.PDF)|*.PDF";

      openFileDialog1.Multiselect = false;
      openFileDialog1.Title = "Select a PDF File";

      if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        try
        {
          PdfDocument document = PdfReader.Open(openFileDialog1.FileName);

          // Iterate pages
          foreach (PdfPage page in document.Pages)
          {
            // Get resources dictionary
            PdfDictionary resources = page.Elements.GetDictionary("/Resources");
            if (resources != null)
            {
              // Get external objects dictionary
              PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
              if (xObjects != null)
              {
                ICollection<PdfItem> items = xObjects.Elements.Values;
                // Iterate references to external objects
                foreach (PdfItem item in items)
                {
                  PdfReference reference = item as PdfReference;
                  if (reference != null)
                  {
                    PdfDictionary xObject = reference.Value as PdfDictionary;
                    // Is external object an image?
                    if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                    {
                      PdfArray pdfArray = null;

                      try
                      {
                        pdfArray = xObject.Elements.GetArray("/Filter");
                      }
                      catch(Exception)
                      {
                        // do nothing
                      }

                      if (pdfArray != null && pdfArray.Elements.Count == 2)
                      {
                        // the "/Filter" field was an array

                        // see if it had two values to indicate it is both JPEG and Deflate encoded
                        if( (pdfArray.Elements[0].ToString() == "/DCTDecode" && pdfArray.Elements[1].ToString() == "/FlateDecode") ||
                            (pdfArray.Elements[1].ToString() == "/DCTDecode" && pdfArray.Elements[0].ToString() == "/FlateDecode") )
                        {
                          byte[] byteArray = xObject.Stream.Value;

                          FlateDecode fd = new FlateDecode();
                          byte[] byteArrayDecompressed = fd.Decode(byteArray);

                          Image image = Image.FromStream(new MemoryStream(byteArrayDecompressed));

                          Page myPage = new Page(image);
                          myDocument.AddPage(myPage);
                        }
                      }
                      else
                      {
                        string filter = xObject.Elements.GetString("/Filter");

                        switch (filter)
                        {
                          case "/DCTDecode":
                            {
                              // this is a directly encoded JPEG image
                              byte[] byteArray = xObject.Stream.Value;

                              Image image = Image.FromStream(new MemoryStream(byteArray));

                              Page myPage = new Page(image);
                              myDocument.AddPage(myPage);
                            }
                            break;
                          case "/FlateDecode":
                            {
                              // potientially this is a BMP/PNG image
                            }
                            break;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        catch(Exception ex)
        {
          string msg = ex.Message;
        }
        
      }

      RefreshControls();
    }
  }
}

