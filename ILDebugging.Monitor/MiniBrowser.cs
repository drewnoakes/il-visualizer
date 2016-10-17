using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using ILDebugging.Monitor.Properties;

namespace ILDebugging.Monitor
{
    public partial class MiniBrowser : Form
    {
        public MiniBrowser()
        {
            InitializeComponent();
        }

        public IncrementalMethodBodyInfo CurrentData { get; private set; }

        public void UpdateWith(IncrementalMethodBodyInfo imbi)
        {
            CurrentData = imbi;

            var xslt = new XslCompiledTransform();
            using (var sr = new StringReader(Resources.XSLT))
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
                serializer.Serialize(beforeTransform, CurrentData);

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