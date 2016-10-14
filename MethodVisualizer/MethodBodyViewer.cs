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
using ClrTest.Reflection;

namespace DynamicMethodVisualizer
{
    public partial class MethodBodyViewer : Form
    {
        IVisualizerObjectProvider m_objectProvider;
        MethodBodyInfo m_mbi = null;

        string[] m_shortVersion = null;
        string[] m_longVersion = null;

        public MethodBodyViewer()
        {
            InitializeComponent();
        }

        public void SetObjectProvider(IVisualizerObjectProvider objectProvider)
        {
            m_objectProvider = objectProvider;
            this.GetObjectData();
            this.UpdateForm(false);
        }

        private void GetObjectData()
        {
            using (Stream output = m_objectProvider.GetData())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                m_mbi = (MethodBodyInfo)formatter.Deserialize(output, null);
            }
        }

        private void UpdateForm(bool verbose)
        {
            lblMethodGetType.Text = m_mbi.TypeName;
            txtMethodToString.Text = m_mbi.MethodToString;
            lblFixupSucceed.Text = "Try fix labels: " + (m_mbi.FixupSuccess ? "succeed" : "failed");
            if (verbose)
            {
                if (m_longVersion == null)
                    m_longVersion = m_mbi.Instructions.ToArray();
                richTextBox1.Lines = m_longVersion;
            }
            else
            {
                if (m_shortVersion == null)
                {
                    int count = m_mbi.Instructions.Count;
                    m_shortVersion = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        m_shortVersion[i] = m_mbi.Instructions[i].Remove(9, 20);
                    }
                }
                richTextBox1.Lines = m_shortVersion;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm(chkShowBytes.Checked);
        }

        private void MethodForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void lstFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Font = new Font((string)lstFont.Items[lstFont.SelectedIndex], richTextBox1.Font.Size);
                Update();
            }
            catch { }
        }

        private void MethodBodyViewer_Load(object sender, EventArgs e)
        {
            Array.ForEach(FontFamily.Families, delegate(FontFamily ff) { lstFont.Items.Add(ff.Name); });

            LoadSettings();
        }

        private void MethodBodyViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings() {
            Properties.Settings s = Properties.Settings.Default;

            this.Width = s.WindowWidth;
            this.Height = s.WindowHeight;
            this.Left = s.WindowLeft;
            this.Top = s.WindowTop;
            this.chkShowBytes.Checked = s.ShowBytes;
            this.lstFont.Text = s.Font;
        }

        private void SaveSettings() {
            Properties.Settings s = Properties.Settings.Default;

            s.WindowWidth = this.Width;
            s.WindowHeight = this.Height;
            s.WindowLeft = this.Left;
            s.WindowTop = this.Top;
            s.ShowBytes = this.chkShowBytes.Checked;
            s.Font = this.lstFont.Text;
            s.Save();
        }
    }
}