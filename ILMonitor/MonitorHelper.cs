using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;

namespace ClrTest.Reflection {
    public class VisualizerDataEventArgs<T> : EventArgs {
        public readonly T VisualizerData;

        public VisualizerDataEventArgs(T data) {
            this.VisualizerData = data;
        }
    }

    public class MonitorStatusChangeEventArgs : EventArgs {
        public readonly MonitorStatus Status;

        public MonitorStatusChangeEventArgs(MonitorStatus status) {
            this.Status = status;
        }
    }

    public enum MonitorStatus {
        NotMonitoring,
        Monitoring,
    }

    public abstract class AbstractXmlDataMonitor<T> {
        public abstract void Start();
        public abstract void Stop();

        public delegate void VisualizerDataReadyEventHandler(object sender, VisualizerDataEventArgs<T> e);
        public delegate void MonitorStatusChangeEventHandler(object sender, MonitorStatusChangeEventArgs e);

        public event VisualizerDataReadyEventHandler VisualizerDataReady;
        public event MonitorStatusChangeEventHandler MonitorStatusChange;

        protected void FireStatusChangeEvent(MonitorStatus status) {
            if (MonitorStatusChange != null) {
                MonitorStatusChangeEventArgs args = new MonitorStatusChangeEventArgs(status);
                Control targetCtrl = MonitorStatusChange.Target as Control;

                if (targetCtrl != null) {
                    targetCtrl.Invoke(MonitorStatusChange, new object[] { this, args });
                } else {
                    MonitorStatusChange(this, args);
                }
            }
        }
        protected void FireDataReadyEvent(T data) {
            if (VisualizerDataReady != null) {
                VisualizerDataEventArgs<T> args = new VisualizerDataEventArgs<T>(data);
                Control targetCtrl = VisualizerDataReady.Target as Control;

                if (targetCtrl != null) {
                    targetCtrl.Invoke(VisualizerDataReady, new object[] { this, args });
                } else {
                    VisualizerDataReady(this, args);
                }
            }
        }
    }

    public class TcpDataMonitor<T> : AbstractXmlDataMonitor<T> {
        private TcpListener m_listener;
        private Thread m_listenerThread;
        private readonly int m_port;

        public TcpDataMonitor(int port) {
            m_port = port;
        }

        private void ListenerThread() {
            m_listener = new TcpListener(IPAddress.Parse("127.0.0.1"), m_port);
            m_listener.Start();

            try {
                while (true) {
                    while (!m_listener.Pending()) {
                        Thread.Sleep(1000);
                    }
                    TcpClient client = m_listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(delegate { HandleConnection(client); });
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            } finally {
                m_listener.Stop();
            }
        }

        private void HandleConnection(TcpClient client) {
            NetworkStream network = client.GetStream();
            MemoryStream memory = new MemoryStream();

            try {
                byte[] buffer = new byte[1024];
                int received = 0;
                while (true) {
                    received = network.Read(buffer, 0, 1024);
                    if (received == 0) {
                        break;
                    } else {
                        memory.Write(buffer, 0, received);
                    }
                }
                XmlSerializer s = new XmlSerializer(typeof(T));
                memory.Position = 0;
                T ret = (T)s.Deserialize(memory);

                FireDataReadyEvent(ret);
            } finally {
                memory.Close();
                network.Close();
                client.Close();
            }
        }

        public override void Start() {
            m_listenerThread = new Thread(new ThreadStart(this.ListenerThread));
            m_listenerThread.Start();
            FireStatusChangeEvent(MonitorStatus.Monitoring);
        }

        public override void Stop() {
            m_listenerThread.Abort();
            FireStatusChangeEvent(MonitorStatus.NotMonitoring);
        }
    }
}
