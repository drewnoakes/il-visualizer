using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace ILDebugging.Visualizer
{
    public class MethodBodyObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target is MethodBase method)
            {
                try
                {
                    var mbi = MethodBodyInfo.Create(method);

                    var formatter = new BinaryFormatter();
                    formatter.Serialize(outgoingData, mbi);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
