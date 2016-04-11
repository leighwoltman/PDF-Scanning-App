namespace PDFScanningApp
{
  partial class FormDataSourceSettingsDialog
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
      this.ButtonOK = new System.Windows.Forms.Button();
      this.ButtonCancel = new System.Windows.Forms.Button();
      this.GroupBoxSettings = new System.Windows.Forms.GroupBox();
      this.label6 = new System.Windows.Forms.Label();
      this.NumericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      this.NumericUpDownContrast = new System.Windows.Forms.NumericUpDown();
      this.label4 = new System.Windows.Forms.Label();
      this.NumericUpDownBrightness = new System.Windows.Forms.NumericUpDown();
      this.label3 = new System.Windows.Forms.Label();
      this.ComboBoxResolution = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.ComboBoxPageType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.ComboBoxColorMode = new System.Windows.Forms.ComboBox();
      this.CheckBoxEnableFeeder = new System.Windows.Forms.CheckBox();
      this.CheckBoxUseNativeUI = new System.Windows.Forms.CheckBox();
      this.GroupBoxSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownThreshold)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownContrast)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownBrightness)).BeginInit();
      this.SuspendLayout();
      // 
      // ButtonOK
      // 
      this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ButtonOK.Location = new System.Drawing.Point(134, 283);
      this.ButtonOK.Name = "ButtonOK";
      this.ButtonOK.Size = new System.Drawing.Size(75, 23);
      this.ButtonOK.TabIndex = 0;
      this.ButtonOK.Text = "OK";
      this.ButtonOK.UseVisualStyleBackColor = true;
      // 
      // ButtonCancel
      // 
      this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.ButtonCancel.Location = new System.Drawing.Point(215, 283);
      this.ButtonCancel.Name = "ButtonCancel";
      this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
      this.ButtonCancel.TabIndex = 1;
      this.ButtonCancel.Text = "Cancel";
      this.ButtonCancel.UseVisualStyleBackColor = true;
      // 
      // GroupBoxSettings
      // 
      this.GroupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GroupBoxSettings.Controls.Add(this.label6);
      this.GroupBoxSettings.Controls.Add(this.NumericUpDownThreshold);
      this.GroupBoxSettings.Controls.Add(this.label5);
      this.GroupBoxSettings.Controls.Add(this.NumericUpDownContrast);
      this.GroupBoxSettings.Controls.Add(this.label4);
      this.GroupBoxSettings.Controls.Add(this.NumericUpDownBrightness);
      this.GroupBoxSettings.Controls.Add(this.label3);
      this.GroupBoxSettings.Controls.Add(this.ComboBoxResolution);
      this.GroupBoxSettings.Controls.Add(this.label2);
      this.GroupBoxSettings.Controls.Add(this.ComboBoxPageType);
      this.GroupBoxSettings.Controls.Add(this.label1);
      this.GroupBoxSettings.Controls.Add(this.ComboBoxColorMode);
      this.GroupBoxSettings.Controls.Add(this.CheckBoxEnableFeeder);
      this.GroupBoxSettings.Location = new System.Drawing.Point(12, 55);
      this.GroupBoxSettings.Name = "GroupBoxSettings";
      this.GroupBoxSettings.Size = new System.Drawing.Size(278, 213);
      this.GroupBoxSettings.TabIndex = 4;
      this.GroupBoxSettings.TabStop = false;
      this.GroupBoxSettings.Text = "Settings";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(44, 127);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Threshold";
      // 
      // NumericUpDownThreshold
      // 
      this.NumericUpDownThreshold.Location = new System.Drawing.Point(104, 125);
      this.NumericUpDownThreshold.Name = "NumericUpDownThreshold";
      this.NumericUpDownThreshold.Size = new System.Drawing.Size(134, 20);
      this.NumericUpDownThreshold.TabIndex = 14;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(52, 179);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(46, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Contrast";
      // 
      // NumericUpDownContrast
      // 
      this.NumericUpDownContrast.Location = new System.Drawing.Point(104, 177);
      this.NumericUpDownContrast.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.NumericUpDownContrast.Name = "NumericUpDownContrast";
      this.NumericUpDownContrast.Size = new System.Drawing.Size(134, 20);
      this.NumericUpDownContrast.TabIndex = 12;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(42, 153);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(56, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Brightness";
      // 
      // NumericUpDownBrightness
      // 
      this.NumericUpDownBrightness.Location = new System.Drawing.Point(104, 151);
      this.NumericUpDownBrightness.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.NumericUpDownBrightness.Name = "NumericUpDownBrightness";
      this.NumericUpDownBrightness.Size = new System.Drawing.Size(134, 20);
      this.NumericUpDownBrightness.TabIndex = 10;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(42, 100);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(57, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Resolution";
      // 
      // ComboBoxResolution
      // 
      this.ComboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboBoxResolution.FormattingEnabled = true;
      this.ComboBoxResolution.Location = new System.Drawing.Point(104, 97);
      this.ComboBoxResolution.Name = "ComboBoxResolution";
      this.ComboBoxResolution.Size = new System.Drawing.Size(134, 21);
      this.ComboBoxResolution.TabIndex = 8;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(44, 73);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Page type";
      // 
      // ComboBoxPageType
      // 
      this.ComboBoxPageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboBoxPageType.FormattingEnabled = true;
      this.ComboBoxPageType.Location = new System.Drawing.Point(104, 70);
      this.ComboBoxPageType.Name = "ComboBoxPageType";
      this.ComboBoxPageType.Size = new System.Drawing.Size(134, 21);
      this.ComboBoxPageType.TabIndex = 6;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(39, 46);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Color mode";
      // 
      // ComboBoxColorMode
      // 
      this.ComboBoxColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboBoxColorMode.FormattingEnabled = true;
      this.ComboBoxColorMode.Location = new System.Drawing.Point(104, 43);
      this.ComboBoxColorMode.Name = "ComboBoxColorMode";
      this.ComboBoxColorMode.Size = new System.Drawing.Size(134, 21);
      this.ComboBoxColorMode.TabIndex = 4;
      // 
      // CheckBoxEnableFeeder
      // 
      this.CheckBoxEnableFeeder.AutoSize = true;
      this.CheckBoxEnableFeeder.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.CheckBoxEnableFeeder.Location = new System.Drawing.Point(25, 20);
      this.CheckBoxEnableFeeder.Name = "CheckBoxEnableFeeder";
      this.CheckBoxEnableFeeder.Size = new System.Drawing.Size(92, 17);
      this.CheckBoxEnableFeeder.TabIndex = 0;
      this.CheckBoxEnableFeeder.Text = "Enable feeder";
      this.CheckBoxEnableFeeder.UseVisualStyleBackColor = true;
      // 
      // CheckBoxUseNativeUI
      // 
      this.CheckBoxUseNativeUI.AutoSize = true;
      this.CheckBoxUseNativeUI.Location = new System.Drawing.Point(12, 23);
      this.CheckBoxUseNativeUI.Name = "CheckBoxUseNativeUI";
      this.CheckBoxUseNativeUI.Size = new System.Drawing.Size(93, 17);
      this.CheckBoxUseNativeUI.TabIndex = 6;
      this.CheckBoxUseNativeUI.Text = "Use Native UI";
      this.CheckBoxUseNativeUI.UseVisualStyleBackColor = true;
      this.CheckBoxUseNativeUI.CheckStateChanged += new System.EventHandler(this.CheckBoxUseNativeUI_CheckStateChanged);
      // 
      // FormDataSourceSettingsDialog
      // 
      this.AcceptButton = this.ButtonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.ButtonCancel;
      this.ClientSize = new System.Drawing.Size(302, 318);
      this.Controls.Add(this.CheckBoxUseNativeUI);
      this.Controls.Add(this.GroupBoxSettings);
      this.Controls.Add(this.ButtonCancel);
      this.Controls.Add(this.ButtonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormDataSourceSettingsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Scan Dialog";
      this.GroupBoxSettings.ResumeLayout(false);
      this.GroupBoxSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownThreshold)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownContrast)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownBrightness)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button ButtonOK;
    private System.Windows.Forms.Button ButtonCancel;
    private System.Windows.Forms.GroupBox GroupBoxSettings;
    private System.Windows.Forms.CheckBox CheckBoxEnableFeeder;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox ComboBoxResolution;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox ComboBoxPageType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox ComboBoxColorMode;
    private System.Windows.Forms.NumericUpDown NumericUpDownBrightness;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown NumericUpDownContrast;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.NumericUpDown NumericUpDownThreshold;
    private System.Windows.Forms.CheckBox CheckBoxUseNativeUI;
  }
}