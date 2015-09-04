namespace WebPaparazzi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelPort = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelResolution = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxResolution = new System.Windows.Forms.ToolStripComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonRemove,
            this.toolStripButtonSave,
            this.toolStripButtonStart,
            this.toolStripButtonStop,
            this.toolStripLabelPort,
            this.toolStripTextBoxPort,
            this.toolStripLabelResolution,
            this.toolStripComboBoxResolution});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(754, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::WebPaparazzi.Properties.Resources.plus;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonAdd.Text = "Ajouter";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.Image = global::WebPaparazzi.Properties.Resources.minus;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(82, 22);
            this.toolStripButtonRemove.Text = "Supprimer";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(92, 22);
            this.toolStripButtonSave.Text = "Sauvegarder";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonStart
            // 
            this.toolStripButtonStart.Image = global::WebPaparazzi.Properties.Resources.play2;
            this.toolStripButtonStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStart.Name = "toolStripButtonStart";
            this.toolStripButtonStart.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonStart.Text = "Start";
            this.toolStripButtonStart.Click += new System.EventHandler(this.toolStripButtonStart_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Enabled = false;
            this.toolStripButtonStop.Image = global::WebPaparazzi.Properties.Resources.stop;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripLabelPort
            // 
            this.toolStripLabelPort.Name = "toolStripLabelPort";
            this.toolStripLabelPort.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabelPort.Text = "port:";
            // 
            // toolStripTextBoxPort
            // 
            this.toolStripTextBoxPort.Name = "toolStripTextBoxPort";
            this.toolStripTextBoxPort.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBoxPort.Text = "8080";
            // 
            // toolStripLabelResolution
            // 
            this.toolStripLabelResolution.Name = "toolStripLabelResolution";
            this.toolStripLabelResolution.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabelResolution.Text = "Résolution:";
            // 
            // toolStripComboBoxResolution
            // 
            this.toolStripComboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxResolution.Items.AddRange(new object[] {
            "1080p",
            "720p",
            "1024x768"});
            this.toolStripComboBoxResolution.Name = "toolStripComboBoxResolution";
            this.toolStripComboBoxResolution.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxResolution.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxResolution_SelectedIndexChanged);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(754, 412);
            this.tabControl.TabIndex = 2;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 437);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "WebPaparazzi";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonStart;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPort;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPort;
        private System.Windows.Forms.ToolStripLabel toolStripLabelResolution;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxResolution;

    }
}

