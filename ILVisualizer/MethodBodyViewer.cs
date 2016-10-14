using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ClrTest.Reflection {
    public partial class MethodBodyViewer : Form {
        private IVisualizerObjectProvider m_objectProvider;
        private MethodBodyInfo m_mbi = null;

        public MethodBodyViewer() {
            InitializeComponent();
        }

        public void SetObjectProvider(IVisualizerObjectProvider objectProvider) {
            m_objectProvider = objectProvider;
            this.GetObjectData();
            this.UpdateForm();
        }

        private void GetObjectData() {
            using (Stream output = m_objectProvider.GetData()) {
                BinaryFormatter formatter = new BinaryFormatter();
                m_mbi = (MethodBodyInfo)formatter.Deserialize(output, null);
            }
        }

        private void UpdateForm() {
            lblMethodGetType.Text = m_mbi.TypeName;
            txtMethodToString.Text = m_mbi.MethodToString;

            int cnt = m_mbi.Instructions.Count;
            string[] lines = new string[cnt];
            for (int i = 0; i < cnt; i++)
                lines[i] = m_mbi.Instructions[i];
            richTextBox.Lines = lines;

            this.ActiveControl = richTextBox;
        }

        // allow ESC to close
        private void MethodForm_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void MethodBodyViewer_Load(object sender, EventArgs e) {
            LoadSettings();
            BuildContextMenu();
        }

        private void MethodBodyViewer_FormClosing(object sender, FormClosingEventArgs e) {
            SaveSettings();
        }

        string selectedFontName;
        ToolStripMenuItem selectedFontNameMenuItem;
        int selectedFontSize;
        ToolStripMenuItem selectedFontSizeMenuItem;

        void BuildContextMenu() {
            string[] fontCandidates = new string[] { "Arial", "Consolas", "Courier New", "Lucida Console", "Tahoma", };

            List<string> fontChoices = new List<string>();
            if (Array.IndexOf(fontCandidates, selectedFontName) == -1)
                fontChoices.Add(selectedFontName);

            // only choose those available in current machine
            foreach (FontFamily ff in FontFamily.Families) {
                if (Array.IndexOf(fontCandidates, ff.Name) != -1)
                    fontChoices.Add(ff.Name);
            }

            int nameCount = fontChoices.Count;
            ToolStripMenuItem[] nameItems = new ToolStripMenuItem[nameCount];
            for (int i = 0; i < nameCount; i++) {
                nameItems[i] = new ToolStripMenuItem(fontChoices[i], null, fontNameToolStripMenuItem_Click);
                if (fontChoices[i] == selectedFontName) {
                    nameItems[i].Checked = true;
                    selectedFontNameMenuItem = nameItems[i];
                }
            }
            this.fontNameToolStripMenuItem.DropDownItems.AddRange(nameItems);

            int[] sizeChoices = new int[] { 9, 10, 11, 12, 14, 20 };
            int sizeCount = sizeChoices.Length;

            ToolStripMenuItem[] sizeItems = new ToolStripMenuItem[sizeCount];
            for (int i = 0; i < sizeCount; i++) {
                sizeItems[i] = new ToolStripMenuItem(sizeChoices[i].ToString(), null, fontSizeToolStripMenuItem_Click);
                if (sizeChoices[i] == selectedFontSize) {
                    sizeItems[i].Checked = true;
                    selectedFontSizeMenuItem = sizeItems[i];
                }
            }
            this.fontSizeToolStripMenuItem.DropDownItems.AddRange(sizeItems);

            this.richTextBox.Font = new Font(selectedFontName, selectedFontSize);
        }

        private void LoadSettings() {
            Properties.Settings s = Properties.Settings.Default;

            this.Width = s.WindowWidth;
            this.Height = s.WindowHeight;
            this.Left = s.WindowLeft;
            this.Top = s.WindowTop;

            selectedFontName = s.FontName;
            selectedFontSize = s.FontSize;
        }

        private void SaveSettings() {
            Properties.Settings s = Properties.Settings.Default;

            s.WindowWidth = this.Width;
            s.WindowHeight = this.Height;
            s.WindowLeft = this.Left;
            s.WindowTop = this.Top;

            s.FontName = selectedFontName;
            s.FontSize = selectedFontSize;

            s.Save();
        }

        private void fontNameToolStripMenuItem_Click(object sender, EventArgs e) {
            selectedFontNameMenuItem.Checked = false;
            selectedFontNameMenuItem = sender as ToolStripMenuItem;
            selectedFontNameMenuItem.Checked = true;

            selectedFontName = selectedFontNameMenuItem.Text;
            richTextBox.Font = new Font(selectedFontName, richTextBox.Font.Size);
            Update();
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e) {
            selectedFontSizeMenuItem.Checked = false;
            selectedFontSizeMenuItem = sender as ToolStripMenuItem;
            selectedFontSizeMenuItem.Checked = true;

            selectedFontSize = Int32.Parse(selectedFontSizeMenuItem.Text);
            richTextBox.Font = new Font(richTextBox.Font.Name, selectedFontSize);
            Update();
        }
    }
}