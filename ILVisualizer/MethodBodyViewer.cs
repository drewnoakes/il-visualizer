using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ClrTest.Reflection.Properties;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace ClrTest.Reflection
{
    public partial class MethodBodyViewer : Form
    {
        public MethodBodyViewer()
        {
            InitializeComponent();
        }

        public void SetObjectProvider(IVisualizerObjectProvider objectProvider)
        {
            MethodBodyInfo mbi;
            using (var output = objectProvider.GetData())
                mbi = (MethodBodyInfo)new BinaryFormatter().Deserialize(output, null);

            lblMethodGetType.Text = mbi.TypeName;
            lblMethodToString.Text = mbi.MethodToString;

            var cnt = mbi.Instructions.Count;
            var lines = new string[cnt];
            for (var i = 0; i < cnt; i++)
                lines[i] = mbi.Instructions[i];
            richTextBox.Lines = lines;

            ActiveControl = richTextBox;
        }

        // allow ESC to close
        private void MethodForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void MethodBodyViewer_Load(object sender, EventArgs e)
        {
            LoadSettings();
            BuildContextMenu();
        }

        private void MethodBodyViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private string selectedFontName;
        private ToolStripMenuItem selectedFontNameMenuItem;
        private int selectedFontSize;
        private ToolStripMenuItem selectedFontSizeMenuItem;

        private void BuildContextMenu()
        {
            var fontCandidates = new[] {"Arial", "Consolas", "Courier New", "Lucida Console", "Tahoma"};

            var fontChoices = new List<string>();
            if (Array.IndexOf(fontCandidates, selectedFontName) == -1)
                fontChoices.Add(selectedFontName);

            // only choose those available in current machine
            foreach (var ff in FontFamily.Families)
            {
                if (Array.IndexOf(fontCandidates, ff.Name) != -1)
                    fontChoices.Add(ff.Name);
            }

            var nameCount = fontChoices.Count;
            var nameItems = new ToolStripMenuItem[nameCount];
            for (var i = 0; i < nameCount; i++)
            {
                nameItems[i] = new ToolStripMenuItem(fontChoices[i], null, fontNameToolStripMenuItem_Click);
                if (fontChoices[i] == selectedFontName)
                {
                    nameItems[i].Checked = true;
                    selectedFontNameMenuItem = nameItems[i];
                }
            }
            fontNameToolStripMenuItem.DropDownItems.AddRange(nameItems);

            var sizeChoices = new[] {9, 10, 11, 12, 14, 20};
            var sizeCount = sizeChoices.Length;

            var sizeItems = new ToolStripMenuItem[sizeCount];
            for (var i = 0; i < sizeCount; i++)
            {
                sizeItems[i] = new ToolStripMenuItem(sizeChoices[i].ToString(), null, fontSizeToolStripMenuItem_Click);
                if (sizeChoices[i] == selectedFontSize)
                {
                    sizeItems[i].Checked = true;
                    selectedFontSizeMenuItem = sizeItems[i];
                }
            }
            fontSizeToolStripMenuItem.DropDownItems.AddRange(sizeItems);

            richTextBox.Font = new Font(selectedFontName, selectedFontSize);
        }

        private void LoadSettings()
        {
            var s = Settings.Default;

            Width = s.WindowWidth;
            Height = s.WindowHeight;
            Left = s.WindowLeft;
            Top = s.WindowTop;

            selectedFontName = s.FontName;
            selectedFontSize = s.FontSize;
        }

        private void SaveSettings()
        {
            var s = Settings.Default;

            s.WindowWidth = Width;
            s.WindowHeight = Height;
            s.WindowLeft = Left;
            s.WindowTop = Top;

            s.FontName = selectedFontName;
            s.FontSize = selectedFontSize;

            s.Save();
        }

        private void fontNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFontNameMenuItem.Checked = false;
            selectedFontNameMenuItem = (ToolStripMenuItem)sender;
            selectedFontNameMenuItem.Checked = true;

            selectedFontName = selectedFontNameMenuItem.Text;
            richTextBox.Font = new Font(selectedFontName, richTextBox.Font.Size);
            Update();
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFontSizeMenuItem.Checked = false;
            selectedFontSizeMenuItem = (ToolStripMenuItem)sender;
            selectedFontSizeMenuItem.Checked = true;

            selectedFontSize = int.Parse(selectedFontSizeMenuItem.Text);
            richTextBox.Font = new Font(richTextBox.Font.Name, selectedFontSize);
            Update();
        }
    }
}