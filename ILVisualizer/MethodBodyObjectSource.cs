using System;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClrTest.Reflection
{
    public class MethodBodyObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var method = target as MethodBase;
            if (method != null)
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