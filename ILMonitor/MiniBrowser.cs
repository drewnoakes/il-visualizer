using System.Windows.Forms;
using System.IO;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Xml;

namespace ClrTest.Reflection {
    public partial class MiniBrowser : Form {
        public MiniBrowser() {
            InitializeComponent();
        }

        private IncrementalMethodBodyInfo m_imbi;

        public IncrementalMethodBodyInfo CurrentData {
            get {
                return m_imbi;
            }
        }

        public void UpdateWith(IncrementalMethodBodyInfo imbi) {
            m_imbi = imbi;

            XslCompiledTransform xslt = new XslCompiledTransform();
            using (StringReader sr = new StringReader(Properties.Resources.XSLT)) {
                using (XmlTextReader xtr = new XmlTextReader(sr)) {
                    xslt.Load(xtr);
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(IncrementalMethodBodyInfo));
            using (MemoryStream beforeTransform = new MemoryStream()) {
                MemoryStream afterTransform = new MemoryStream();
                serializer.Serialize(beforeTransform, m_imbi);

                beforeTransform.Position = 0;
                using (XmlTextReader reader = new XmlTextReader(beforeTransform)) {
                    xslt.Transform(reader, null, afterTransform);
                }

                afterTransform.Position = 0;
                webBrowser.DocumentStream = afterTransform;
            }
        }
    }
}