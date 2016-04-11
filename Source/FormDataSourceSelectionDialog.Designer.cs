namespace PDFScanningApp
{
  partial class FormDataSourceSelectionDialog
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
      this.CheckBoxUseNativeUI = new System.Windows.Forms.CheckBox();
      this.ComboBoxDataSources = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // ButtonOK
      // 
      this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ButtonOK.Location = new System.Drawing.Point(116, 75);
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
      this.ButtonCancel.Location = new System.Drawing.Point(197, 75);
      this.ButtonCancel.Name = "ButtonCancel";
      this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
      this.ButtonCancel.TabIndex = 1;
      this.ButtonCancel.Text = "Cancel";
      this.ButtonCancel.UseVisualStyleBackColor = true;
      // 
      // CheckBoxUseNativeUI
      // 
      this.CheckBoxUseNativeUI.AutoSize = true;
      this.CheckBoxUseNativeUI.Location = new System.Drawing.Point(12, 39);
      this.CheckBoxUseNativeUI.Name = "CheckBoxUseNativeUI";
      this.CheckBoxUseNativeUI.Size = new System.Drawing.Size(93, 17);
      this.CheckBoxUseNativeUI.TabIndex = 2;
      this.CheckBoxUseNativeUI.Text = "Use Native UI";
      this.CheckBoxUseNativeUI.UseVisualStyleBackColor = true;
      // 
      // ComboBoxDataSources
      // 
      this.ComboBoxDataSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboBoxDataSources.FormattingEnabled = true;
      this.ComboBoxDataSources.Location = new System.Drawing.Point(13, 12);
      this.ComboBoxDataSources.Name = "ComboBoxDataSources";
      this.ComboBoxDataSources.Size = new System.Drawing.Size(259, 21);
      this.ComboBoxDataSources.TabIndex = 3;
      // 
      // FormDataSourceDialog
      // 
      this.AcceptButton = this.ButtonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.ButtonCancel;
      this.ClientSize = new System.Drawing.Size(284, 110);
      this.Controls.Add(this.ComboBoxDataSources);
      this.Controls.Add(this.CheckBoxUseNativeUI);
      this.Controls.Add(this.ButtonCancel);
      this.Controls.Add(this.ButtonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormDataSourceDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Select Data Source";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button ButtonOK;
    private System.Windows.Forms.Button ButtonCancel;
    private System.Windows.Forms.CheckBox CheckBoxUseNativeUI;
    private System.Windows.Forms.ComboBox ComboBoxDataSources;
  }
}