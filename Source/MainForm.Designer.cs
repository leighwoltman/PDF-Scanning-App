namespace PDFScanningApp
{
    partial class MainForm
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
            if (disposing && (components != null))
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.button3 = new System.Windows.Forms.Button();
      this.imagePanel = new System.Windows.Forms.FlowLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.moveLeftButton = new System.Windows.Forms.Button();
      this.moveRightButton = new System.Windows.Forms.Button();
      this.saveButton = new System.Windows.Forms.Button();
      this.deleteButton = new System.Windows.Forms.Button();
      this.imageLoad = new System.Windows.Forms.Button();
      this.rotateButton = new System.Windows.Forms.Button();
      this.sided2Button = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.buttonLandscape = new System.Windows.Forms.Button();
      this.comboBoxResolution = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(12, 253);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(131, 52);
      this.button3.TabIndex = 7;
      this.button3.Text = "Scan 8.5x11";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // imagePanel
      // 
      this.imagePanel.AutoScroll = true;
      this.imagePanel.Location = new System.Drawing.Point(12, 42);
      this.imagePanel.Name = "imagePanel";
      this.imagePanel.Size = new System.Drawing.Size(905, 205);
      this.imagePanel.TabIndex = 15;
      this.imagePanel.WrapContents = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(18, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(97, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Layout Adjustment:";
      // 
      // moveLeftButton
      // 
      this.moveLeftButton.Location = new System.Drawing.Point(121, 13);
      this.moveLeftButton.Name = "moveLeftButton";
      this.moveLeftButton.Size = new System.Drawing.Size(33, 23);
      this.moveLeftButton.TabIndex = 16;
      this.moveLeftButton.Text = "<<";
      this.moveLeftButton.UseVisualStyleBackColor = true;
      this.moveLeftButton.Click += new System.EventHandler(this.moveLeftButton_Click);
      // 
      // moveRightButton
      // 
      this.moveRightButton.Location = new System.Drawing.Point(160, 13);
      this.moveRightButton.Name = "moveRightButton";
      this.moveRightButton.Size = new System.Drawing.Size(33, 23);
      this.moveRightButton.TabIndex = 18;
      this.moveRightButton.Text = ">>";
      this.moveRightButton.UseVisualStyleBackColor = true;
      this.moveRightButton.Click += new System.EventHandler(this.moveRightButton_Click);
      // 
      // saveButton
      // 
      this.saveButton.Location = new System.Drawing.Point(786, 253);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(131, 52);
      this.saveButton.TabIndex = 19;
      this.saveButton.Text = "Save As PDF";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
      // 
      // deleteButton
      // 
      this.deleteButton.Location = new System.Drawing.Point(471, 13);
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new System.Drawing.Size(60, 23);
      this.deleteButton.TabIndex = 20;
      this.deleteButton.Text = "Delete";
      this.deleteButton.UseVisualStyleBackColor = true;
      this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
      // 
      // imageLoad
      // 
      this.imageLoad.Location = new System.Drawing.Point(328, 253);
      this.imageLoad.Name = "imageLoad";
      this.imageLoad.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
      this.imageLoad.Size = new System.Drawing.Size(131, 52);
      this.imageLoad.TabIndex = 21;
      this.imageLoad.Text = "Load Images";
      this.imageLoad.UseVisualStyleBackColor = true;
      this.imageLoad.Click += new System.EventHandler(this.imageLoad_Click);
      // 
      // rotateButton
      // 
      this.rotateButton.Location = new System.Drawing.Point(212, 13);
      this.rotateButton.Name = "rotateButton";
      this.rotateButton.Size = new System.Drawing.Size(54, 23);
      this.rotateButton.TabIndex = 22;
      this.rotateButton.Text = "Rotate";
      this.rotateButton.UseVisualStyleBackColor = true;
      this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
      // 
      // sided2Button
      // 
      this.sided2Button.Location = new System.Drawing.Point(287, 13);
      this.sided2Button.Name = "sided2Button";
      this.sided2Button.Size = new System.Drawing.Size(60, 23);
      this.sided2Button.TabIndex = 23;
      this.sided2Button.Text = "2 Sided";
      this.sided2Button.UseVisualStyleBackColor = true;
      this.sided2Button.Click += new System.EventHandler(this.sided2Button_Click);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(171, 253);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(131, 52);
      this.button1.TabIndex = 24;
      this.button1.Text = "Scan 8.5x14";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // buttonLandscape
      // 
      this.buttonLandscape.Location = new System.Drawing.Point(362, 13);
      this.buttonLandscape.Name = "buttonLandscape";
      this.buttonLandscape.Size = new System.Drawing.Size(72, 23);
      this.buttonLandscape.TabIndex = 25;
      this.buttonLandscape.Text = "Landscape";
      this.buttonLandscape.UseVisualStyleBackColor = true;
      this.buttonLandscape.Click += new System.EventHandler(this.buttonLandscape_Click);
      // 
      // comboBoxResolution
      // 
      this.comboBoxResolution.FormattingEnabled = true;
      this.comboBoxResolution.Location = new System.Drawing.Point(778, 15);
      this.comboBoxResolution.Name = "comboBoxResolution";
      this.comboBoxResolution.Size = new System.Drawing.Size(139, 21);
      this.comboBoxResolution.TabIndex = 26;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(929, 319);
      this.Controls.Add(this.comboBoxResolution);
      this.Controls.Add(this.buttonLandscape);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.sided2Button);
      this.Controls.Add(this.rotateButton);
      this.Controls.Add(this.imageLoad);
      this.Controls.Add(this.deleteButton);
      this.Controls.Add(this.saveButton);
      this.Controls.Add(this.moveRightButton);
      this.Controls.Add(this.moveLeftButton);
      this.Controls.Add(this.imagePanel);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MainForm";
      this.Text = "PDF Scanning Application v1.0";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FlowLayoutPanel imagePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button moveLeftButton;
        private System.Windows.Forms.Button moveRightButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button imageLoad;
        private System.Windows.Forms.Button rotateButton;
        private System.Windows.Forms.Button sided2Button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonLandscape;
        private System.Windows.Forms.ComboBox comboBoxResolution;

    }
}

