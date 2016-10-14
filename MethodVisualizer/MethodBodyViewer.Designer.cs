namespace DynamicMethodVisualizer
{
    partial class MethodBodyViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodBodyViewer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFixupSucceed = new System.Windows.Forms.Label();
            this.lstFont = new System.Windows.Forms.ComboBox();
            this.chkShowBytes = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtMethodToString = new System.Windows.Forms.TextBox();
            this.lblMethodGetType = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblFixupSucceed);
            this.panel1.Controls.Add(this.lstFont);
            this.panel1.Controls.Add(this.chkShowBytes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 352);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 21);
            this.panel1.TabIndex = 5;
            // 
            // lblFixupSucceed
            // 
            this.lblFixupSucceed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFixupSucceed.AutoSize = true;
            this.lblFixupSucceed.Location = new System.Drawing.Point(3, 4);
            this.lblFixupSucceed.Name = "lblFixupSucceed";
            this.lblFixupSucceed.Size = new System.Drawing.Size(10, 13);
            this.lblFixupSucceed.TabIndex = 8;
            this.lblFixupSucceed.Text = " ";
            // 
            // lstFont
            // 
            this.lstFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFont.Location = new System.Drawing.Point(331, 0);
            this.lstFont.Name = "lstFont";
            this.lstFont.Size = new System.Drawing.Size(161, 21);
            this.lstFont.TabIndex = 7;
            this.lstFont.Text = "Lucida Console";
            this.lstFont.SelectedIndexChanged += new System.EventHandler(this.lstFont_SelectedIndexChanged);
            // 
            // chkShowBytes
            // 
            this.chkShowBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowBytes.AutoSize = true;
            this.chkShowBytes.Location = new System.Drawing.Point(243, 2);
            this.chkShowBytes.Name = "chkShowBytes";
            this.chkShowBytes.Size = new System.Drawing.Size(82, 17);
            this.chkShowBytes.TabIndex = 6;
            this.chkShowBytes.Text = "Show bytes";
            this.chkShowBytes.UseVisualStyleBackColor = true;
            this.chkShowBytes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtMethodToString);
            this.panel3.Controls.Add(this.lblMethodGetType);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(492, 20);
            this.panel3.TabIndex = 7;
            // 
            // txtMethodToString
            // 
            this.txtMethodToString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodToString.Location = new System.Drawing.Point(96, 0);
            this.txtMethodToString.Name = "txtMethodToString";
            this.txtMethodToString.Size = new System.Drawing.Size(396, 21);
            this.txtMethodToString.TabIndex = 1;
            // 
            // lblMethodGetType
            // 
            this.lblMethodGetType.AutoSize = true;
            this.lblMethodGetType.Location = new System.Drawing.Point(3, 3);
            this.lblMethodGetType.Name = "lblMethodGetType";
            this.lblMethodGetType.Size = new System.Drawing.Size(35, 13);
            this.lblMethodGetType.TabIndex = 0;
            this.lblMethodGetType.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 332);
            this.panel2.TabIndex = 8;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(492, 332);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // MethodBodyViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 373);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "MethodBodyViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ILStream";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MethodBodyViewer_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MethodForm_KeyUp);
            this.Load += new System.EventHandler(this.MethodBodyViewer_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkShowBytes;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtMethodToString;
        private System.Windows.Forms.Label lblMethodGetType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox lstFont;
        private System.Windows.Forms.Label lblFixupSucceed;


    }
}