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

namespace ClrTest.Reflection
{
    internal interface IXmlDataProvider<T>
    {
        void Dump(T obj);
    }

    internal class TcpClientDataProvider : IXmlDataProvider<MethodBodyInfo>
    {
        private readonly int m_portNumber;

        public TcpClientDataProvider(int port)
        {
            this.m_portNumber = port;
        }

        public void Dump(MethodBodyInfo mbi)
        {
            var tcpClient = new TcpClient();
            var memoryStream = new MemoryStream();
            try
            {
                tcpClient.Connect(IPAddress.Parse("127.0.0.1"), this.m_portNumber);

                var s = new XmlSerializer(typeof(MethodBodyInfo));
                s.Serialize(memoryStream, mbi);

                var buffer = memoryStream.ToArray();
                using (var networkStream = tcpClient.GetStream())
                {
                    networkStream.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }
    }

    public class MethodBodyNoVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            {
                MethodBodyInfo mbi;
                using (var output = objectProvider.GetData())
                {
                    var formatter = new BinaryFormatter();
                    mbi = (MethodBodyInfo)formatter.Deserialize(output, null);
                }

                IXmlDataProvider<MethodBodyInfo> provider = new TcpClientDataProvider(22017);
                provider.Dump(mbi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Send to IL Monitor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}