namespace ILDebugging.Visualizer {
    partial class MethodBodyViewer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodBodyViewer));
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMethodToString = new System.Windows.Forms.Label();
            this.lblMethodGetType = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fontNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblMethodToString);
            this.panel3.Controls.Add(this.lblMethodGetType);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(492, 25);
            this.panel3.TabIndex = 7;
            // 
            // lblMethodToString
            // 
            this.lblMethodToString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMethodToString.AutoSize = true;
            this.lblMethodToString.BackColor = System.Drawing.SystemColors.Control;
            this.lblMethodToString.Location = new System.Drawing.Point(110, 6);
            this.lblMethodToString.Name = "lblMethodToString";
            this.lblMethodToString.Size = new System.Drawing.Size(93, 13);
            this.lblMethodToString.TabIndex = 1;
            this.lblMethodToString.Text = "lblMethodToString";
            // 
            // lblMethodGetType
            // 
            this.lblMethodGetType.AutoSize = true;
            this.lblMethodGetType.Location = new System.Drawing.Point(3, 6);
            this.lblMethodGetType.Name = "lblMethodGetType";
            this.lblMethodGetType.Size = new System.Drawing.Size(94, 13);
            this.lblMethodGetType.TabIndex = 1;
            this.lblMethodGetType.Text = "lblMethodGetType";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 380);
            this.panel2.TabIndex = 8;
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox.ContextMenuStrip = this.contextMenuStrip;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.ForeColor = System.Drawing.Color.Black;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(492, 380);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            this.richTextBox.WordWrap = false;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontNameToolStripMenuItem,
            this.fontSizeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(134, 48);
            // 
            // fontNameToolStripMenuItem
            // 
            this.fontNameToolStripMenuItem.Name = "fontNameToolStripMenuItem";
            this.fontNameToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.fontNameToolStripMenuItem.Text = "Font Name";
            // 
            // fontSizeToolStripMenuItem
            // 
            this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
            this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.fontSizeToolStripMenuItem.Text = "Font Size";
            // 
            // MethodBodyViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 405);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "MethodBodyViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IL Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MethodBodyViewer_FormClosing);
            this.Load += new System.EventHandler(this.MethodBodyViewer_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MethodForm_KeyUp);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMethodToString;
        private System.Windows.Forms.Label lblMethodGetType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fontNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
    }
}