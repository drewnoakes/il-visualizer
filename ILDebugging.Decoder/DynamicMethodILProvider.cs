using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ILDebugging.Decoder
{
    public class DynamicMethodILProvider : IILProvider
    {
        private static readonly FieldInfo s_fiLen = typeof(ILGenerator).GetField("m_length", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo s_fiStream = typeof(ILGenerator).GetField("m_ILStream", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo s_miBakeByteArray = typeof(ILGenerator).GetMethod("BakeByteArray", BindingFlags.NonPublic | BindingFlags.Instance);

        private readonly DynamicMethod m_method;
        private byte[] m_byteArray;

        public DynamicMethodILProvider(DynamicMethod method)
        {
            m_method = method;
        }

        public byte[] GetByteArray()
        {
            if (m_byteArray == null)
            {
                var ilgen = m_method.GetILGenerator();
                try
                {
                    m_byteArray = (byte[])s_miBakeByteArray.Invoke(ilgen, null) ?? new byte[0];
                }
                catch (TargetInvocationException)
                {
                    var length = (int)s_fiLen.GetValue(ilgen);
                    m_byteArray = new byte[length];
                    Array.Copy((byte[])s_fiStream.GetValue(ilgen), m_byteArray, length);
                }
            }
            return m_byteArray;
        }
    }
}