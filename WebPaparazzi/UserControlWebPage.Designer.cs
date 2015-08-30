namespace WebPaparazzi
{
    partial class UserControlWebPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.labelSec = new System.Windows.Forms.Label();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.labelFrequence = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelImage = new System.Windows.Forms.Label();
            this.textBoxImgPath = new System.Windows.Forms.TextBox();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.labelSec);
            this.panel.Controls.Add(this.textBoxFrequency);
            this.panel.Controls.Add(this.labelFrequence);
            this.panel.Controls.Add(this.buttonTest);
            this.panel.Controls.Add(this.buttonRefresh);
            this.panel.Controls.Add(this.labelImage);
            this.panel.Controls.Add(this.textBoxImgPath);
            this.panel.Controls.Add(this.labelUrl);
            this.panel.Controls.Add(this.textBoxUrl);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(477, 97);
            this.panel.TabIndex = 1;
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(139, 69);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(59, 13);
            this.labelSec.TabIndex = 8;
            this.labelSec.Text = "(secondes)";
            // 
            // textBoxFrequency
            // 
            this.textBoxFrequency.Location = new System.Drawing.Point(70, 66);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(63, 20);
            this.textBoxFrequency.TabIndex = 7;
            this.textBoxFrequency.Text = "30";
            this.textBoxFrequency.TextChanged += new System.EventHandler(this.textBoxFrequence_TextChanged);
            // 
            // labelFrequence
            // 
            this.labelFrequence.AutoSize = true;
            this.labelFrequence.Location = new System.Drawing.Point(3, 69);
            this.labelFrequence.Name = "labelFrequence";
            this.labelFrequence.Size = new System.Drawing.Size(61, 13);
            this.labelFrequence.TabIndex = 6;
            this.labelFrequence.Text = "Frequence:";
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest.Location = new System.Drawing.Point(399, 32);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 5;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(399, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "Rafraichir";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.Location = new System.Drawing.Point(3, 37);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(39, 13);
            this.labelImage.TabIndex = 3;
            this.labelImage.Text = "Image:";
            // 
            // textBoxImgPath
            // 
            this.textBoxImgPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxImgPath.Location = new System.Drawing.Point(48, 34);
            this.textBoxImgPath.Name = "textBoxImgPath";
            this.textBoxImgPath.Size = new System.Drawing.Size(345, 20);
            this.textBoxImgPath.TabIndex = 2;
            this.textBoxImgPath.Text = "c:\\test.jpg";
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(3, 8);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(32, 13);
            this.labelUrl.TabIndex = 1;
            this.labelUrl.Text = "URL:";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(48, 5);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(345, 20);
            this.textBoxUrl.TabIndex = 0;
            this.textBoxUrl.TextChanged += new System.EventHandler(this.textBoxUrl_TextChanged);
            // 
            // panelBrowser
            // 
            this.panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrowser.Location = new System.Drawing.Point(0, 97);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(477, 401);
            this.panelBrowser.TabIndex = 3;
            // 
            // UserControlWebPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBrowser);
            this.Controls.Add(this.panel);
            this.Name = "UserControlWebPage";
            this.Size = new System.Drawing.Size(477, 498);
            this.Load += new System.EventHandler(this.UserControlWebPage_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.TextBox textBoxImgPath;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.Label labelFrequence;
        private System.Windows.Forms.Panel panelBrowser;
    }
}
