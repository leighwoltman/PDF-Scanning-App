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
      this.ButtonScanSettings = new System.Windows.Forms.ToolStripButton();
      this.ImageListPages = new System.Windows.Forms.ImageList(this.components);
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.ListViewPages = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PictureBoxPreview = new System.Windows.Forms.PictureBox();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.ButtonLandscape = new System.Windows.Forms.Button();
      this.ButtonRotate = new System.Windows.Forms.Button();
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
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PictureBoxPreview)).BeginInit();
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
            this.ButtonScanSettings});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.toolStrip1.Size = new System.Drawing.Size(805, 25);
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
      // ButtonScanSettings
      // 
      this.ButtonScanSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.ButtonScanSettings.Image = ((System.Drawing.Image)(resources.GetObject("ButtonScanSettings.Image")));
      this.ButtonScanSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ButtonScanSettings.Name = "ButtonScanSettings";
      this.ButtonScanSettings.Size = new System.Drawing.Size(53, 22);
      this.ButtonScanSettings.Text = "Settings";
      this.ButtonScanSettings.Click += new System.EventHandler(this.ButtonScanSettings_Click);
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
      this.statusStrip1.Size = new System.Drawing.Size(805, 22);
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
      this.splitContainer1.Panel2.Controls.Add(this.PictureBoxPreview);
      this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
      this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
      this.splitContainer1.Size = new System.Drawing.Size(805, 484);
      this.splitContainer1.SplitterDistance = 356;
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
      this.ListViewPages.MultiSelect = false;
      this.ListViewPages.Name = "ListViewPages";
      this.ListViewPages.OwnerDraw = true;
      this.ListViewPages.Size = new System.Drawing.Size(356, 484);
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
      // PictureBoxPreview
      // 
      this.PictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PictureBoxPreview.Location = new System.Drawing.Point(0, 39);
      this.PictureBoxPreview.Name = "PictureBoxPreview";
      this.PictureBoxPreview.Size = new System.Drawing.Size(445, 319);
      this.PictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.PictureBoxPreview.TabIndex = 4;
      this.PictureBoxPreview.TabStop = false;
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.flowLayoutPanel2.Controls.Add(this.ButtonLandscape);
      this.flowLayoutPanel2.Controls.Add(this.ButtonRotate);
      this.flowLayoutPanel2.Controls.Add(this.Button2Sided);
      this.flowLayoutPanel2.Controls.Add(this.ButtonDelete);
      this.flowLayoutPanel2.Controls.Add(this.ButtonDeleteAll);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
      this.flowLayoutPanel2.Size = new System.Drawing.Size(445, 39);
      this.flowLayoutPanel2.TabIndex = 3;
      // 
      // ButtonLandscape
      // 
      this.ButtonLandscape.AutoSize = true;
      this.ButtonLandscape.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonLandscape.Location = new System.Drawing.Point(8, 8);
      this.ButtonLandscape.Name = "ButtonLandscape";
      this.ButtonLandscape.Size = new System.Drawing.Size(70, 23);
      this.ButtonLandscape.TabIndex = 29;
      this.ButtonLandscape.Text = "Landscape";
      this.ButtonLandscape.UseVisualStyleBackColor = true;
      this.ButtonLandscape.Click += new System.EventHandler(this.ButtonLandscape_Click);
      // 
      // ButtonRotate
      // 
      this.ButtonRotate.AutoSize = true;
      this.ButtonRotate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ButtonRotate.Location = new System.Drawing.Point(84, 8);
      this.ButtonRotate.Name = "ButtonRotate";
      this.ButtonRotate.Size = new System.Drawing.Size(49, 23);
      this.ButtonRotate.TabIndex = 27;
      this.ButtonRotate.Text = "Rotate";
      this.ButtonRotate.UseVisualStyleBackColor = true;
      this.ButtonRotate.Click += new System.EventHandler(this.ButtonRotate_Click);
      // 
      // Button2Sided
      // 
      this.Button2Sided.AutoSize = true;
      this.Button2Sided.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Button2Sided.Location = new System.Drawing.Point(139, 8);
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
      this.ButtonDelete.Location = new System.Drawing.Point(198, 8);
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
      this.ButtonDeleteAll.Location = new System.Drawing.Point(252, 8);
      this.ButtonDeleteAll.Name = "ButtonDeleteAll";
      this.ButtonDeleteAll.Size = new System.Drawing.Size(88, 23);
      this.ButtonDeleteAll.TabIndex = 30;
      this.ButtonDeleteAll.Text = "Clear All Pages";
      this.ButtonDeleteAll.UseVisualStyleBackColor = true;
      this.ButtonDeleteAll.Click += new System.EventHandler(this.ButtonDeleteAll_Click);
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.flowLayoutPanel1.Controls.Add(this.ButtonScanLetter);
      this.flowLayoutPanel1.Controls.Add(this.ButtonScanLegal);
      this.flowLayoutPanel1.Controls.Add(this.ButtonLoadImages);
      this.flowLayoutPanel1.Controls.Add(this.ButtonLoadPdf);
      this.flowLayoutPanel1.Controls.Add(this.ButtonSavePdf);
      this.flowLayoutPanel1.Controls.Add(this.ButtonPrint);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 358);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
      this.flowLayoutPanel1.Size = new System.Drawing.Size(445, 126);
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
      this.ButtonLoadImages.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
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
      this.ButtonLoadPdf.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
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
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(805, 531);
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
      ((System.ComponentModel.ISupportInitialize)(this.PictureBoxPreview)).EndInit();
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
    private System.Windows.Forms.ToolStripButton ButtonScanSettings;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ListView ListViewPages;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.PictureBox PictureBoxPreview;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Button ButtonLandscape;
    private System.Windows.Forms.Button ButtonRotate;
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
  }
}

