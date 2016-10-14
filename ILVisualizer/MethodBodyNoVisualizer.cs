using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Net;

[assembly: DebuggerVisualizer(
    typeof(ClrTest.Reflection.MethodBodyNoVisualizer),
    typeof(ClrTest.Reflection.MethodBodyObjectSource),
    Target = typeof(System.Reflection.MethodBase),
    Description = "Send to IL Monitor")
]

namespace ClrTest.Reflection {
    internal interface IXmlDataProvider<T> {
        void Dump(T obj);
    }

    internal class TcpClientDataProvider : IXmlDataProvider<MethodBodyInfo> {
        private int m_portNumber;

        public TcpClientDataProvider(int port) {
            this.m_portNumber = port;
        }

        public void Dump(MethodBodyInfo mbi) {
            TcpClient tcpClient = new TcpClient();
            MemoryStream memoryStream = new MemoryStream();
            try {
                tcpClient.Connect(IPAddress.Parse("127.0.0.1"), this.m_portNumber);

                XmlSerializer s = new XmlSerializer(typeof(MethodBodyInfo));
                s.Serialize(memoryStream, mbi);

                byte[] buffer = memoryStream.ToArray();
                using (NetworkStream networkStream = tcpClient.GetStream()) {
                    networkStream.Write(buffer, 0, buffer.Length);
                }
            } finally {
                if (memoryStream != null)
                    memoryStream.Dispose();
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }
    }

    public class MethodBodyNoVisualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) {
            try {
                MethodBodyInfo mbi;
                using (Stream output = objectProvider.GetData()) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    mbi = (MethodBodyInfo)formatter.Deserialize(output, null);
                }

                IXmlDataProvider<MethodBodyInfo> provider = new TcpClientDataProvider(22017);
                provider.Dump(mbi);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message,
                    "Send to IL Monitor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
