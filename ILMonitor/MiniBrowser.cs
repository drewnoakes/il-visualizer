using System.Windows.Forms;
using System.IO;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Xml;

namespace ClrTest.Reflection
{
    public partial class MiniBrowser : Form
    {
        public MiniBrowser()
        {
            InitializeComponent();
        }

        private IncrementalMethodBodyInfo m_imbi;

        public IncrementalMethodBodyInfo CurrentData => m_imbi;

        public void UpdateWith(IncrementalMethodBodyInfo imbi)
        {
            m_imbi = imbi;

            var xslt = new XslCompiledTransform();
            using (var sr = new StringReader(Properties.Resources.XSLT))
            {
                using (var xtr = new XmlTextReader(sr))
                {
                    xslt.Load(xtr);
                }
            }

            var serializer = new XmlSerializer(typeof(IncrementalMethodBodyInfo));
            using (var beforeTransform = new MemoryStream())
            {
                var afterTransform = new MemoryStream();
                serializer.Serialize(beforeTransform, m_imbi);

                beforeTransform.Position = 0;
                using (var reader = new XmlTextReader(beforeTransform))
                {
                    xslt.Transform(reader, null, afterTransform);
                }

                afterTransform.Position = 0;
                webBrowser.DocumentStream = afterTransform;
            }
        }
    }
}