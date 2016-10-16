using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using ClrTest.Reflection;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: DebuggerVisualizer(
               typeof(MethodBodyNoVisualizer),
               typeof(MethodBodyObjectSource),
               Target = typeof(MethodBase),
               Description = "Send to IL Monitor")
]

namespace ClrTest.Reflection
{
    internal interface IXmlDataProvider<in T>
    {
        void Dump(T obj);
    }

    internal class TcpClientDataProvider : IXmlDataProvider<MethodBodyInfo>
    {
        private readonly int m_portNumber;

        public TcpClientDataProvider(int port)
        {
            m_portNumber = port;
        }

        public void Dump(MethodBodyInfo mbi)
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(IPAddress.Parse("127.0.0.1"), m_portNumber);

                var memoryStream = new MemoryStream();
                new XmlSerializer(typeof(MethodBodyInfo)).Serialize(memoryStream, mbi);

                memoryStream.Position = 0;

                using (var networkStream = tcpClient.GetStream())
                    memoryStream.CopyTo(networkStream);
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

                IXmlDataProvider<MethodBodyInfo> provider = new TcpClientDataProvider(port: 22017);
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