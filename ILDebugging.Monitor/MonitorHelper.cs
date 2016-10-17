using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ILDebugging.Monitor
{
    public class VisualizerDataEventArgs<T> : EventArgs
    {
        public readonly T VisualizerData;

        public VisualizerDataEventArgs(T data)
        {
            VisualizerData = data;
        }
    }

    public class MonitorStatusChangeEventArgs : EventArgs
    {
        public readonly MonitorStatus Status;

        public MonitorStatusChangeEventArgs(MonitorStatus status)
        {
            Status = status;
        }
    }

    public enum MonitorStatus
    {
        NotMonitoring,
        Monitoring
    }

    public abstract class AbstractXmlDataMonitor<T>
    {
        public abstract void Start();
        public abstract void Stop();

        public delegate void VisualizerDataReadyEventHandler(object sender, VisualizerDataEventArgs<T> e);

        public delegate void MonitorStatusChangeEventHandler(object sender, MonitorStatusChangeEventArgs e);

        public event VisualizerDataReadyEventHandler VisualizerDataReady;
        public event MonitorStatusChangeEventHandler MonitorStatusChange;

        protected void FireStatusChangeEvent(MonitorStatus status)
        {
            if (MonitorStatusChange != null)
            {
                var args = new MonitorStatusChangeEventArgs(status);
                var targetCtrl = MonitorStatusChange.Target as Control;

                if (targetCtrl != null)
                {
                    targetCtrl.Invoke(MonitorStatusChange, this, args);
                }
                else
                {
                    MonitorStatusChange(this, args);
                }
            }
        }

        protected void FireDataReadyEvent(T data)
        {
            if (VisualizerDataReady != null)
            {
                var args = new VisualizerDataEventArgs<T>(data);
                var targetCtrl = VisualizerDataReady.Target as Control;

                if (targetCtrl != null)
                {
                    targetCtrl.Invoke(VisualizerDataReady, this, args);
                }
                else
                {
                    VisualizerDataReady(this, args);
                }
            }
        }
    }

    public class TcpDataMonitor<T> : AbstractXmlDataMonitor<T>
    {
        private TcpListener m_listener;
        private Thread m_listenerThread;
        private readonly int m_port;

        public TcpDataMonitor(int port)
        {
            m_port = port;
        }

        private void ListenerThread()
        {
            m_listener = new TcpListener(IPAddress.Parse("127.0.0.1"), m_port);
            m_listener.Start();

            try
            {
                while (true)
                {
                    while (!m_listener.Pending())
                    {
                        Thread.Sleep(1000);
                    }
                    var client = m_listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(delegate { HandleConnection(client); });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                m_listener.Stop();
            }
        }

        private void HandleConnection(TcpClient client)
        {
            using (client)
            using (var network = client.GetStream())
            {
                var memory = new MemoryStream();
                var buffer = new byte[1024];

                while (true)
                {
                    var received = network.Read(buffer, 0, 1024);
                    if (received == 0)
                        break;
                    memory.Write(buffer, 0, received);
                }

                memory.Position = 0;

                var data = (T)new XmlSerializer(typeof(T)).Deserialize(memory);

                FireDataReadyEvent(data);
            }
        }

        public override void Start()
        {
            m_listenerThread = new Thread(ListenerThread);
            m_listenerThread.Start();
            FireStatusChangeEvent(MonitorStatus.Monitoring);
        }

        public override void Stop()
        {
            m_listenerThread.Abort();
            FireStatusChangeEvent(MonitorStatus.NotMonitoring);
        }
    }
}