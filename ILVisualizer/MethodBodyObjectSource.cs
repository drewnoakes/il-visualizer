using System;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClrTest.Reflection {

    public class MethodBodyObjectSource : VisualizerObjectSource {
        public override void GetData(object target, Stream outgoingData) {
            MethodBase method = target as MethodBase;
            if (method != null) {
                try {
                    MethodBodyInfo mbi = MethodBodyInfo.Create(method);

                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(outgoingData, mbi);
                } catch (Exception) { }
            }
        }
    }
}
