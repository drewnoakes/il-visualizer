using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.DebuggerVisualizers;

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