using System;
using System.Reflection.Emit;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization.Formatters.Binary;
using ClrTest.Reflection;

namespace DynamicMethodVisualizer
{
    public class MethodBodyObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            DynamicMethod method = target as DynamicMethod;

            if (method != null)
            {
                try
                {
                    MethodBodyInfo info = new MethodBodyInfo();
                    info.TypeName = method.GetType().Name;
                    info.MethodToString = method.ToString();

                    ILReader reader = new ILReader(method);
                    foreach (ILInstruction instr in reader)
                        info.Instructions.Add(instr.ToString());

                    info.FixupSuccess = reader.FixupSuccess;

                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(outgoingData, info);
                }
                catch (Exception) { }
            }
        }
    }

}
