namespace PDFScanningApp
{
  partial class FormMain
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.ComboBoxScanners = new System.Windows.Forms.ToolStripComboBox();
      this.MenuSettings = new System.Windows.Forms.ToolStripDropDownButton();
      this.MenuSettingsScanner = new System.Windows.Forms.ToolStripMenuItem();
      this.MenuSettingsPrinter = new System.Windows.Forms.ToolStripMenuItem();
      this.MenuSettingsPrinterUsePreview = new System.Windows.Forms.ToolStripMenuItem();
      this.ImageListPages = new System.Windows.Forms.ImageList(this.components);
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.ListViewPages = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PanelPreview = new System.Windows.Forms.Panel();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.ButtonRotateClockwise = new System.Windows.Forms.Button();
      this.ButtonRotateCounterClockwise = new System.Windows.Forms.Button();
      this.ButtonMirrorHorizontally = new System.Windows.Forms.Button();
      this.ButtonMirrorVertically = new System.Windows.Forms.Button();
      this.ButtonLandscape = new System.Windows.Forms.Button();
      this.Button2Sided = new System.Windows.Forms.Button();
      this.ButtonDelete = new System.Windows.Forms.Button();
      this.ButtonDeleteAll = new System.Windows.Forms.Button();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.ButtonScanLetter = new System.Windows.Forms.Button();
      this.ButtonScanLegal = new System.Windows.Forms.Button();
      this.ButtonLoadImages = new System.Windows.Forms.Button();
      this.ButtonLoadPdf = new System.Windows.Forms.Button();
      this.ButtonSavePdf = new System.Windows.Forms.Button();
      this.ButtonPrint = new System.Windows.Forms.Button();
      this.ButtonSaveImages = new System.Windows.Forms.Button();
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ComboBoxScanners,
            this.MenuSettings});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.toolStrip1.Size = new System.Drawing.Size(976, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(49, 22);
      this.toolStripLabel1.Text = "Scanner";
      // 
      // ComboBoxScanners
      // 
      this.ComboBoxScanners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboBoxScanners.DropDownWidth = 100;
      this.ComboBoxScanners.Name = "ComboBoxScanners";
      this.ComboBoxScanners.Size = new System.Drawing.Size(250, 25);
      this.ComboBoxScanners.SelectedIndexChanged += new System.EventHandler(this.ComboBoxScanners_SelectedIndexChanged);
      // 
      // MenuSettings
      // 
      this.MenuSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.MenuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSettingsScanner,
            this.MenuSettingsPrinter});
      this.MenuSettings.Image = ((System.Drawing.Image)(resources.GetObject("MenuSettings.Image")));
      this.MenuSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.MenuSettings.Name = "MenuSettings";
      this.MenuSettings.Size = new System.Drawing.Size(62, 22);
      this.MenuSettings.Text = "Settings";
      // 
      // MenuSettingsScanner
      // 
      this.MenuSettingsScanner.DoubleClickEnabled = true;
      this.MenuSettingsScanner.Name = "MenuSettingsScanner";
      this.MenuSettingsScanner.Size = new System.Drawing.Size(125, 22);
      this.MenuSettingsScanner.Text = "Scanner...";
      this.MenuSettingsScanner.Click += new System.EventHandler(this.MenuSettingsScanner_Click);
      // 
      // MenuSettingsPrinter
      // 
      this.MenuSettingsPrinter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSettingsPrinterUsePreview});
      this.MenuSettingsPrinter.Name = "MenuSettingsPrinter";
      this.MenuSettingsPrinter.Size = new System.Drawing.Size(125, 22);
      this.MenuSettingsPrinter.Text = "Printer";
      // 
      // MenuSettingsPrinterUsePreview
      // 
      this.MenuSettingsPrinterUsePreview.CheckOnClick = true;
      this.MenuSettingsPrinterUsePreview.Name = "MenuSettingsPrinterUsePreview";
      this.MenuSettingsPrinterUsePreview.Size = new System.Drawing.Size(137, 22);
      this.MenuSettingsPrinterUsePreview.Text = "Use Preview";
      // 
      // ImageListPages
      // 
      this.ImageListPages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      this.ImageListPages.ImageSize = new System.Drawing.Size(100, 100);
      this.ImageListPages.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // statusStrip1
      // 
      this.statusStrip1.BackColor = System.Drawing.SystemColors.Window;
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.StatusLabel2});
      this.statusStrip1.Location = new System.Drawing.Point(0, 509);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(976, 22);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // StatusLabel1
      // 
      this.StatusLabel1.Name = "StatusLabel1";
      this.StatusLabel1.Size = new System.Drawing.Size(73, 17);
      this.StatusLabel1.Text = "StatusLabel1";
      // 
      // StatusLabel2
      // 
      this.StatusLabel2.Name = "StatusLabel2";
      this.StatusLabel2.Size = new System.Drawing.Size(73, 17);
      this.StatusLabel2.Text = "StatusLabel2";
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 25);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.ListViewPages);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.PanelPreview);
      this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
      this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
      this.splitContainer1.Size = new System.Drawing.Size(976, 484);
      this.splitContainer1.SplitterDistance = 412;
      this.splitContainer1.TabIndex = 3;
      // 
      // ListViewPages
      // 
      this.ListViewPages.AllowDrop = true;
      this.ListViewPages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.ListViewPages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListViewPages.FullRowSelect = true;
      this.ListViewPages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.ListViewPages.LargeImageList = this.ImageListPages;
      this.ListViewPages.Location = new System.Drawing.Point(0, 0);
      this.ListViewPages.Name = "ListViewPages";
      this.ListViewPages.OwnerDraw = true;
      this.ListViewPages.Size = new System.Drawing.Size(412, 484);
      this.ListViewPages.SmallImageList = this.ImageListPages;
      this.ListViewPages.TabIndex = 0;
      this.ListViewPages.TileSize = new System.Drawing.Size(10, 10);
      this.ListViewPages.UseCompatibleStateImageBehavior = false;
      this.ListViewPages.View = System.Windows.Forms.View.Details;
      this.ListViewPages.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListViewPages_DrawItem);
      this.ListViewPages.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ListViewPages_ItemDrag);
      this.ListViewPages.SelectedIndexChanged += new System.EventHandler(this.ListViewPages_SelectedIndexChanged);
      this.ListViewPages.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListViewPages_DragDrop);
      this.ListViewPages.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListViewPages_DragEnter);
      this.ListViewPages.DragOver += new System.Windows.Forms.DragEventHandler(this.ListViewPages_DragOver);
      this.ListViewPages.DragLeave += new System.EventHandler(this.ListViewPages_DragLeave);
      this.ListViewPages.Layout += new System.Windows.Forms.LayoutEventHandler(this.ListViewPages_Layout);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Width = 128;
      // 
      // PanelPreview
      // 
      this.PanelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PanelPreview.Location = new System.Drawing.Point(0, 39);
      this.PanelPreview.Name = "PanelPreview";
      this.PanelPreview.Size = new System.Drawing.Size(560, 319);
      this.PanelPreview.TabIndex = 4;
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
      this.flowLayoutPanel2.Controls.Add(this.ButtonRotateClockwise);
      this.flowLayoutPanel2.Controls.Add(this.ButtonRotateCounterClockwise);
      this.flowLayoutPanel2.Controls.Add(this.ButtonMirrorHorizontally);
      this.flowLayoutPanel2.Controls.Add(this.ButtonMirrorVertically);
      this.flowLayoutPanel2.Controls.Add(this.ButtonLandscape);
      this.flowLayoutPanel2.Controls.Add(this.Button2Sided);
      this.flowLayoutPanel2.Controls.Add(this.ButtonDelete);
      this.flowLayoutPanel2.Controls.Add(this.ButtonDeleteAll);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
      this.flowLayoutPanel2.Size = new System.Drawing.Size(560, 39);
      this.flowLayoutPanel2.TabIndex = 3;
      // 
      // ButtonRotateClockwise
      // 
      this.ButtonRotateClockwise.AutoSize = true;
      this.ButtonRotateClockwise.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonRotateClockwise.Location = new System.Drawing.Point(8, 8);
      this.ButtonRotateClockwise.Name = "ButtonRotateClockwise";
      this.ButtonRotateClockwise.Size = new System.Drawing.Size(64, 23);
      this.ButtonRotateClockwise.TabIndex = 27;
      this.ButtonRotateClockwise.Text = "Rotate >>";
      this.ButtonRotateClockwise.UseVisualStyleBackColor = true;
      this.ButtonRotateClockwise.Click += new System.EventHandler(this.ButtonRotateClockwise_Click);
      // 
      // ButtonRotateCounterClockwise
      // 
      this.ButtonRotateCounterClockwise.AutoSize = true;
      this.ButtonRotateCounterClockwise.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonRotateCounterClockwise.Location = new System.Drawing.Point(78, 8);
      this.ButtonRotateCounterClockwise.Name = "ButtonRotateCounterClockwise";
      this.ButtonRotateCounterClockwise.Size = new System.Drawing.Size(64, 23);
      this.ButtonRotateCounterClockwise.TabIndex = 33;
      this.ButtonRotateCounterClockwise.Text = "Rotate <<";
      this.ButtonRotateCounterClockwise.UseVisualStyleBackColor = true;
      this.ButtonRotateCounterClockwise.Click += new System.EventHandler(this.ButtonRotateCounterClockwise_Click);
      // 
      // ButtonMirrorHorizontally
      // 
      this.ButtonMirrorHorizontally.AutoSize = true;
      this.ButtonMirrorHorizontally.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonMirrorHorizontally.Location = new System.Drawing.Point(148, 8);
      this.ButtonMirrorHorizontally.Name = "ButtonMirrorHorizontally";
      this.ButtonMirrorHorizontally.Size = new System.Drawing.Size(54, 23);
      this.ButtonMirrorHorizontally.TabIndex = 31;
      this.ButtonMirrorHorizontally.Text = "Mirror-H";
      this.ButtonMirrorHorizontally.UseVisualStyleBackColor = true;
      this.ButtonMirrorHorizontally.Click += new System.EventHandler(this.ButtonMirrorHorizontally_Click);
      // 
      // ButtonMirrorVertically
      // 
      this.ButtonMirrorVertically.AutoSize = true;
      this.ButtonMirrorVertically.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonMirrorVertically.Location = new System.Drawing.Point(208, 8);
      this.ButtonMirrorVertically.Name = "ButtonMirrorVertically";
      this.ButtonMirrorVertically.Size = new System.Drawing.Size(53, 23);
      this.ButtonMirrorVertically.TabIndex = 32;
      this.ButtonMirrorVertically.Text = "Mirror-V";
      this.ButtonMirrorVertically.UseVisualStyleBackColor = true;
      this.ButtonMirrorVertically.Click += new System.EventHandler(this.ButtonMirrorVertically_Click);
      // 
      // ButtonLandscape
      // 
      this.ButtonLandscape.AutoSize = true;
      this.ButtonLandscape.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonLandscape.Location = new System.Drawing.Point(267, 8);
      this.ButtonLandscape.Name = "ButtonLandscape";
      this.ButtonLandscape.Size = new System.Drawing.Size(70, 23);
      this.ButtonLandscape.TabIndex = 29;
      this.ButtonLandscape.Text = "Landscape";
      this.ButtonLandscape.UseVisualStyleBackColor = true;
      this.ButtonLandscape.Click += new System.EventHandler(this.ButtonLandscape_Click);
      // 
      // Button2Sided
      // 
      this.Button2Sided.AutoSize = true;
      this.Button2Sided.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Button2Sided.Location = new System.Drawing.Point(343, 8);
      this.Button2Sided.Name = "Button2Sided";
      this.Button2Sided.Size = new System.Drawing.Size(53, 23);
      this.Button2Sided.TabIndex = 28;
      this.Button2Sided.Text = "2 Sided";
      this.Button2Sided.UseVisualStyleBackColor = true;
      this.Button2Sided.Click += new System.EventHandler(this.Button2Sided_Click);
      // 
      // ButtonDelete
      // 
      this.ButtonDelete.AutoSize = true;
      this.ButtonDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonDelete.Location = new System.Drawing.Point(402, 8);
      this.ButtonDelete.Name = "ButtonDelete";
      this.ButtonDelete.Size = new System.Drawing.Size(48, 23);
      this.ButtonDelete.TabIndex = 26;
      this.ButtonDelete.Text = "Delete";
      this.ButtonDelete.UseVisualStyleBackColor = true;
      this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
      // 
      // ButtonDeleteAll
      // 
      this.ButtonDeleteAll.AutoSize = true;
      this.ButtonDeleteAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonDeleteAll.Location = new System.Drawing.Point(456, 8);
      this.ButtonDeleteAll.Name = "ButtonDeleteAll";
      this.ButtonDeleteAll.Size = new System.Drawing.Size(55, 23);
      this.ButtonDeleteAll.TabIndex = 30;
      this.ButtonDeleteAll.Text = "Clear All";
      this.ButtonDeleteAll.UseVisualStyleBackColor = true;
      this.ButtonDeleteAll.Click += new System.EventHandler(this.ButtonDeleteAll_Click);
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
      this.flowLayoutPanel1.Controls.Add(this.ButtonScanLetter);
      this.flowLayoutPanel1.Controls.Add(this.ButtonScanLegal);
      this.flowLayoutPanel1.Controls.Add(this.ButtonLoadImages);
      this.flowLayoutPanel1.Controls.Add(this.ButtonSaveImages);
      this.flowLayoutPanel1.Controls.Add(this.ButtonLoadPdf);
      this.flowLayoutPanel1.Controls.Add(this.ButtonSavePdf);
      this.flowLayoutPanel1.Controls.Add(this.ButtonPrint);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 358);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
      this.flowLayoutPanel1.Size = new System.Drawing.Size(560, 126);
      this.flowLayoutPanel1.TabIndex = 1;
      // 
      // ButtonScanLetter
      // 
      this.ButtonScanLetter.Location = new System.Drawing.Point(8, 8);
      this.ButtonScanLetter.Name = "ButtonScanLetter";
      this.ButtonScanLetter.Size = new System.Drawing.Size(131, 52);
      this.ButtonScanLetter.TabIndex = 28;
      this.ButtonScanLetter.Text = "Scan 8.5x11";
      this.ButtonScanLetter.UseVisualStyleBackColor = true;
      this.ButtonScanLetter.Click += new System.EventHandler(this.ButtonScanLetter_Click);
      // 
      // ButtonScanLegal
      // 
      this.ButtonScanLegal.Location = new System.Drawing.Point(145, 8);
      this.ButtonScanLegal.Name = "ButtonScanLegal";
      this.ButtonScanLegal.Size = new System.Drawing.Size(131, 52);
      this.ButtonScanLegal.TabIndex = 31;
      this.ButtonScanLegal.Text = "Scan 8.5x14";
      this.ButtonScanLegal.UseVisualStyleBackColor = true;
      this.ButtonScanLegal.Click += new System.EventHandler(this.ButtonScanLegal_Click);
      // 
      // ButtonLoadImages
      // 
      this.ButtonLoadImages.Location = new System.Drawing.Point(282, 8);
      this.ButtonLoadImages.Name = "ButtonLoadImages";
      this.ButtonLoadImages.Size = new System.Drawing.Size(131, 52);
      this.ButtonLoadImages.TabIndex = 30;
      this.ButtonLoadImages.Text = "Load Images";
      this.ButtonLoadImages.UseVisualStyleBackColor = true;
      this.ButtonLoadImages.Click += new System.EventHandler(this.ButtonLoadImages_Click);
      // 
      // ButtonLoadPdf
      // 
      this.ButtonLoadPdf.Location = new System.Drawing.Point(8, 66);
      this.ButtonLoadPdf.Name = "ButtonLoadPdf";
      this.ButtonLoadPdf.Size = new System.Drawing.Size(131, 52);
      this.ButtonLoadPdf.TabIndex = 32;
      this.ButtonLoadPdf.Text = "Load From PDF";
      this.ButtonLoadPdf.UseVisualStyleBackColor = true;
      this.ButtonLoadPdf.Click += new System.EventHandler(this.ButtonLoadPdf_Click);
      // 
      // ButtonSavePdf
      // 
      this.ButtonSavePdf.Location = new System.Drawing.Point(145, 66);
      this.ButtonSavePdf.Name = "ButtonSavePdf";
      this.ButtonSavePdf.Size = new System.Drawing.Size(131, 52);
      this.ButtonSavePdf.TabIndex = 29;
      this.ButtonSavePdf.Text = "Save As PDF";
      this.ButtonSavePdf.UseVisualStyleBackColor = true;
      this.ButtonSavePdf.Click += new System.EventHandler(this.ButtonSavePdf_Click);
      // 
      // ButtonPrint
      // 
      this.ButtonPrint.Location = new System.Drawing.Point(282, 66);
      this.ButtonPrint.Name = "ButtonPrint";
      this.ButtonPrint.Size = new System.Drawing.Size(131, 52);
      this.ButtonPrint.TabIndex = 33;
      this.ButtonPrint.Text = "Print";
      this.ButtonPrint.UseVisualStyleBackColor = true;
      this.ButtonPrint.Click += new System.EventHandler(this.ButtonPrint_Click);
      // 
      // ButtonSaveImages
      // 
      this.ButtonSaveImages.Location = new System.Drawing.Point(419, 8);
      this.ButtonSaveImages.Name = "ButtonSaveImages";
      this.ButtonSaveImages.Size = new System.Drawing.Size(131, 52);
      this.ButtonSaveImages.TabIndex = 34;
      this.ButtonSaveImages.Text = "Save Images";
      this.ButtonSaveImages.UseVisualStyleBackColor = true;
      this.ButtonSaveImages.Click += new System.EventHandler(this.ButtonSaveImages_Click);
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(976, 531);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.toolStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormMain";
      this.Text = "Form1";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
      this.Load += new System.EventHandler(this.FormMain_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel2.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ImageList ImageListPages;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripComboBox ComboBoxScanners;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ListView ListViewPages;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Button ButtonLandscape;
    private System.Windows.Forms.Button ButtonRotateClockwise;
    private System.Windows.Forms.Button Button2Sided;
    private System.Windows.Forms.Button ButtonDelete;
    private System.Windows.Forms.Button ButtonDeleteAll;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button ButtonScanLetter;
    private System.Windows.Forms.Button ButtonScanLegal;
    private System.Windows.Forms.Button ButtonLoadImages;
    private System.Windows.Forms.Button ButtonLoadPdf;
    private System.Windows.Forms.Button ButtonSavePdf;
    private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
    private System.Windows.Forms.Button ButtonPrint;
    private System.Windows.Forms.ToolStripDropDownButton MenuSettings;
    private System.Windows.Forms.ToolStripMenuItem MenuSettingsScanner;
    private System.Windows.Forms.ToolStripMenuItem MenuSettingsPrinter;
    private System.Windows.Forms.ToolStripMenuItem MenuSettingsPrinterUsePreview;
    private System.Windows.Forms.Button ButtonMirrorHorizontally;
    private System.Windows.Forms.Button ButtonMirrorVertically;
    private System.Windows.Forms.Button ButtonRotateCounterClockwise;
    private System.Windows.Forms.Panel PanelPreview;
    private System.Windows.Forms.Button ButtonSaveImages;
  }
}

